using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    public class Card
    {
        public string Text {get; set;}
        public string ShortText { get; set; }
        public byte[] Image { get; set; }

        public Card() { }

        public Card(string text)
        {
            this.Text = text;
        }
    }
}
