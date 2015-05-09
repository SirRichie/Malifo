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

        public IList<CardViewModel> LastMainDraw
        {
            get
            {
                if (Model.LastMainDraw == null)
                    return null;
                return Model.LastMainDraw.Select(c => new CardViewModel(c)).ToList();
            }
            set
            {
                Model.LastMainDraw = value.Select(c => new Card(){Key = c.Model.Key}).ToList();
                OnPropertyChanged("LastMainDraw");
            }
        }

        public IList<CardViewModel> LastPersonalDraw
        {
            get
            {
                if (Model.LastPersonalDraw == null)
                    return null;
                return Model.LastPersonalDraw.Select(c => new CardViewModel(c)).ToList();
            }
            set
            {
                Model.LastPersonalDraw = value.Select(c => new Card() { Key = c.Model.Key }).ToList();
                OnPropertyChanged("LastPersonalDraw");
            }
        }
    }
}
