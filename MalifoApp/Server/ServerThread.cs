using Common;
using Common.types;
using Common.types.exceptions;
using Common.types.impl;
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
	public class ServerThread : IDisposable
	{	
		public bool stop = false;		
		public bool running = false;		
		private TcpClient connection = null;
        private string clientHash = null;
		
		public ServerThread(TcpClient connection)
		{		
			this.connection = connection;
			new Thread(new ThreadStart(Run)).Start();
		}		
		
		public void Run()
		{
          IFormatter formatter = new BinaryFormatter();

            while (!stop) {
                lock (connection)
                {
                    try
                    {
                        Object obj = formatter.Deserialize(connection.GetStream());
                        Response resp = null;
                        Request request = null;
                        if (obj is ITransferableObject)
                        {
                            if (clientHash == null)
                            {
                                clientHash = Guid.NewGuid().ToString();
                                ((ITransferableObject)obj).ClientHash = clientHash;
                                ClientManager.Instance.AddClient(clientHash, connection);
                            }
                        }

                        if (obj is Request)
                        {
                            if (clientHash.Equals(((ITransferableObject)obj).ClientHash))
                            {
                                request = (Request)obj;
                                var handler = HandlerFactory.Instance.GetHandlerForRequestType(request.GetType());
                                resp = handler.HandleRequest(request);
                                resp.ClientHash = clientHash;
                                formatter.Serialize(connection.GetStream(), resp);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                        Console.WriteLine("Error: "+e.Message);
                    }
                }
            }
            running = false;
		}

        public void Dispose()
        {
            connection.Close();
        }
    }
}
