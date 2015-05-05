using Common.types;
using Common.types.exceptions;
using Newtonsoft.Json;
using Server.handler;
using System;
using System.Net.Sockets;
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
            String jsonString = ReadStream(connection.GetStream());
            if(String.IsNullOrEmpty(jsonString)){

            }
            else{

                Request req = null;
                try
                {
                    req = JsonConvert.DeserializeObject<Request>(jsonString);
                }
                catch (Exception e)
                {
                    BusinessException exception = new BusinessException("Coldn't deserialize Object", e);
                    WriteStream(connection.GetStream(), JsonConvert.SerializeObject(exception));
                }
                Response resp = null;
                try
                {
                    var handler = HandlerFactory.Instance.GetHandlerForRequestType(req.GetType());
                    resp = handler.HandleRequest(req);
                    string jsonStringResponse = JsonConvert.SerializeObject(resp);
                    WriteStream(connection.GetStream(), jsonStringResponse);
                }
                catch (BusinessException e)
                {
                    WriteStream(connection.GetStream(), JsonConvert.SerializeObject(e));
                }
            }
		}

        private string ReadStream(NetworkStream stream)
        {
            if (!stream.CanRead)
            {
                 Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
            }
            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;              
            do
            {
                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
            }
            while (stream.DataAvailable);
            return myCompleteMessage.ToString();
        }

        private void WriteStream(NetworkStream stream, string jsonStringRepresentation)
        {
            if (!stream.CanWrite)
            {
                Console.WriteLine("Sorry.  Cant write");
            }

            byte[] myWriteBuffer = Encoding.ASCII.GetBytes(jsonStringRepresentation);
            stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);            
        }
	}
}
