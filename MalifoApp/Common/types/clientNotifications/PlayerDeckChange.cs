using Common.models;
using Common.types.serverNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.clientNotifications
{
    [Serializable]
    public class PlayerDeckChange : AsyncRequest
    {
        public string PlayerName { get; set; }
        public Deck PlayerDeck { get; set; }
    }
}
