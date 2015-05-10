using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.models;
using System.Collections.Generic;
using MalifoApp;

namespace MalifoTests
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void TestNormalDraw()
        {
            // setup
            Deck testDeck = new Deck(createTestDeck(54));

            // exercise
            IList<Card> drawnCards = testDeck.Draw(1);

            // verify
            Assert.AreEqual(1, drawnCards.Count);
            Assert.AreEqual(1, testDeck.Discard.Count);
            Assert.AreEqual(53, testDeck.Cards.Count);
        }

        [TestMethod]
        public void TestMaxDraw()
        {
            // setup
            Deck testDeck = new Deck(createTestDeck(54));

            // exercise
            IList<Card> drawnCards = testDeck.Draw(4);

            // verify
            Assert.AreEqual(4, drawnCards.Count);
            Assert.AreEqual(4, testDeck.Discard.Count);
            Assert.AreEqual(50, testDeck.Cards.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(IllegalDrawAmountException))]
        public void TestIllegalAmount()
        {
            // setup
            Deck testDeck = new Deck(createTestDeck(10));

            // exercise
            testDeck.Draw(5);

            // verify implicit
        }

        [TestMethod]
        [ExpectedException(typeof(NotEnoughCardsLeftException))]
        public void TestNotEnoughCards()
        {
            // setup
            Deck testDeck = new Deck(createTestDeck(2));

            // exercise
            testDeck.Draw(4);

            // verify implicit
        }

        /// <summary>
        /// Creates a deck with dummy data for the cards
        /// </summary>
        /// <param name="amount">number of the cards in the deck</param>
        /// <returns>the new deck</returns>
        private Stack<Card> createTestDeck(int amount)
        {
            Stack<Card> deck = new Stack<Card>(amount);

            IEnumerator<string> keyEnum = CardRegistry.Instance.ShortTexts.Keys.GetEnumerator();
            keyEnum.Reset();

            for (int i = 0; i < amount; i++)
            {
                keyEnum.MoveNext();
                Card card = new Card() { Key = keyEnum.Current };
                deck.Push(card);
            }

            return deck;
        }
    }
}
