using Client;
using Common.types;
using Common.types.impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.configuration;
using Server.Services;
using Server.userManagement;
using System.Net;
using System.Threading;

namespace MalifoTests.ServerClientTests
{
     [TestClass]
    public class NotificationTest
    {
         [TestInitialize]
         public void Setup()
         {
             ServiceManager.Instance.AddService(typeof(LoginRequest), new UserService(UserManager.Instance, ClientManager.Instance));
         }

        ITransferableObject notification;
        [TestMethod]
        public void TestMethod1()
        {
            using (MalifoServer server = StartServer())
            {                
                ServerInterface client1 = StartClient();
                LoginClient(client1, "client1");
                UserManager userManager = UserManager.Instance;
                Assert.AreEqual(1, userManager.UserList.Count);
                client1.Dispose();
                client1 = null;
                server.StopServer();
                server.Dispose();
            }
        }



        private void LoginClient(ServerInterface client1, string userName)
        {
            LoginRequest req = new LoginRequest() { UserName = userName };
            LoginResponse resp = client1.Execute(req) as LoginResponse;
            client1.RaiseNotivicationEvent += client_RaiseNotivicationEvent;
            Assert.IsNotNull(resp.ClientHash);
            Assert.AreEqual(resp.MessageHash, req.MessageHash);
        }

        void client_RaiseNotivicationEvent(object sender, NotificationEventArgs a)
        {
            Assert.IsNotNull(a);
        }

        private ServerInterface StartClient()
        {
            return ServerInterfaceFactory.GetServerInterface("localhost", 4711);
        }

        private MalifoServer StartServer()
        {
            ServerConfiguration config = new ServerConfiguration()
            {
                LocalAddress = IPAddress.Parse("127.0.0.1"),
                Port = 4711,
                MaxPlayer = 9
            };

            MalifoServer server = new MalifoServer(config);

            server.StartServer();
            return server;
        }
    }
}
