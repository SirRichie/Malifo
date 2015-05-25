using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    /// <summary>
    /// Represents a card in the game
    /// After careful consideration, we just need to store the key's name, which makes this class superfluous, but we keep it for future extensibility
    /// </summary>
    [Serializable]
    public class Card
    {
        /// <summary>
        /// the card's key, used to access all other, static resources like images and texts
        /// </summary>
        public string Key { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Card))
                return false;

            Card card = obj as Card;
            return Key.Equals(card.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
