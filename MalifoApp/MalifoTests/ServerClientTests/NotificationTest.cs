using System;

using Server.configuration;
using System.Net;
using Server;
using System.Threading;
using Client;
using Common.types.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MalifoTests.ServerClientTests
{
     [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            StartServer();
            ServerInterface client1 = StartClient();
            LoginClient(client1, "client1");
           
        }

        private void LoginClient(ServerInterface client1, string userName)
        {
            LoginRequest req = new LoginRequest() { UserName = userName };
        }

        private ServerInterface StartClient()
        {
            return ServerInterfaceFactory.GetServerInterface("localhost", 4711);
        }

        private void StartServer()
        {
            ServerConfiguration config = new ServerConfiguration()
            {
                LocalAddress = IPAddress.Parse("127.0.0.1"),
                Port = 4711,
                MaxPlayer = 9
            };

            MalifoServer server = new MalifoServer(config);

            Thread th = new Thread(new ThreadStart(server.runServer));
            th.Start();
        }
    }
}
