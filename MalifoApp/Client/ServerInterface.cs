using Common.types;
using Common.types.exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ServerInterface
    {
        TcpClient _client;
        private TcpClient client;        

        public ServerInterface(TcpClient client)
        {
            _client = client;
        }

        public Response Execute(Object request)
        {
            IFormatter formatter = new BinaryFormatter();
            if (request == null)
            {
                throw new ArgumentException(String.Format("Request couldn't be null"));
            }
          
            formatter.Serialize(_client.GetStream(), request);
            Object response = formatter.Deserialize(_client.GetStream());
            
            if (response is BusinessException)
            {
                throw (BusinessException)response;
            }
            return (Response)response;
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
