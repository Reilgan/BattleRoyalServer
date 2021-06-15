using ExitGames.Threading;
using gameServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace masterServer
{
    public class MasterClientsPool
    {
        public static readonly MasterClientsPool Instance = new MasterClientsPool();
        public Dictionary<int, MasterClient> Clients { get; private set; }
        private readonly ReaderWriterLockSlim readWriteLock;
        public static ExchangePsql DataBase { get; private set; }

        public MasterClientsPool() {
            Clients = new Dictionary<int, MasterClient>();
            readWriteLock = new ReaderWriterLockSlim();
            DataBase = new ExchangePsql("localhost",
                                        "5432",
                                        "masterserver",
                                        "masterserver",
                                        "masterServer");
        }

        public bool IsContain(StructPlayer player)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                return Clients.ContainsKey(player.Id);
            }

        }
        public void AddClient(MasterClient client)
        {
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients.Add(client.Player.Id, client);
            }
        }
        public MasterClient getByName(string name)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                return Clients.Values.ToList().Find(n => n.Player.Name.Equals(name));
            }
        }
        public void RemoveClient(MasterClient client)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients.Remove(client.Player.Id);
            }

        }
        ~MasterClientsPool()
        {
            readWriteLock.Dispose();
        }
    }
}
