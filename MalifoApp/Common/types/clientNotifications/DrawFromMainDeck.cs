using Common.types.serverNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.clientNotifications
{
    [Serializable]
    public class DrawFromMainDeck : AsyncRequest
    {
        public int NumberOfCards { get; set; }
    }
}
