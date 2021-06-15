using ExitGames.Logging;
using ExitGames.Threading;
using gameServer.Common;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gameServer
{

    public class Room
    {
        public Guid IdRoom { get; private set; }

        public Dictionary<int, GameClient> Clients { get; private set; }

        private readonly ReaderWriterLockSlim readWriteLock;

        private byte RoomType { get; set; }

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Room(byte roomType) 
        {
            Clients = new Dictionary<int, GameClient>();
            readWriteLock = new ReaderWriterLockSlim();
            IdRoom = Guid.NewGuid();
            RoomType = roomType;

        }
        public bool IsContain(int id)
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                return Clients.ContainsKey(id);
            }

        }
        public void Enter(GameClient client, SendParameters sendParameters)
        {
            ClientsPool.Instance.Exit(client.Player.Id);
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients[client.Player.Id] = client;
            }
            client.Conteiner = this;
            CheckRoomReady(sendParameters);
        }

        private void CheckRoomReady(SendParameters sendParameters) 
        {
            switch (RoomType) 
            {
                case ((byte)RoomTypeCode.Room_1x1):
                    {
                        if (Clients.Count == 2)
                        {
                            RoomReadyEvent(sendParameters);
                        }
                        break;
                    }
            
            }
        }

        private void RoomReadyEvent(SendParameters sendParameters) 
        {
            EventData eventData = new EventData((byte)EventCode.RoomReady);
            Dictionary<byte, object> param = new Dictionary<byte, object>();
            param.Add((byte)ParameterCode.IdRoom, IdRoom.ToString());
            param.Add((byte)ParameterCode.RoomType, RoomType);
            eventData.Parameters = param;
            eventData.SendTo(Clients.Values, sendParameters);
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
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                Clients.Remove(id);
                if (Clients.Count == 0)
                {
                    ClientsPool.Instance.Rooms_1x1.Remove(this);
                }
            }

        }

        ~Room()
        {
            readWriteLock.Dispose();
        }
    }
}
