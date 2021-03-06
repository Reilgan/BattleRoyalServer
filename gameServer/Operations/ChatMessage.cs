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
    class ChatMessage:BaseOperations
    {
        public ChatMessage(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        { 
        }

        [DataMember(Code = (byte)ParameterCode.ChatMessage)]
        public string Message { get; set; }

    }
}
