using gameServer.Common;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace gameServer.Operations
{
    class EnterInGameServer : BaseOperations
    {
        public EnterInGameServer(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.Id)]
        public int Id { get; set; }

        [DataMember(Code = (byte)ParameterCode.CharactedName)]
        public string CharactedName { get; set; }

        [DataMember(Code = (byte)ParameterCode.PlayerInfo)]
        public Info_ playerInfo { get; set; }

    }
}
