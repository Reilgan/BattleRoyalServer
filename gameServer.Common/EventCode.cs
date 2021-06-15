using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public enum EventCode : byte
    {
        ChatMessage,
        PlayerTemplate,
        Move,
        RoomReady,
        PlayerExitFromGameServer,
    }
}
