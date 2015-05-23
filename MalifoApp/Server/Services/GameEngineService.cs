using Common;
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

        public GameEngineService()
        {
            // TODO later, we want to load the model
            gameState = CreateEmptyGameState();
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
                return;
            }

            if (gameState.Players.ContainsKey(name))
            {
                // we already know the player, so don't do anything
                // the player is probably in the list because their data was loaded on startup
                return;
            }
            // add the player with an empty deck, decks are edited separately
            gameState.Players.Add(name, new Player() { Name = name, Deck = new Deck() });
            broadcastNewState();
        }

        public void DrawFromMainDeck(int amount, UserInfo user)
        {
            // draw the cards
            IList<Card> drawnCards = gameState.MainDeck.Draw(amount);

            // TODO add to game log

            // update player state
            // gameState.Players.Where(p => p.Name.Equals(user.UserName)).First().LastMainDraw = drawnCards;
            gameState.Players[user.UserName].LastMainDraw = drawnCards;

            broadcastNewState();

            return;
        }

        public void DrawFromPersonalDeck(int amount, UserInfo user)
        {
            // draw the cards
            Player player = gameState.Players[user.UserName];
            IList<Card> drawnCards = player.Deck.Draw(amount);

            // update player state
            // gameState.Players.Where(p => p.Name.Equals(user.UserName)).First().LastMainDraw = drawnCards;
            player.LastPersonalDraw = drawnCards;

            broadcastNewState();

            return;
        }

        public void PlayerDeckChange(string playername, Deck playerDeck, UserInfo requester)
        {
            EnsureFatemasterStatusOrThrowException(requester);
            playerDeck.ReShuffle();
            gameState.Players[playername].Deck = playerDeck;
            broadcastNewState();
        }

        public void ShufflePlayerDeck(string playername, UserInfo requester)
        {
            EnsureFatemasterStatusOrThrowException(requester);
            gameState.Players[playername].Deck.ReShuffle();
            broadcastNewState();
        }

        public void ShuffleMainDeck(UserInfo requester)
        {
            EnsureFatemasterStatusOrThrowException(requester);
            gameState.MainDeck.ReShuffle();
            broadcastNewState();
        }

        private void EnsureFatemasterStatusOrThrowException(UserInfo requester)
        {
            if (!(requester.IsFatemaster))
            {
                throw new BusinessException("This operation can only be done by the fatemaster");
            }
        }

        private void broadcastNewState()
        {
            ClientManager.Instance.BroadcastToAllClients(new NewGameState() { NewState = gameState });
        }


    }
}
