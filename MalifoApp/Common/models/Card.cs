using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    [Serializable]
    public class Card
    {
        public string Text {get; set;}
        public string ShortText { get; set; }
        /// <summary>
        /// a reference string to a card's name, used like a primary key
        /// </summary>
        public string ImageReference { get; set; }

        public Card() { }

        public Card(string text)
        {
            this.Text = text;
        }
    }
}
