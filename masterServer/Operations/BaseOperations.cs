using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gameServer.Common;

namespace gameServer.Operations
{
    public class BaseOperations : Operation
    {
        public BaseOperations(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        { 
        }

        public virtual OperationResponse GetResponse(ErrorCode errorCode, string debugMessage = "")
        {
            var response = new OperationResponse(OperationRequest.OperationCode);
            response.ReturnCode = (short)errorCode;
            response.DebugMessage = debugMessage;
            return response;
        }
    }
}
