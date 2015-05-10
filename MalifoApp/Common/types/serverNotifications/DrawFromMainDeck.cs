using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.serverNotifications
{
    [Serializable]
    public class DrawFromMainDeck : ServerNotification
    {
        public int NumberOfCards { get; set; }
    }
}
