﻿using Common;
using Common.models;
using Common.types.exceptions;
using Common.types.serverNotifications;
using Server.userManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class GameEngineService : IService
    {

        private GameState gameState;

        public GameState GameState
        {
            get { return gameState; }
        }

        public GameEngineService()
        {
            gameState = CreateEmptyGameState();
        }

        public GameEngineService(GameState gameState)
        {
            this.gameState = gameState;
        }

        private GameState CreateEmptyGameState()
        {
            return new GameState()
            {
                GameLog = new GameLog(new List<GameLogEvent>()),
                MainDeck = CardRegistry.Instance.LoadDefaultDeck(),
                Players = new Dictionary<string, Player>()
            };
        }

        public void PlayerConnected(string name, bool asFatemaster)
        {
            if (asFatemaster)
            {
                // do not add the fatemaster to the list of players... yet
                broadcastNewState();    // broadcast state nonetheless so fatemaster gets a list of players
                return;
            }

            if (gameState.Players.ContainsKey(name))
            {
                // we already know the player, so don't do anything except broadcast so the connecting player's client has the current state
                // the player is probably in the list because their data was loaded on startup
                broadcastNewState();
                return;
            }
            // add the player with an empty deck, decks are edited separately
            gameState.Players.Add(name, new Player() { Name = name, Deck = new Deck() });

            // update game state
            string text = "connected";
            gameState.GameLog.Events.Add(new GameLogEvent() { Playername = name, Text = text, Timestamp = DateTime.Now, IsSensitive = false });

            broadcastNewState();
        }

        public void DrawFromMainDeck(int amount, UserInfo user)
        {
            // draw the cards
            IList<Card> drawnCards = gameState.MainDeck.Draw(amount);

            // update game state
            gameState.Players[user.UserName].LastMainDraw = drawnCards;
            gameState.LastDrawPlayer = user.UserName;

            // update game log
            UpdateGameLogWithDrawnCards(drawnCards, user.UserName, false);

            // broadcast
            broadcastNewState();
        }

        public void DrawFromPersonalDeck(int amount, UserInfo user)
        {
            // draw the cards
            Player player = gameState.Players[user.UserName];
            IList<Card> drawnCards = player.Deck.Draw(amount);

            // update player state
            player.LastPersonalDraw = drawnCards;

            // update game log
            UpdateGameLogWithDrawnCards(drawnCards, user.UserName, false);

            // broadcast
            broadcastNewState();
        }

        public void PlayerDeckChange(string playername, Deck playerDeck, UserInfo requester)
        {
            // update deck
            EnsureFatemasterStatusOrThrowException(requester);
            playerDeck.ReShuffle();
            gameState.Players[playername].Deck = playerDeck;

            // update game log
            string text = "changes deck of " + playername;
            gameState.GameLog.Events.Add(new GameLogEvent() { Playername = "Fatemaster", Text = text, Timestamp = DateTime.Now, IsSensitive = false });

            // broadcast
            broadcastNewState();
        }

        public void ShufflePlayerDeck(string playername, UserInfo requester)
        {
            // shuffle deck
            EnsureFatemasterStatusOrThrowException(requester);
            gameState.Players[playername].Deck.ReShuffle();
            
            // update game log
            string text = "shuffles the deck of " + playername;
            gameState.GameLog.Events.Add(new GameLogEvent() { Playername = "Fatemaster", Text = text, Timestamp = DateTime.Now, IsSensitive = false });

            // broadcast
            broadcastNewState();
        }

        public void ShuffleMainDeck(UserInfo requester)
        {
            // shuffle deck
            EnsureFatemasterStatusOrThrowException(requester);
            gameState.MainDeck.ReShuffle();

            // update game log
            string text = "shuffles the main deck";
            gameState.GameLog.Events.Add(new GameLogEvent() { Playername = "Fatemaster", Text = text, Timestamp = DateTime.Now, IsSensitive = false });

            // broadcast
            broadcastNewState();
        }

        public void DiscardCard(Card card, UserInfo user)
        {
            // update deck
            gameState.Players[user.UserName].Deck.DiscardCardFromHand(card);

            // update game log
            string text = "discards " + CardRegistry.Instance.ShortTexts[card.Key];
            gameState.GameLog.Events.Add(new GameLogEvent() { Playername = user.UserName, Text = text, Timestamp = DateTime.Now, IsSensitive = true });

            // broadcast
            broadcastNewState();
        }

        public void AcknowledgeMainDraw(UserInfo user)
        {
            // update deck
            gameState.MainDeck.DiscardHand();

            // broadcast
            broadcastNewState();
        }

        private void UpdateGameLogWithDrawnCards(IList<Card> drawnCards, string playername, bool personalDraw)
        {
            string deckText = personalDraw ? "twist deck" : "fate deck";
            string text = "draws (" + deckText + ") ";
            foreach (Card card in drawnCards)
            {
                text += CardRegistry.Instance.ShortTexts[card.Key] + ", ";
            }
            text = text.Substring(0, text.Length - 2);
            gameState.GameLog.Events.Add(new GameLogEvent() { Text = text, Playername = playername, Timestamp = DateTime.Now, IsSensitive = true });
        }

        private void EnsureFatemasterStatusOrThrowException(UserInfo requester)
        {
#if DEBUG
            return;
#else
            if (!(requester.IsFatemaster))
            {
                throw new BusinessException("This operation can only be done by the fatemaster");
            }
#endif
        }

        private void broadcastNewState()
        {
            ClientManager.Instance.BroadcastToAllClients(new NewGameState() { NewState = gameState });
        }


    }
}
