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
    public class Deck
    {
        public Stack<Card> Cards { get; private set; }
        public Stack<Card> Discard { get; private set; }

        public Deck()
        {
            Cards = new Stack<Card>();
            Discard = new Stack<Card>();
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
                Discard.Push(card);
            }

            return result;
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
            Discard = new Stack<Card>();
        }
    }

    public class IllegalDrawAmountException : Exception
    {
        public IllegalDrawAmountException() : base() { }
        public IllegalDrawAmountException(string msg) : base(msg) { }
        public IllegalDrawAmountException(string msg, Exception e) : base(msg, e) { }
    }

    public class NotEnoughCardsLeftException : Exception
    {
        public NotEnoughCardsLeftException() : base() { }
        public NotEnoughCardsLeftException(string msg) : base(msg) { }
        public NotEnoughCardsLeftException(string msg, Exception e) : base(msg, e) { }
    }
}
