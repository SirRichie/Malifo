using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    public class Player
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }
        public IList<Card> LastMainDraw { get; set; }
        public IList<Card> LastPersonalDraw { get; set; }
    }
}
