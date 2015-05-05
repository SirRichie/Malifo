using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return Model.Text;
            }
            set
            {
                Model.Text = value;
                OnPropertyChanged("Text");
            }
        }
    }
}
