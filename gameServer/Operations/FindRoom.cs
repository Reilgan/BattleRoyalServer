using gameServer.Common;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Operations
{
    public class FindRoom : BaseOperations
    {
        public FindRoom(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.Id)]
        public int IdPlayer { get; set; }

        [DataMember(Code = (byte)ParameterCode.RoomType)]
        public byte RoomType { get; set; }

    }
}
