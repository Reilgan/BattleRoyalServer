using ExitGames.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using gameServer.Common;
using Photon.SocketServer;
using ExitGames.Logging;

namespace gameServer
{
    public class ClientsPool
    {
        public static readonly ClientsPool Instance = new ClientsPool();
        public Dictionary<int, GameClient> Clients {get; set; }

        public List<Room> Rooms_1x1 { get; set; }

        private readonly ReaderWriterLockSlim readWriteLock;

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public ClientsPool() {
            Clients = new Dictionary<int, GameClient>();
            Rooms_1x1 = new List<Room>();
            readWriteLock = new ReaderWriterLockSlim();
        }

        public bool IsContain(int id)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {

                return Clients.ContainsKey(id);
            }

        }
        public void Enter(GameClient client)
        {
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients[client.Player.Id] = client;
            }
        }
        public GameClient getById(int id)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                return Clients[id];
            }
        }
        public void Exit(int id)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients.Remove(id);
            }

        }

        public void FindRoom(int id, byte roomType, SendParameters sendParameters) 
        {
            switch (roomType) 
            {
                case ((byte)RoomTypeCode.Room_1x1):
                {
                    foreach (Room room in Rooms_1x1) 
                    {
                        if (room.Clients.Count < 2)
                        {
                            room.Enter(Clients[id], sendParameters);
                            return;
                        }
                    }
                    Room newRoom = new Room(roomType);
                    Rooms_1x1.Add(newRoom);
                    newRoom.Enter(Clients[id], sendParameters);
                    break; 
                }
            }
        }

        ~ClientsPool()
        {
            readWriteLock.Dispose();
        }

    }
}
