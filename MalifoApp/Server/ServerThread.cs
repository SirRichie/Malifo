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
		public bool Stop {get; set;}
        public bool Running { get; internal set; }		
		private TcpClient connection = null;
        private string clientHash = null;
		
		public ServerThread(TcpClient connection)
		{		
			this.connection = connection;
            Running = true;
            Stop = false;
			new Thread(new ThreadStart(Run)).Start();
		}		
		
		public void Run()
		{
          IFormatter formatter = new BinaryFormatter();

          while (!Stop)
          {
                if (!connection.Connected)
                {
                    Stop = true;
                }              
                try
                {
                    string messageHash = null;
                    Object obj = null;
                    try
                    {
                        obj = formatter.Deserialize(connection.GetStream());
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
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
                            messageHash = request.MessageHash;
                            var handler = HandlerFactory.Instance.GetHandlerForRequestType(request.GetType());
                            if (request is AsyncRequest)
                            {
                                handler.HandleRequest(request);
                                continue;
                            }
                            resp = handler.HandleRequest(request);
                            resp.ClientHash = clientHash;
                            resp.MessageHash = messageHash;
                            formatter.Serialize(connection.GetStream(), resp);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e is BusinessException)
                    {
                        formatter.Serialize(connection.GetStream(), e);
                    }
                    else
                    {
                        connection.Close();
                        Console.WriteLine("Error: " + e.Message);
                    }                     
                }                
            }
            Running = false;           
		}

        public void Dispose()
        {
            connection.Close();
        }

        protected virtual void Finalize()
        {
            Dispose();
        }
    }
}
