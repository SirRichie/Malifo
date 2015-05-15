using Client;
using Common.types;
using Common.types.clientNotifications;
using Common.types.impl;
using Common.types.serverNotifications;
using MalifoApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MalifoApp.ViewModels
{
    public class ConnectionViewModel : ViewModel
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string Username { get; set; }

        public string ClientHash { get; set; }

        private bool connected;
        public bool Connected
        {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
                OnPropertyChanged("Connected");
                OnPropertyChanged("NotConnected");
            }
        }

        public bool NotConnected
        {
            get
            {
                return !Connected;
            }
        }

        private ServerInterface server;

        private GameStateViewModel gameState;

        public ConnectionViewModel(GameStateViewModel gameState)
        {
            Connected = false;
            this.gameState = gameState;
        }

        private ICommand connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                if (connectCommand == null)
                {
                    connectCommand = new RelayCommand(p => ExecuteConnectCommand(p));
                }
                return connectCommand;
            }
        }

        private ICommand drawMainCommand;
        public ICommand DrawMainCommand
        {
            get
            {
                if (drawMainCommand == null)
                {
                    drawMainCommand = new RelayCommand(p => ExecuteDrawMainCommand(p), x => Connected);
                }
                return drawMainCommand;
            }
        }

        private ICommand drawPersonalCommand;
        public ICommand DrawPersonalCommand
        {
            get
            {
                if (drawPersonalCommand == null)
                {
                    drawPersonalCommand = new RelayCommand(p => ExecuteDrawPersonalCommand(p), x => Connected);
                }
                return drawPersonalCommand;
            }
        }

        private void ExecuteDrawPersonalCommand(object p)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            int numberOfCards = Convert.ToInt32(p);
            AsyncRequest notification = new DrawFromPersonalDeck() { NumberOfCards = numberOfCards, ClientHash = ClientHash };
            server.ExecuteAsync(notification);
        }

        private void ExecuteDrawMainCommand(object p)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            int numberOfCards = Convert.ToInt32(p);
            AsyncRequest notification = new DrawFromMainDeck() { NumberOfCards = numberOfCards, ClientHash = ClientHash };
            server.ExecuteAsync(notification);
        }

        private void ExecuteConnectCommand(object p)
        {
            server = ServerInterfaceFactory.GetServerInterface(ServerAddress, ServerPort);
            server.RaiseNotivicationEvent += server_RaiseNotivicationEvent;

            LoginResponse res = (LoginResponse)server.Execute(new LoginRequest() { ClientHash = null, UserName = Username });
            ClientHash = res.ClientHash;
            OnPropertyChanged("ClientHash");

            Connected = true;
        }

        void server_RaiseNotivicationEvent(object sender, NotificationEventArgs a)
        {
            ITransferableObject notification = a.Notification;

            System.Diagnostics.Debug.WriteLine("received notification: {0}", notification);

            if (notification is NewGameState)
            {
                NewGameState gameStateNotification = notification as NewGameState;
                gameState.NewGameState(gameStateNotification.NewState);
            }
        }

        //void PersonalDeck_CardsDrawnEvent(IList<Card> cards)
        //{
        //    String text = "zieht (personal) ";
        //    foreach (Card card in cards)
        //    {
        //        text += CardRegistry.Instance.ShortTexts[card.Key] + ", ";
        //    }

        //    debugGameState.GameLog.Events.Add(new GameLogEvent() { Text = text, Playername = "Player 1", Timestamp = DateTime.Now });
        //    debugGameState.Players[0].LastPersonalDraw = cards;

        //    GameState.NewGameState(debugGameState);
        //}

        //void MainDeck_CardsDrawnEvent(IList<Card> cards)
        //{
        //    String text = "zieht ";
        //    foreach (Card card in cards)
        //    {
        //        text += CardRegistry.Instance.ShortTexts[card.Key] + ", ";
        //    }

        //    debugGameState.GameLog.Events.Add(new GameLogEvent() { Text = text, Playername = "Player 1", Timestamp = DateTime.Now });
        //    GameState.NewGameState(debugGameState);

        //    //Players[0].LastMainDraw = cards.Select(c => new CardViewModel(c)).ToList();
        //}
    }
}
