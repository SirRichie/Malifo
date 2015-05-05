using Server.configuration;
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
