using Common.types.serverNotifications;
/*
 * Created by SharpDevelop.
 * User: Maschiene
 * Date: 05/05/2015
 * Time: 12:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server.userManagement
{
	public class UserInfo
	{
        public string UserName { get; set; }
		public string SessionHash{ get; set; }       
      
		public UserInfo()
		{
            
		}

        public void SendNotification(ServerNotification notivication , TcpClient client)
        {
            IFormatter formatter = new BinaryFormatter();
            if (CheckClientIsConnected(client))
            {               
                formatter.Serialize(client.GetStream(), notivication);
            }
        }

        private bool CheckClientIsConnected(TcpClient client)
        {
            return client != null && client.Connected;
        }
	}
}
