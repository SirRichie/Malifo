using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace MalifoApp
{
    /// <summary>
    /// singleton class to provide unified access to card resources such as images
    /// </summary>
    public class CardRegistry
    {
        private Dictionary<string, ImageSource> images;
        private Dictionary<string, string> texts;
        private Dictionary<string, string> shortTexts;

        private CardRegistry()
        {
            

            XmlDocument doc = new XmlDocument();
            doc.Load("Cards.xml");

            images = new Dictionary<string, ImageSource>(doc.DocumentElement.ChildNodes.Count);
            texts = new Dictionary<string, string>(doc.DocumentElement.ChildNodes.Count);
            shortTexts = new Dictionary<string, string>(doc.DocumentElement.ChildNodes.Count);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string key = node.Attributes["key"].InnerText;
                
                string path = node.Attributes["imagePath"].InnerText;
                //ImageSource tempSource = new BitmapImage(new Uri(path, UriKind.Relative));
                //path = Path.GetFullPath(path);
                Uri uri = new Uri(path, UriKind.Relative);
                images.Add(key, new BitmapImage(uri));

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

        public Dictionary<string, ImageSource> Images
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
    }
}
