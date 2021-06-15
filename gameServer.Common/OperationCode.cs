using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public enum OperationCode : byte
    {
        Login,
        EnterInGameServer,
        ExitFromGameServer,
        SendChatMessage,
        GetRecentChatMessages,
        Move,
        GetPlayersInRoom,
        FindRoom,
        GetGameServerIP,
    }
}
