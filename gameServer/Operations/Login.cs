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
    public class Login : BaseOperations
    {
        public Login(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.CharactedName)]
        public string CharactedName { get; set; }
    }
}
