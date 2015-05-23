using Common.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    /// <summary>
    /// singleton class to provide unified access to card resources such as images
    /// </summary>
    public class CardRegistry
    {
        private Dictionary<string, string> images;
        private Dictionary<string, string> texts;
        private Dictionary<string, string> shortTexts;

        private CardRegistry()
        {
            

            XmlDocument doc = new XmlDocument();
            doc.Load("Cards.xml");

            images = new Dictionary<string, string>(doc.DocumentElement.ChildNodes.Count);
            texts = new Dictionary<string, string>(doc.DocumentElement.ChildNodes.Count);
            shortTexts = new Dictionary<string, string>(doc.DocumentElement.ChildNodes.Count);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string key = node.Attributes["key"].InnerText;
                images.Add(key, node.Attributes["imagePath"].InnerText);
                texts.Add(key, node.Attributes["text"].InnerText);
                shortTexts.Add(key, node.Attributes["shorttext"].InnerText);

            }
        }

        private static CardRegistry instance;
        public static CardRegistry Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CardRegistry();
                }
                return instance;
            }
        }

        public Dictionary<string, string> Images
        {
            get { return images; }
        }

        public Dictionary<string, string> Texts
        {
            get {return texts; }
        }

        public Dictionary<string, string> ShortTexts
        {
            get { return shortTexts; }
        }

        /// <summary>
        /// return the default deck, that is a deck with all cards where no cards have been drawn
        /// </summary>
        /// <returns></returns>
        public Deck LoadDefaultDeck()
        {
            int cardCount = CardRegistry.Instance.ShortTexts.Count;

            Stack<Card> deck = new Stack<Card>(cardCount);
            for (int i = 0; i < cardCount; i++)
            {
                Card card = new Card() { Key = CardRegistry.Instance.ShortTexts.Keys.ToList()[i] };
                deck.Push(card);
            }

            return new Deck(deck);
        }
    }
}
