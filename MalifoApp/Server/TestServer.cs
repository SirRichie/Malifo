using Common.types.impl;
using Server.configuration;
using Server.handler;
using Server.handler.impl;
using Server.Services;
using Server.userManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class TestServer
    {
        public static void Main(String[] args)
        {
            //HandlerFactory factory = HandlerFactory.Instance;
            //factory.RegisterHandler(new LoginHandler());
            ServerConfiguration config = new ServerConfiguration()
            {
                LocalAddress = IPAddress.Parse("127.0.0.1"),
                Port = 4711,
                MaxPlayer = 9
            };
            ServiceManager.Instance.AddService(typeof(LoginRequest), new UserService(UserManager.Instance, ClientManager.Instance));
            MalifoServer server = new MalifoServer(config);
            server.StartServer();
           
        }
    }
}
