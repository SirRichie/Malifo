using Common.types;
using Common.types.serverNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class NotificationEventArgs : EventArgs
    {
        private ITransferableObject _notification;
        public NotificationEventArgs(ITransferableObject notification)
        {
            _notification = notification;
        }

        public ITransferableObject Notification
        {
            get { return _notification; }
        }
    }
}
