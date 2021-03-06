﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public class PlayerTemlateEventArgs : EventArgs
    {
        //public Dictionary<string, object> PlayerTemplate { get; set; }
        public int Id { get; private set; }
        public string CharactedName { get; private set; }

        public Dictionary<byte, object> PlayerInfo = new Dictionary<byte, object>();
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public float RotationZ { get; private set; }
        public float RotationW { get; private set; }

        public PlayerTemlateEventArgs(Dictionary<byte, object> dict)
        {
            Id = (int)dict[(byte)ParameterCode.Id];
            CharactedName = (string)dict[(byte)ParameterCode.CharactedName];
            PlayerInfo = (Dictionary<byte, object>)dict[(byte)ParameterCode.PlayerInfo];
            PositionX = (float)dict[(byte)ParameterCode.positionX];
            PositionY = (float)dict[(byte)ParameterCode.positionY];
            PositionZ = (float)dict[(byte)ParameterCode.positionZ];
            RotationX = (float)dict[(byte)ParameterCode.rotationX];
            RotationY = (float)dict[(byte)ParameterCode.rotationY];
            RotationZ = (float)dict[(byte)ParameterCode.rotationZ];
            RotationW = (float)dict[(byte)ParameterCode.rotationW];
        }
    }
}
