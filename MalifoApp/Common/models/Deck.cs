using Common.types.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.models
{
    /// <summary>
    /// Represents a card stack
    /// </summary>
    [Serializable]
    public class Deck : ICloneable
    {
        public Stack<Card> Cards { get; private set; }
        public Stack<Card> Discard { get; private set; }
        public IList<Card> Hand { get; private set; }

        public Deck()
        {
            Cards = new Stack<Card>();
            Discard = new Stack<Card>();
            Hand = new List<Card>(5);
        }

        public Deck(Stack<Card> cards)
            : this()
        {
            this.Cards = cards;
            shuffleCards();
        }

        /// <summary>
        /// draws from the card stack without putting the cards back
        /// drawn cards are put on the discard pile
        /// </summary>
        /// <param name="amount">the amount of cards to be drawn, must be between 1 and 4 inclusive</param>
        /// <returns>a list of cards that were drawn</returns>
        public IList<Card> Draw(int amount)
        {
            if (amount < 1 || amount > 4)
            {
                throw new IllegalDrawAmountException("amount must be between 1 and 4 (inclusive)");
            }
            if (amount > Cards.Count)
            {
                throw new NotEnoughCardsLeftException("The card stack does not have enough cards left");
            }

            IList<Card> result = new List<Card>();
            for (int i = 0; i < amount; i++)
            {
                Card card = Cards.Pop();
                result.Add(card);
                Hand.Add(card);
                //Discard.Push(card);
            }

            return result;
        }

        /// <summary>
        /// move a specific card from the hand to the discard pile
        /// </summary>
        /// <param name="card"></param>
        public void DiscardCardFromHand(Card card)
        {
            if (!Hand.Contains(card))
            {
                throw new CardNotInHandException("This card is not in the hand");
            }
            Hand.Remove(card);
            Discard.Push(card);
        }

        /// <summary>
        /// move the entire hand to the discard pile
        /// </summary>
        public void CardcardHand()
        {
            foreach (Card card in Hand)
            {
                Discard.Push(card);
            }
            Hand.Clear();
        }

        /// <summary>
        /// shuffles all cards of this deck, including those left on the stack and the discard pile
        /// </summary>
        public void ReShuffle()
        {
            shuffleCards();
        }

        /// <summary>
        /// shuffle list according to Fisher-Yates shuffle
        /// </summary>
        private void shuffleCards()
        {
            // first, put everything into a list
            List<Card> list = new List<Card>(Cards);
            list.AddRange(Discard);

            // now shuffle that list
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            // finally, recreate piles
            Cards = new Stack<Card>(list);
            Discard.Clear();
        }

        public object Clone()
        {
            return ObjectCloner.Clone(this);
        }
    }

    public class IllegalDrawAmountException : BusinessException
    {
        public IllegalDrawAmountException() : base() { }
        public IllegalDrawAmountException(string msg) : base(msg) { }
        public IllegalDrawAmountException(string msg, Exception e) : base(msg, e) { }
    }

    public class NotEnoughCardsLeftException : BusinessException
    {
        public NotEnoughCardsLeftException() : base() { }
        public NotEnoughCardsLeftException(string msg) : base(msg) { }
        public NotEnoughCardsLeftException(string msg, Exception e) : base(msg, e) { }
    }

    public class CardNotInHandException : BusinessException
    {
        public CardNotInHandException() : base() { }
        public CardNotInHandException(string msg) : base(msg) { }
        public CardNotInHandException(string msg, Exception e) : base(msg, e) { }
    }
}
