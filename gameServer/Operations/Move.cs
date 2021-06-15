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
    class Move:BaseOperations
    {
        Dictionary<byte, object> parameters;
        public Move(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.Id)]
        public int Id { get; set; }

        [DataMember(Code = (byte)ParameterCode.positionX)]
        public float PositionX { get; set; }

        [DataMember(Code = (byte)ParameterCode.positionY)]
        public float PositionY { get; set; }

        [DataMember(Code = (byte)ParameterCode.positionZ)]
        public float PositionZ { get; set; }

        [DataMember(Code = (byte)ParameterCode.rotationX)]
        public float RotationX { get; set; }

        [DataMember(Code = (byte)ParameterCode.rotationY)]
        public float RotationY { get; set; }

        [DataMember(Code = (byte)ParameterCode.rotationZ)]
        public float RotationZ { get; set; }

        [DataMember(Code = (byte)ParameterCode.rotationW)]
        public float RotationW { get; set; }

        public Dictionary<byte, object> GetParametersForEvent()
        {
            parameters = new Dictionary<byte, object>();
            parameters.Add((byte)ParameterCode.Id, Id);
            parameters.Add((byte)ParameterCode.positionX, PositionX);
            parameters.Add((byte)ParameterCode.positionY, PositionY);
            parameters.Add((byte)ParameterCode.positionZ, PositionZ);
            parameters.Add((byte)ParameterCode.rotationX, RotationX);
            parameters.Add((byte)ParameterCode.rotationY, RotationY);
            parameters.Add((byte)ParameterCode.rotationZ, RotationZ);
            parameters.Add((byte)ParameterCode.rotationW, RotationW);
            return parameters;
        }

    }
}
