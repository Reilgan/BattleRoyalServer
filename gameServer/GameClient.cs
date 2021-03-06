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

        public string CharactedName { get; private set; }

        public GameClient(InitRequest initRequest) : base(initRequest)
        {
            log.Debug("Player connection ip: " + initRequest.RemoteIP);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            ClientsPool.Instance.RemoveClient(this);
            log.Debug("Disconnected: " + CharactedName);
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
                case (byte)OperationCode.SendChatMessage:
                    {
                        SendChatMessageHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.GetRecentChatMessages:
                    {
                        GetRecentChatMessageHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.GetLocalPlayerTemplate:
                    {
                        GetLocalPlayerTemplateHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.Move:
                    {
                        MoveHandler(operationRequest, sendParameters);
                        break;
                    }
                case (byte)OperationCode.GetPlayersTemplate:
                    {
                        GetPlayersTemplateHandler(operationRequest, sendParameters);
                        break;
                    }
                default:
                    log.Debug("Unknown OperationRequest recv: " + operationRequest.OperationCode);
                    break;
            }
        }

        #region handler for requests
        private void GetPlayersTemplateHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            List<GameClient> clientsForEven = new List<GameClient>();
            clientsForEven.AddRange(ClientsPool.Instance.Clients);
            clientsForEven.Remove(this);
            foreach (GameClient client in clientsForEven)
            {
                OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
                Dictionary<byte, object> template = new Dictionary<byte, object>();
                template.Add((byte)ParameterCode.CharactedName, CharactedName);
                template.Add((byte)ParameterCode.positionX, null);
                template.Add((byte)ParameterCode.positionY, null);
                template.Add((byte)ParameterCode.positionZ, null);
                template.Add((byte)ParameterCode.rotationX, null);
                template.Add((byte)ParameterCode.rotationY, null);
                template.Add((byte)ParameterCode.rotationZ, null);
                template.Add((byte)ParameterCode.rotationW, null);
                resp.Parameters = template;
                SendOperationResponse(resp, sendParameters);
            }
        }
        private void MoveHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var moveRequest = new Move(Protocol, operationRequest);
            var eventDataMove = new EventData((byte)EventCode.Move);
            eventDataMove.Parameters = moveRequest.GetParametersForEvent();
            List<GameClient> clientsForEven = new List<GameClient>();
            clientsForEven.AddRange(ClientsPool.Instance.Clients);
            clientsForEven.Remove(this);
            eventDataMove.SendTo(clientsForEven, sendParameters);
        }
        private void GetLocalPlayerTemplateHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var eventDataPlayerTemp = new EventData((byte)EventCode.PlayerTemplate);
            Dictionary<byte, object> template = new Dictionary<byte, object>();
            template.Add((byte)ParameterCode.CharactedName, CharactedName);
            template.Add((byte)ParameterCode.positionX, null);
            template.Add((byte)ParameterCode.positionY, null);
            template.Add((byte)ParameterCode.positionZ, null);
            template.Add((byte)ParameterCode.rotationX, null);
            template.Add((byte)ParameterCode.rotationY, null);
            template.Add((byte)ParameterCode.rotationZ, null);
            template.Add((byte)ParameterCode.rotationW, null);
            eventDataPlayerTemp.Parameters = template;
            eventDataPlayerTemp.SendTo(ClientsPool.Instance.Clients, sendParameters);
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
        private void LoginHandler(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var loginRequest = new Login(Protocol, operationRequest);

            if (!loginRequest.IsValid)
            {
                SendOperationResponse(loginRequest.GetResponse(ErrorCode.InvalidParametrs), sendParameters);
                return;
            }
            CharactedName = loginRequest.CharactedName;
            if (ClientsPool.Instance.IsContain(CharactedName))
            {
                SendOperationResponse(loginRequest.GetResponse(ErrorCode.NameIsExist), sendParameters);
                return;
            }

            ClientsPool.Instance.AddClient(this);
            OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
            resp.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.CharactedName, CharactedName } };
            SendOperationResponse(resp, sendParameters);
            log.Info("user with name:" + CharactedName);
        }
        private void SendChatMessageHandler(OperationRequest operationRequest, SendParameters sendParameters)
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
        }
        #endregion
    }
}

//TODO Добавить определение шаблона локального игрока
//Dictionary<string, object> playerTemplate = new Dictionary<string, object>();
//playerTemplate.Add(loginRequest.CharactedName, null);
//resp.Parameters = new Dictionary<byte, object>() { { (byte)ParameterCode.Player, playerTemplate } };
