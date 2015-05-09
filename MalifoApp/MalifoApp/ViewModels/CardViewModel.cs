using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
                return CardRegistry.Instance.Images[Model.Key];
            }
        }

        public string ShortText
        {
            get
            {
                return CardRegistry.Instance.ShortTexts[Model.Key];
            }
        }
    }
}
