using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    /// <summary>
    /// This class represents the game state at any time.
    /// It includes references to all other relevant models which are part of the game state
    /// </summary>
    [Serializable]
    public class GameState
    {
        public GameLog GameLog { get; set; }
        public IList<Player> Players { get; set; }
        public Deck MainDeck { get; set; }

        // that is all we need
    }
}
