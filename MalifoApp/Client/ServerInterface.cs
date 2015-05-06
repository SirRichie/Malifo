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
    public class ServerInterface
    {

        public delegate void NotificationEventHandler(object sender, NotificationEventArgs a);
        public event NotificationEventHandler RaiseNotivicationEvent;

        private TcpClient _client;       
        private bool stopNotificationListener;
        private string clientHash;

        public ServerInterface(TcpClient client)
        {
            stopNotificationListener = false;
            clientHash = null;
            _client = client;
            
            Thread th = new Thread(new ThreadStart(NotificationThread));

            th.Start();
        }

        private void NotificationThread()
        {
            NotificationEventHandler handler = RaiseNotivicationEvent;
            IFormatter formatter = new BinaryFormatter();
            if (!_client.Connected)
            {
                return;
            }
            while (!stopNotificationListener)
            {
                Object response = null;              
                lock (_client)
                {                  
                    response = formatter.Deserialize(_client.GetStream());
                }
                if (!(response is ITransferableObject))
                {
                    continue;
                }
                ITransferableObject tranferableObj = response as ITransferableObject;
                if (CheckClientHash(tranferableObj))
                {
                    continue;
                }
                if (handler == null)
                {
                    continue;
                }
                if (response is ServerNotification)
                {
                    ServerNotification notification = (ServerNotification)response;
                    handler(this, new NotificationEventArgs(notification));
                }
                else
                {
                    Console.WriteLine("NotificationThread: Wrong type {0}", response.GetType().ToString());
                }                                 
            }            
        }

        private bool CheckClientHash(ITransferableObject tranferableObj)
        {
            return !(clientHash != null && clientHash.Equals(tranferableObj.ClientHash));
        }

        public Response Execute(ITransferableObject request)
        {
            IFormatter formatter = new BinaryFormatter();
            if (request == null)
            {
                throw new ArgumentException(String.Format("Request couldn't be null"));
            }
            Object response = null;
            lock (_client)
            {
                formatter.Serialize(_client.GetStream(), request);
                response = formatter.Deserialize(_client.GetStream());               
            }
            HandleResponse(ref response);
            return (Response)response;
        }

        private void HandleResponse(ref Object response)
        {
            if (response is BusinessException)
            {
                throw (BusinessException)response;
            }
            if (response is Response)
            {
                SpecialResponseHandling(response as Response);
            }           
        }

        private void SpecialResponseHandling(Response response)
        {
            if (response is LoginResponse)
            {
                if (clientHash != null)
                {
                    throw new ServerInterfaceException("ClientHash already set. Something went wrong");
                }
                if (response.ClientHash == null)
                {
                    throw new ServerInterfaceException("Didn't recieve a ClientHash.");
                }
                clientHash = response.ClientHash;
            }
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
