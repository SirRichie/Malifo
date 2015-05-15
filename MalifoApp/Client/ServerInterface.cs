using Common.types;
using Common.types.exceptions;
using Common.types.impl;
using Common.types.serverNotifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class ServerInterface: IDisposable
    {
        private TcpClient _client;        
        private string _clientHash;
        private ServerMessageQueue _messageQueue;
        private Response _response;
        private bool _responseAvailable;

        public delegate void NotificationEventHandler(object sender, NotificationEventArgs a);
        public event NotificationEventHandler RaiseNotivicationEvent;

        private NotificationEventHandler handler; 
        public ServerInterface(TcpClient client)
        {         
            _clientHash = null;
            _client = client;
            _response = null;
            _responseAvailable = false;
            _messageQueue = new ServerMessageQueue(_client); 
            _messageQueue.RaiseResponseEvent += _messageQueue_RaiseResponseEvent;
            _messageQueue.RaiseNotivicationEvent += _messageQueue_RaiseNotivicationEvent;
            _messageQueue.StartMessageQueue();
        }

        private void _messageQueue_RaiseNotivicationEvent(object sender, NotificationEventArgs a)
        {
            if (handler != null)
            {
                handler(this, a);
            }
            if (RaiseNotivicationEvent != null)
            {
                RaiseNotivicationEvent(sender, a);
            }
        }

        private void _messageQueue_RaiseResponseEvent(object sender, NotificationEventArgs a)
        {
            _response = a.Notification as Response;
            _responseAvailable = true;
        }        

        private bool CheckClientHash(ITransferableObject tranferableObj)
        {
            return !(_clientHash != null && _clientHash.Equals(tranferableObj.ClientHash));
        }

        public Response Execute(Request request)
        {
            IFormatter formatter = new BinaryFormatter();
          
            if (request == null)
            {
                throw new ArgumentException(String.Format("Request couldn't be null"));
            }
            string messageHash = Guid.NewGuid().ToString();
            (request).MessageHash = messageHash;
  
            formatter.Serialize(_client.GetStream(), request);

            _messageQueue.WaitForResponse((request).MessageHash);
            while (!_responseAvailable) {
                Thread.Sleep(50);
            }
            _responseAvailable = false;
            HandleResponse(ref _response);
            return (Response)_response;
        }

        public void ExecuteAsync(AsyncRequest request)
        {
            IFormatter formatter = new BinaryFormatter();

            if (request == null)
            {
                throw new ArgumentException(String.Format("Request couldn't be null"));
            }
            string messageHash = Guid.NewGuid().ToString();
            request.MessageHash = messageHash;
           
            formatter.Serialize(_client.GetStream(), request);
        }

        private void HandleResponse(ref Response response)
        {           
            if (response is Response)
            {
                SpecialResponseHandling(response as Response);
            }           
        }

        private void SpecialResponseHandling(Response response)
        {
            if (response is LoginResponse)
            {
                if (_clientHash != null)
                {
                    throw new ServerInterfaceException("ClientHash already set. Something went wrong");
                }
                if (response.ClientHash == null)
                {
                    throw new ServerInterfaceException("Didn't recieve a ClientHash.");
                }
                _clientHash = response.ClientHash;
            }
        }

        public void Dispose()
        {
            _messageQueue.RaiseResponseEvent -= _messageQueue_RaiseResponseEvent;
            _messageQueue.RaiseNotivicationEvent -= _messageQueue_RaiseNotivicationEvent;
            _messageQueue.StopMessageQueue();
            _messageQueue.Dispose();
        }

        protected virtual void Finalize()
        {
            Dispose();
        }

        
    }

    public class ServerInterfaceException : Exception
    {
        public ServerInterfaceException(string msg)
            : base(msg)
        {

        }

        public ServerInterfaceException(string msg, Exception e)
            :base(msg, e)
        {

        }
    }
}
