using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace voice2text
{
    public class Msc
    {
        private const string ParamForLogin = "appid=5964bde1";

        public void Login()
        {
            int result = MSCDll.MSPLogin(null, null, ParamForLogin);

            if (result == 0)
            {
                Trace.TraceInformation("Login succeeded.");
            }
            else
            {
                throw new Exception("Failed to login. Error code is: " + result);
            }
        }

    }
}
