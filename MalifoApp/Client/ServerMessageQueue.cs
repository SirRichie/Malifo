using Common.types;
using Common.types.exceptions;
using Common.types.serverNotifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class ServerMessageQueue
    {
        public delegate void NotificationEventHandler(object sender, NotificationEventArgs a);
        public event NotificationEventHandler RaiseNotivicationEvent;

        public delegate void ResponseEventHandler(object sender, NotificationEventArgs a);
        public event ResponseEventHandler RaiseResponseEvent;

        private bool _stopMessageQueue;
        private bool _waitForResponse;
        private long _timeout;       
        private string _messageHash;
        private TcpClient _tcpClient;
        private Stopwatch _timer;

        private ConcurrentQueue<ITransferableObject> _incommingMessages;

        private ResponseEventHandler _responsehandler;
        private NotificationEventHandler _notificationhandler;

        public ServerMessageQueue(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _timeout = 4000;                       
            _timer = new Stopwatch();
        }

        public void WaitForResponse(string messageHash)
        {
            _messageHash = messageHash;
            _waitForResponse = true;
            _timer.Start();            
        }

        public void StopMessageQueue()
        {
            _stopMessageQueue = true;
        }

        public void StartMessageQueue()
        {
            _timer.Reset();
            _messageHash = null;
            _stopMessageQueue = false;
            _waitForResponse = false;
            _incommingMessages = new ConcurrentQueue<ITransferableObject>();
            Thread enqueueThread = new Thread(new ThreadStart(RunEnqueue));
            enqueueThread.Start();
            Thread dequeueThread = new Thread(new ThreadStart(RunDequeue));
            dequeueThread.Start();
        }

        public void RunEnqueue()
        {          
            IFormatter formatter = new BinaryFormatter();            
            while (!_stopMessageQueue)
            {
                _responsehandler = RaiseResponseEvent;
                _notificationhandler = RaiseNotivicationEvent;
                Thread.Sleep(200);
                Object response = null;
                //lock (_tcpClient)
                //{
                    response = formatter.Deserialize(_tcpClient.GetStream());
                //}
                if (!(response is ITransferableObject))
                {
                    continue;
                }
                if (!handlerSubcribed())
                {
                    continue;
                }
                 ITransferableObject tranferableObj = response as ITransferableObject;              
                _incommingMessages.Enqueue(tranferableObj);            
            }        
        }

        private bool handlerSubcribed()
        {
            return _notificationhandler != null && _responsehandler != null;
        }

        public void RunDequeue()
        {                  
            IFormatter formatter = new BinaryFormatter();
            Queue<ITransferableObject> notificationQueue = new Queue<ITransferableObject>();
            while (!_stopMessageQueue)
            {
                _responsehandler = RaiseResponseEvent;
                _notificationhandler = RaiseNotivicationEvent;
                Thread.Sleep(200);  
                if (!handlerSubcribed())
                {
                    continue;
                }

                ITransferableObject tmpObj = null;
                bool couldPeek = _incommingMessages.TryDequeue(out tmpObj);           
                if (!couldPeek)
                {
                    continue;
                }
                if (tmpObj is BusinessException)
                {
                    throw tmpObj as BusinessException;
                }
                else if (_waitForResponse)
                {
                    if (tmpObj is Response)
                    {
                        handleResponse(_responsehandler, tmpObj);
                    }
                    else
                    {
                        notificationQueue.Enqueue(tmpObj);
                    }
                }
                else
                {
                    notificationQueue.Enqueue(tmpObj);
                    //handleServerNotification(_notificationhandler, tmpObj);
                }
                while (notificationQueue.Count > 0)
                {
                    handleServerNotification(_notificationhandler, notificationQueue.Dequeue());
                }
            }
        }

        private void handleServerNotification(NotificationEventHandler handler, ITransferableObject tmpObj)
        {
            if (!(tmpObj is ServerNotification))
            {
                return;
            }
            if (handler == null)
            {
                return;
            }           
            handler(this, new NotificationEventArgs(tmpObj));
        }

        private void handleResponse(ResponseEventHandler handler, ITransferableObject tmpObj)
        {                   
            //if (_timer.ElapsedMilliseconds > _timeout)
            //{
            //    throw new MessageQueueException("Timeout for Response is reached");
            //}
            if (!(tmpObj is Response))
            {
                return;
            }
            if (handler == null)
            {
                return;
            }
            if (_messageHash == null || !(tmpObj as Response).MessageHash.Equals(_messageHash))
            {
                throw new MessageQueueException("ResponseHash does not match the RequestHash");
            }
            _waitForResponse = false;
            _messageHash = null;
            handler(this, new NotificationEventArgs(tmpObj));
        }
    }

    public class MessageQueueException : Exception
    {
        public MessageQueueException(string msg)
            : base(msg)
        {

        }

        public MessageQueueException(string msg, Exception e)
            : base(msg, e)
        {

        }
    }
}
