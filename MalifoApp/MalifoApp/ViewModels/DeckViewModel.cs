﻿using Common.models;
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

    public class DeckViewModel : ViewModel
    {
        private Deck deck;

        public DeckViewModel(Deck deck)
        {
            this.deck = deck;
        }

        public event CardsDrawn CardsDrawnEvent;
        protected virtual void OnCardsDrawn(IList<Card> cards)
        {
            if (CardsDrawnEvent != null)
            {
                CardsDrawnEvent(cards);
            }
        }

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
            IList<Card> result = deck.Draw(numberOfCards);
            OnCardsDrawn(result);

            OnPropertyChanged("CardsCount");
            OnPropertyChanged("DiscardCount");
        }
    }
}
