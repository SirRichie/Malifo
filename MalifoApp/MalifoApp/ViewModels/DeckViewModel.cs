using Common.models;
using MalifoApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MalifoApp.ViewModels
{
    public class DeckViewModel : ViewModel
    {
        private Deck deck;
        private GameLogViewModel gameLog;

        public DeckViewModel(Deck deck, GameLogViewModel gameLog)
        {
            this.deck = deck;
            this.gameLog = gameLog;
        }

        public int CardsCount
        {
            get
            {
                return deck.Cards.Count;
            }
        }

        public int DiscardCount
        {
            get
            {
                return deck.Discard.Count;
            }
        }

        private ICommand drawCommand;
        public ICommand DrawCommand
        {
            get
            {
                if (drawCommand == null)
                {
                    drawCommand = new RelayCommand(p => ExecuteDrawCommand(p));
                }
                return drawCommand;
            }
        }

        private void ExecuteDrawCommand(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("parameter: {0}", parameter);
            int numberOfCards = Convert.ToInt32(parameter);
            // TODO return the result somehow
            IList<Card> result = deck.Draw(numberOfCards);

            String text = "zieht ";
            foreach (Card card in result)
            {
                text += card.ShortText + ", ";
            }

            gameLog.Add(new GameLogEvent() { Text = text, Playername = "Tobias", Timestamp = DateTime.Now });

            OnPropertyChanged("CardsCount");
            OnPropertyChanged("DiscardCount");
        }
    }
}
