using ExitGames.Logging;
using System.Collections.Generic;
using PhotonHostRuntimeInterfaces;
using System;
using gameServer.Common;
using gameServer.Operations;
using System.Linq;
using Photon.SocketServer;

namespace gameServer
{
    public class GameClient : ClientPeer
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public StructPlayer Player { get; private set; }

        public object Conteiner { get; set; }

        public GameClient(InitRequest initRequest) : base(initRequest)
        {
            log.Debug("Player connection ip: " + initRequest.RemoteIP);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if (Conteiner is Room)
            {
                Room room = (Room)Conteiner;
                room.Exit(Player.Id);
                return;
            }

            if (Conteiner is ClientsPool)
            {
                ClientsPool clientsPool = (ClientsPool)Conteiner;
                clientsPool.Exit(Player.Id);
                return;
            }
            log.Debug("Disconnected: " + Player.Name);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)OperationCode.EnterInGameServer:
                    {
                        EnterInGameServerHandler(operationRequest, sendParameters);
                        break;
                    }

                case (byte)OperationCode.ExitFromGameServer:
                    {
                        ExitFromGameServerHandler(operationRequest, sendParameters);
                        break;
                    }

                case (byte)OperationCode.FindRoom:
                    {
                        FindRoomHandler(operationRequest, sendParameters);
                        break;
                    }

                case (byte)OperationCode.SendChatMessage:
                    {
                        //SendChatMessageHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.GetRecentChatMessages:
                    {
                        GetRecentChatMessageHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.Move:
                    {
                        MoveHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.GetPlayersInRoom:
                    {
                        GetPlayersInRoomHandler(operationRequest, sendParameters);
                        break;
                    }
                default:
                    log.Debug("Unknown OperationRequest recv: " + operationRequest.OperationCode);
                    break;
            }
        }

        #region handler for requests
        private void GetPlayersInRoomHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            if (Conteiner is Room)
            {
                Room room = (Room)Conteiner;
                Dictionary<byte, object> param = new Dictionary<byte, object>();
                foreach (GameClient client in room.Clients.Values)
                {
                    OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
                    resp.Parameters = client.Player.SerializationPlayerToDict();
                    resp.Parameters.Add((byte)ParameterCode.positionX, (float)0.0);
                    resp.Parameters.Add((byte)ParameterCode.positionY, (float)0.0);
                    resp.Parameters.Add((byte)ParameterCode.positionZ, (float)0.0);
                    resp.Parameters.Add((byte)ParameterCode.rotationX, (float)0.0);
                    resp.Parameters.Add((byte)ParameterCode.rotationY, (float)0.0);
                    resp.Parameters.Add((byte)ParameterCode.rotationZ, (float)0.0);
                    resp.Parameters.Add((byte)ParameterCode.rotationW, (float)0.0);
                    SendOperationResponse(resp, sendParameters);
                }
            }

        }
        private void MoveHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var moveRequest = new Move(Protocol, operationRequest);
            var eventDataMove = new EventData((byte)EventCode.Move);
            eventDataMove.Parameters = moveRequest.GetParametersForEvent();
            Room room = (Room)Conteiner; 
            Dictionary<int, GameClient> clientsForEven = new Dictionary<int, GameClient>(room.Clients);
            clientsForEven.Remove(this.Player.Id);
            eventDataMove.SendTo(clientsForEven.Values, sendParameters);
        }
        private void GetRecentChatMessageHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // Отправка истории сообщений
            // Создание event для каждого client
            string allMessages = Chat.Instance.GetRecentMessages().Aggregate((i, j) => i + "\r\n" + j);
            var allMessagesEventData = new EventData((byte)EventCode.ChatMessage);
            allMessagesEventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, allMessages } };
            allMessagesEventData.SendTo(new GameClient[] { this }, sendParameters);
        }
        private void EnterInGameServerHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var EnterRequest = new EnterInGameServer(Protocol, operationRequest);
/*            if (!EnterRequest.IsValid)
            {
                SendOperationResponse(EnterRequest.GetResponse(ErrorCode.InvalidParametrs), sendParameters);
                return;
            }*/
            StructPlayer player = new StructPlayer();
            player.DeserializationPlayerFromDict(operationRequest.Parameters);
            if (ClientsPool.Instance.IsContain(player.Id))
            {
                SendOperationResponse(EnterRequest.GetResponse(ErrorCode.NameIsExist), sendParameters);
                return;
            }
            Player = player;
            ClientsPool.Instance.Enter(this);
            OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
            SendOperationResponse(resp, sendParameters);
        }
        private void ExitFromGameServerHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var eventData = new EventData((byte)EventCode.PlayerExitFromGameServer);
            eventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.Id, Player.Id } };
            Room room = (Room)Conteiner;
            log.Debug("ExitPlayer: " + Player.Id);
            log.Debug("Clients: " + room.Clients.Values);
            eventData.SendTo(room.Clients.Values, sendParameters);
        }
        private void FindRoomHandler(OperationRequest operationRequest, SendParameters sendParameters) 
        {
            FindRoom findRoomRequest = new FindRoom(Protocol, operationRequest);
            ClientsPool.Instance.FindRoom(findRoomRequest.IdPlayer, findRoomRequest.RoomType, sendParameters);
        }

/*        private void SendChatMessageHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var chatRequest = new ChatMessage(Protocol, operationRequest);

            if (!chatRequest.IsValid)
            {
                SendOperationResponse(chatRequest.GetResponse(ErrorCode.InvalidParametrs), sendParameters);
                return;
            }

            string message = chatRequest.Message;

            string[] param = message.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (param.Length == 2)
            {
                string targetName = param[0];
                message = param[1];

                if (ClientsPool.Instance.IsContain(targetName))
                {
                    var targetClient = ClientsPool.Instance.getByName(targetName);
                    if (targetClient == null)
                    {
                        return;
                    }

                    message = CharactedName + "[PM]: " + message;

                    // Создание event для отправителя и получателя
                    var personalEventData = new EventData((byte)EventCode.ChatMessage);
                    personalEventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, message } };
                    // Отправка eventa с заданными получателями
                    personalEventData.SendTo(new GameClient[] { this, targetClient }, sendParameters);
                    return;
                }
            }

            message = CharactedName + ": " + message;
            Chat.Instance.AddMessage(message);
            // Создание event для каждого client
            var eventData = new EventData((byte)EventCode.ChatMessage);
            eventData.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.ChatMessage, message } };
            //SendEvent(eventData, sendParameters);
            eventData.SendTo(ClientsPool.Instance.Clients, sendParameters);
        }*/
        #endregion
    }
}
