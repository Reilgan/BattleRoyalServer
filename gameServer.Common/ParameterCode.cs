using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public enum ParameterCode : byte
    {
        Id,
        CharactedName,
        ChatMessage,
        positionX,
        positionY,
        positionZ,
        rotationX,
        rotationY,
        rotationZ,
        rotationW,
        PlayerInfo
    }
}
