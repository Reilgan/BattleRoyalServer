using ExitGames.Logging;
using gameServer.Common;
using gameServer.Operations;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masterServer
{
    public class MasterClient : ClientPeer
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
       
        public StructPlayer Player { get; private set; }

        public MasterClient(InitRequest initRequest) : base(initRequest)
        {
            log.Debug("Player connection masterServer ip: " + initRequest.RemoteIP);
        }

        protected override void OnDisconnect(global::PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            MasterClientsPool.Instance.RemoveClient(this);
            log.Debug("Disconnected: " + Player.Name);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)OperationCode.Login:
                    {
                        LoginHandler(operationRequest, sendParameters);
                        break;
                    }
            }

        }

        #region handlers
        private void LoginHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var loginRequest = new Login(Protocol, operationRequest);
            if (!loginRequest.IsValid)
            {
                SendOperationResponse(loginRequest.GetResponse(ErrorCode.InvalidParametrs), sendParameters);
                return;
            }

            // Поулчение игрока из базы данных, если игрок новый создание
            bool isExistName = MasterClientsPool.DataBase.IsExistPlayerName(loginRequest.CharactedName);
            if (isExistName == false)
            {
                StructPlayer newPlayer = MasterClientsPool.DataBase.CreateNewPlayer(loginRequest.CharactedName);
                if (newPlayer.Id == -1)
                {
                    SendOperationResponse(loginRequest.GetResponse(ErrorCode.ErrorCreatePlayer), sendParameters);
                    return;
                }
                Player = newPlayer;
            }
            else 
            {
                StructPlayer player = MasterClientsPool.DataBase.getStructPlayerByName((string)loginRequest.CharactedName);
                if (player.Id == -1)
                {
                    SendOperationResponse(loginRequest.GetResponse(ErrorCode.ErrorGetPlayer), sendParameters);
                    return;
                }
                Player = player;
            }

            // Проверка подключен ли  игрок с таким именем к серверу
            if (MasterClientsPool.Instance.IsContain(Player))
            {
                SendOperationResponse(loginRequest.GetResponse(ErrorCode.NameIsExist), sendParameters);
                return;
            }

            MasterClientsPool.Instance.AddClient(this);
            OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
            resp.Parameters = Player.SerializationPlayerToDict();
            SendOperationResponse(resp, sendParameters);
        }
        #endregion

    }
}
