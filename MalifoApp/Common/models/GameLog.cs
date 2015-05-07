using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    public class GameLog
    {
        public IList<GameLogEvent> Events { get; private set; }

        public GameLog(IList<GameLogEvent> events)
        {
            if (events == null)
                Events = new List<GameLogEvent>();
            Events = events;
        }
    }

    public class GameLogEvent
    {
        public DateTime Timestamp { get; set; }
        public string Playername { get; set; }
        public string Text { get; set; }
    }
}
