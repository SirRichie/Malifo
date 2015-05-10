using Client;
using Common.types;
using Common.types.clientNotifications;
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
    public class ConnectionViewModel
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string Username { get; set; }
        public bool Connected { get; set; }

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
                if (connectCommand == null) {
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
                    drawMainCommand = new RelayCommand(p => ExecuteDrawMainCommand(p));
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
                    drawPersonalCommand = new RelayCommand(p => ExecuteDrawPersonalCommand(p));
                }
                return drawPersonalCommand;
            }
        }

        private void ExecuteDrawPersonalCommand(object p)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            if (p is int)
            {
                AsyncRequest notification = new DrawFromPersonalDeck() { NumberOfCards = (int)p };
                server.ExecuteAsync(notification);
            }
        }

        private void ExecuteDrawMainCommand(object p)
        {
            if (!Connected)
                throw new InvalidOperationException("Must be connected to execute this command");
            if (p is int)
            {
                AsyncRequest notification = new DrawFromMainDeck() { NumberOfCards = (int)p };
                server.ExecuteAsync(notification);
            }
        }

        private void ExecuteConnectCommand(object p)
        {
            server = ServerInterfaceFactory.GetServerInterface(ServerAddress, ServerPort);
            server.RaiseNotivicationEvent += server_RaiseNotivicationEvent;
            Connected = true;
        }

        void server_RaiseNotivicationEvent(object sender, NotificationEventArgs a)
        {
            ITransferableObject notification = a.Notification;

            System.Diagnostics.Debug.WriteLine("received notification: {0}", notification);

            if (notification is NewGameState)
            {

            }
        }
    }
}
