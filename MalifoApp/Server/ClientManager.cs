using Common.types.serverNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server
{     
    public class ClientManager
    {
        private static readonly Lazy<ClientManager> _lazy =
            new Lazy<ClientManager>(() => new ClientManager());

        public static ClientManager Instance { get { return _lazy.Value; } }

        private Dictionary<string, TcpClient> clientHashToTcpClient;

        private ClientManager()
        {
            clientHashToTcpClient = new Dictionary<string, TcpClient>();
        }

        public void AddClient(string clientHash, TcpClient client)
        {           
            clientHashToTcpClient[clientHash] = client;           
        }

        public TcpClient GetTcpClientByHash(string clientHash)
        {
            if (clientHashToTcpClient.ContainsKey(clientHash))
            {
                return clientHashToTcpClient[clientHash];
            }
            return null;
        }

        /// <summary>
        /// send a ServerNotification to all connected clients
        /// </summary>
        /// <param name="notification">the notification to send</param>
        public void BroadcastToAllClients(ServerNotification notification)
        {
            IFormatter formatter = new BinaryFormatter();
            string messageHash = Guid.NewGuid().ToString();

            foreach (KeyValuePair<string, TcpClient> entry in clientHashToTcpClient) {
                notification.ClientHash = entry.Key;
                formatter.Serialize(entry.Value.GetStream(), notification);
            }
        }
    }
}
