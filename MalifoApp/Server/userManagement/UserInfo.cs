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

namespace Server.userManagement
{
	public class UserInfo
	{		
		public string SessionHash{ get; set; }
        public string UserName { get; set; }		
		
		public UserInfo()
		{
            
		}
	}
}
