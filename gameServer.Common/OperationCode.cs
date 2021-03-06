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
        SendChatMessage,
        GetRecentChatMessages,
        GetLocalPlayerTemplate,
        Move,
        GetPlayersTemplate
    }
}
