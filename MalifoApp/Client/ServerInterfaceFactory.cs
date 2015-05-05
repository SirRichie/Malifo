using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ServerInterfaceFactory
    {
        public static ServerInterface GetServerInterface(string serverAddress, int port)
        {
            TcpClient client = new TcpClient(serverAddress, port);
            ServerInterface serverInterface = new ServerInterface(client);
            return serverInterface;
        }
    }
}
