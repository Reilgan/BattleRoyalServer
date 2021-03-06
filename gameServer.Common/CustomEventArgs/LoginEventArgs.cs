using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public class LoginEventArgs:EventArgs
    {
        public ErrorCode Error { get; private set; }
        public LoginEventArgs(ErrorCode error)
        {
            Error = error;
        }
    }
}
