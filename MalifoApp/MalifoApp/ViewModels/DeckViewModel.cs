using Common;
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
    public delegate void CardsDrawn(IList<Card> cards);

    public class DeckViewModel : ViewModel, ICloneable
    {
        private Deck deck;

        public DeckViewModel(Deck deck)
        {
            this.deck = deck;
        }

        //public event CardsDrawn CardsDrawnEvent;
        //protected virtual void OnCardsDrawn(IList<Card> cards)
        //{
        //    if (CardsDrawnEvent != null)
        //    {
        //        CardsDrawnEvent(cards);
        //    }
        //}

        public Deck Deck
        {
            get
            {
                return deck;
            }
            set
            {
                deck = value;
            }
        }

        public IList<CardViewModel> AllCardsSorted
        {
            get
            {
                if (Deck == null) return new CardViewModel[0];
                List<Card> cards = Deck.Cards.ToList();
                cards.AddRange(Deck.Discard);
                cards.Sort(new CardComparer());
                return cards.Select(card => new CardViewModel(card)).ToList();
            }
        }

        public int CardsCount
        {
            get
            {
                if (deck == null) return 0;
                return deck.Cards.Count;
            }
        }

        public int DiscardCount
        {
            get
            {
                if (deck == null) return 0;
                return deck.Discard.Count;
            }
        }

        //private ICommand drawCommand;
        //public ICommand DrawCommand
        //{
        //    get
        //    {
        //        if (drawCommand == null)
        //        {
        //            drawCommand = new RelayCommand(p => ExecuteDrawCommand(p));
        //        }
        //        return drawCommand;
        //    }
        //}

        public void AddCard(CardViewModel card)
        {
            if (Deck.Cards.Select(c => c.Key).Contains(card.Model.Key))
            {
                // do not add twice
                return;
            }
            Deck.Cards.Push(card.Model);
            OnPropertyChanged("Deck");
            OnPropertyChanged("CardsSorted");
        }

        public void RemoveCard(CardViewModel card)
        {
            if (!Deck.Cards.Select(c => c.Key).Contains(card.Model.Key))
            {
                // silently ignore a case where we should remove a card that isn't even in the deck
                return;
            }

            // reshuffle to make sure we are not screwing up the whole deck
            Deck.ReShuffle();
            // take every element except for the one we want to remove
            IList<Card> reducedEnumeration = Deck.Cards.Where(c => !c.Key.Equals(card.Model.Key)).ToList();
            // clear the entire deck
            Deck.Cards.Clear();
            // and add everything again
            foreach (Card c in reducedEnumeration)
            {
                Deck.Cards.Push(c);
            }

            OnPropertyChanged("Deck");
            OnPropertyChanged("CardsSorted");
        }

        //private void ExecuteDrawCommand(object parameter)
        //{
        //    System.Diagnostics.Debug.WriteLine("parameter: {0}", parameter);
        //    int numberOfCards = Convert.ToInt32(parameter);
        //    IList<Card> result = deck.Draw(numberOfCards);
        //    OnCardsDrawn(result);

        //    OnPropertyChanged("CardsCount");
        //    OnPropertyChanged("DiscardCount");
        //}

        public object Clone()
        {
            return new DeckViewModel((Deck) deck.Clone());
        }
    }

    class CardComparer : IComparer<Card>
    {
        public int Compare(Card x, Card y)
        {
            string suitX = x.Key.Substring(x.Key.Length - 1);
            string suitY = y.Key.Substring(y.Key.Length - 1);

            string valueX = x.Key.Substring(0, x.Key.Length - 1);
            string valueY = y.Key.Substring(0, y.Key.Length - 1);

            // first, make the special treatment for jokers
            if (suitX.Equals("J"))
            {
                if (suitY.Equals("J"))
                    return valueX.CompareTo(valueY) * (-1); // red joker (R) is greater than black joker (B), hence * -1
                else 
                    return 1;   // jokers are always greater than other suits
            }

            // also check if y is a joker
            if (suitY.Equals("J"))
            {
                return -1;  // x is definitely smaller
            }

            if (suitX.Equals(suitY))
            {
                // within the same suit, only the number counts
                int intValueX = Convert.ToInt32(valueX);
                int intValueY = Convert.ToInt32(valueY);

                return intValueX.CompareTo(intValueY);
            }
            else
            {
                // suits are different so just use the suit for the decision
                return suitX.CompareTo(suitY);
            }
        }
    }

}
