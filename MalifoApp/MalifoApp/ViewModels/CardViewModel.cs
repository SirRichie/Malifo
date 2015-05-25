using Common;
using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MalifoApp.ViewModels
{
    /// <summary>
    /// A ViewModel for a Card
    /// </summary>
    public class CardViewModel : ViewModel<Card>
    {
        public CardViewModel(Card model) : base(model) { }

        public string Text
        {
            get
            {
                return CardRegistry.Instance.Texts[Model.Key];
            }
        }

        public ImageSource Image
        {
            get
            {
                String imagePath = CardRegistry.Instance.Images[Model.Key];
                //ImageSource tempSource = new BitmapImage(new Uri(path, UriKind.Relative));
                //path = Path.GetFullPath(path);
                Uri uri = new Uri(imagePath, UriKind.Relative);
                return new BitmapImage(uri);
            }
        }

        public string ShortText
        {
            get
            {
                return CardRegistry.Instance.ShortTexts[Model.Key];
            }
        }

        public string Key
        {
            get
            {
                return Model.Key;
            }
        }
    }
}
