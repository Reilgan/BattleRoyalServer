using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public class MoveEventArgs: EventArgs
    {

        public string CharactedName { get; private set; }
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public float RotationZ { get; private set; }
        public float RotationW { get; private set; }

        public MoveEventArgs(Dictionary<byte, object> dict)
        {
            CharactedName = (string)dict[(byte)ParameterCode.CharactedName];
            PositionX = (float)dict[(byte)ParameterCode.positionX];
            PositionY = (float)dict[(byte)ParameterCode.positionY];
            PositionZ = (float)dict[(byte)ParameterCode.positionZ];
            PositionX = (float)dict[(byte)ParameterCode.positionX];
            RotationX = (float)dict[(byte)ParameterCode.rotationX];
            RotationY = (float)dict[(byte)ParameterCode.rotationY];
            RotationZ = (float)dict[(byte)ParameterCode.rotationZ];
            RotationW = (float)dict[(byte)ParameterCode.rotationW];
        }
    }
}
