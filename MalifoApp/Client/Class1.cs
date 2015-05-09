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
            serverInterface.RaiseNotivicationEvent += serverInterface_RaiseNotivicationEvent;
            LoginResponse res = (LoginResponse) serverInterface.Execute(new LoginRequest() { ClientHash = null, UserName = "Bert" });
            Console.WriteLine("Hash: "+res.ClientHash);
        }

        private static void serverInterface_RaiseNotivicationEvent(object sender, NotificationEventArgs a)
        {
            throw new NotImplementedException();
        }
    }
}
