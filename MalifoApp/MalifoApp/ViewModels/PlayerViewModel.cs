using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.ViewModels
{
    public class PlayerViewModel : ViewModel<Player>
    {
        public PlayerViewModel(Player model) : base(model)
        {

        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public DeckViewModel Deck
        {
            get
            {
                return new DeckViewModel(Model.Deck);
            }
            set
            {
                Model.Deck = value.Deck;
                OnPropertyChanged("Deck");
            }
        }

        public IList<Card> LastMainDraw
        {
            get
            {
                return Model.LastMainDraw;
            }
            set
            {
                Model.LastMainDraw = value;
                OnPropertyChanged("LastMainDraw");
            }
        }

        public IList<Card> LastPersonalDraw
        {
            get
            {
                return Model.LastPersonalDraw;
            }
            set
            {
                Model.LastPersonalDraw = value;
                OnPropertyChanged("LastPersonalDraw");
            }
        }
    }
}
