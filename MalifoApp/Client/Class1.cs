using Common.types;
using Common.types.impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Class1
    {
        public static void Main(String[] args)
        {
            ServerInterface serverInterface = ServerInterfaceFactory.GetServerInterface("localhost", 4711);
            LoginResponse res = (LoginResponse) serverInterface.Execute(new LoginRequest() { ClientHash = null, UserName = "Bert" });
           
        }
    }
}
