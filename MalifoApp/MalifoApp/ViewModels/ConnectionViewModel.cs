﻿using Client;
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
        private string serverAddress;
        public string ServerAddress
        {
            get
            {
                return serverAddress;
            }
            set
            {
                serverAddress = value;
                Properties.Settings.Default.serverAddress = serverAddress;
            }
        }

        private int serverPort;
        public int ServerPort
        {
            get
            {
                return serverPort;
            }
            set
            {
                serverPort = value;
                Properties.Settings.Default.serverPort = serverPort;
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                Properties.Settings.Default.username = username;
            }
        }

        /// <summary>
        /// indicates if we are trying to connect as the fatemaster
        /// </summary>
        private bool asFatemaster;
        public bool AsFatemaster
        {
            get
            {
                return asFatemaster;
            }
            set
            {
                asFatemaster = value;
                Properties.Settings.Default.asFatemaster = asFatemaster;
            }
        }

        /// <summary>
        /// indicates if the server confirmed that we are the fatemaster
        /// </summary>
        public bool IsFatemaster { get; set; }

        public string ClientHash { get; set; }

        private ErrorStateViewModel ErrorState { get { return ErrorStateViewModel.Instance; } }

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

        private ICommand discardCommand;
        public ICommand DiscardCommand
        {
            get
            {
                if (discardCommand == null)
                {
                    discardCommand = new RelayCommand(p => ExecuteDiscardCommand(p));
                }
                return discardCommand;
            }
        }

        private ICommand acknowledgeMainDrawCommand;
        public ICommand AcknowledgeMainDrawCommand
        {
            get
            {
                if (acknowledgeMainDrawCommand == null)
                {
                    acknowledgeMainDrawCommand = new RelayCommand(p => ExecuteAcknowledgeMainDrawCommand(p));
                }
                return acknowledgeMainDrawCommand;
            }
        }

        public void PlayerDeckChange(string playername, DeckViewModel playerDeck)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            AsyncRequest notification = new PlayerDeckChange() { PlayerName = playername, PlayerDeck = playerDeck.Deck, ClientHash = ClientHash };
            server.ExecuteAsync(notification);
        }

        public void ShufflePlayerDeck(string playername)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            AsyncRequest notification = new ShufflePlayerDeck() { PlayerName = playername, ClientHash = ClientHash };
            server.ExecuteAsync(notification);
        }

        public void ShuffleMainDeck()
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            AsyncRequest notification = new ShuffleMainDeck() { ClientHash = ClientHash };
            server.ExecuteAsync(notification);
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

        private void ExecuteDiscardCommand(object p)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            if (!(p is CardViewModel))
                throw new InvalidOperationException("Can only work with CardViewModels");
            AsyncRequest notification = new DiscardCard() { Card = ((CardViewModel)p).Model, ClientHash = ClientHash };
            server.ExecuteAsync(notification);
        }

        private void ExecuteAcknowledgeMainDrawCommand(object p)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            AsyncRequest notification = new AcknowledgeMainDraw() { ClientHash = ClientHash };
            server.ExecuteAsync(notification);
        }

        private void ExecuteConnectCommand(object p)
        {
            server = ServerInterfaceFactory.GetServerInterface(ServerAddress, ServerPort);
            server.RaiseNotificationEvent += server_RaiseNotificationEvent;
            server.RaiseExceptionEvent += server_RaiseExceptionEvent;

            LoginResponse res = (LoginResponse)server.Execute(new LoginRequest() { ClientHash = null, UserName = Username, AsFatemaster = AsFatemaster });
            ClientHash = res.ClientHash;
            OnPropertyChanged("ClientHash");

            Connected = true;
        }

        void server_RaiseExceptionEvent(object sender, NotificationEventArgs a)
        {
            if (a.Notification is Exception)
            {
                Exception e = a.Notification as Exception;
                ErrorState.Message = e.Message;
                ErrorState.Visible = true;
            }

        }

        void server_RaiseNotificationEvent(object sender, NotificationEventArgs a)
        {
            ITransferableObject notification = a.Notification;

            System.Diagnostics.Debug.WriteLine("received notification: {0}", notification);

            if (notification is NewGameState)
            {
                NewGameState gameStateNotification = notification as NewGameState;
                gameState.NewGameState(gameStateNotification.NewState);
            }
        }

    }
}
