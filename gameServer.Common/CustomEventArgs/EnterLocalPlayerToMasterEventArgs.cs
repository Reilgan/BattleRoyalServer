using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public class EnterLocalPlayerToMasterEventArgs : EventArgs
    {
        public StructPlayer LocalPlayer { get; private set; }

        public EnterLocalPlayerToMasterEventArgs(StructPlayer localPlayer) 
        {
            LocalPlayer = localPlayer;
        }
    }
}
