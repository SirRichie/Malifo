using Common;
using Common.types;
using Common.types.exceptions;
using Newtonsoft.Json;
using Server.handler;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Server
{
	public class ServerThread
	{
	
		public bool stop = false;
		
		public bool running = false;
		
		private TcpClient connection = null;
		
		public ServerThread(TcpClient connection)
		{		
			this.connection = connection;
			new Thread(new ThreadStart(Run)).Start();
		}		
		
		public void Run()
		{
          IFormatter formatter = new BinaryFormatter();

            while (!stop) {
                Object obj = formatter.Deserialize(connection.GetStream());
                Response resp = null;
                Request request = null;
                if (obj is Request)
                {
                    request = (Request)obj;
                    var handler = HandlerFactory.Instance.GetHandlerForRequestType(request.GetType());
                    resp = handler.HandleRequest(request);
                    formatter.Serialize(connection.GetStream(), resp);
                }
            }
            running = false;
		}       
	}
}
