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
        private ServerNotification _notification;
        public NotificationEventArgs(ServerNotification notification)
        {
            _notification = notification;
        }

        public ServerNotification Notification
        {
            get { return _notification; }
        }
    }
}
