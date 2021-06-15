using ExitGames.Logging;
using gameServer.Common;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static gameServer.Common.StructPlayer;

namespace masterServer
{
    public class ExchangePsql
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        #region parametrs
        NpgsqlParameter npgSqlParameterName = new NpgsqlParameter("@name", NpgsqlTypes.NpgsqlDbType.Varchar);
        NpgsqlParameter npgSqlParameterTemplate = new NpgsqlParameter("@template", NpgsqlTypes.NpgsqlDbType.Jsonb);
        #endregion

        private string Host { get; set; }
        private string Port { get; set; }
        private string DbName { get; set; }
        private string Password { get; set; }
        private string User { get; set; }

        private NpgsqlConnection Connection { get; set; }

        public ExchangePsql(string host, string port, string user, string dbName, string password)
        {
            Host = host;
            Port = port;
            DbName = dbName;
            Password = password;
            User = user;
            String connectionString = String.Format("Server={0};Port={1};Username={2};Password={3};Database={4};",
                                                    Host, Port, User, Password, DbName);

            Connection = new NpgsqlConnection(connectionString);
        }

        public StructPlayer CreateNewPlayer(string name)
        {
            StructPlayer playerStruct = new StructPlayer();
            Connection.Open();
            NpgsqlCommand commandCreateNewPlayer = new NpgsqlCommand("INSERT INTO users(name, template) VALUES (@name, @template)", Connection);
            npgSqlParameterName.Value = name;

            Info_ playerInfo = StructPlayer.getNUllPlayerTemplate();
            npgSqlParameterTemplate.Value = JsonConvert.SerializeObject(playerInfo);
            commandCreateNewPlayer.Parameters.AddRange(new NpgsqlParameter[] { npgSqlParameterName, npgSqlParameterTemplate });
            int count = commandCreateNewPlayer.ExecuteNonQuery();
            if (count == 1)
            {
                List<int> idlist = new List<int>();
                String sql = String.Format("SELECT id FROM users where name = '{0}'", name);
                NpgsqlCommand npgSqlCommandNewId = new NpgsqlCommand(sql, Connection);
                NpgsqlDataReader npgSqlDataReader = npgSqlCommandNewId.ExecuteReader();
                if (npgSqlDataReader.HasRows)
                {
                    foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    idlist.Add((int)dbDataRecord["id"]);
                }
                if (idlist.Count == 1)
                {
                    playerStruct.Id = idlist.ElementAt(0);
                    playerStruct.Name = name;
                    playerStruct.Info = playerInfo;
                }
                else
                { 
                    //TODO Исключение на случай нахождение 2-х id с одинвковыми именами
                }
            }

            Connection.Close();
            return playerStruct;
        }

        public StructPlayer getStructPlayerByName(string name)
        {
            StructPlayer playerStruct = new StructPlayer();
            Connection.Open();
            List<int> idlist = new List<int>();
            List<string> namesList = new List<string>();
            List<object> templatesList = new List<object>();
            String sql = String.Format("SELECT * FROM users where name = '{0}'", name);
            NpgsqlCommand npgSqlCommandNewId = new NpgsqlCommand(sql, Connection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommandNewId.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    idlist.Add((int)dbDataRecord["id"]);
                    namesList.Add((string)dbDataRecord["name"]);
                    templatesList.Add(dbDataRecord["template"]);
                }
            }

            if (idlist.Count == 1 & namesList.Count == 1 & templatesList.Count == 1)
            {
                playerStruct.Id = idlist.ElementAt(0);
                playerStruct.Name = namesList.ElementAt(0);
                playerStruct.Info = JsonConvert.DeserializeObject<Info_>((string)templatesList.ElementAt(0));
            }
            else
            {
                //TODO Исключение на случай нахождение 2-х id с одинвковыми именами
            }

            Connection.Close();
            return playerStruct;
        }

        public bool IsExistPlayerName(string name) 
        {
            List<string> names = getPlayersNames();
            foreach (string n in names)
            {
                if (n == name)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> getPlayersNames() 
        {
            List<string> names = new List<string>();
            Connection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand("SELECT name FROM users", Connection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    names.Add((string)dbDataRecord["name"]);
            }
            Connection.Close();
            return names;
        }
    }
}
