﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.models;
using System.Collections.Generic;

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

        private Stack<Card> createTestDeck(int amount)
        {
            Stack<Card> deck = new Stack<Card>(amount);
            for (int i = 0; i < amount; i++)
            {
                Card card = new Card(i.ToString());
                deck.Push(card);
            }

            return deck;
        }
    }
}
