﻿using Common.types.clientNotifications;
using Common.types.impl;
using MalifoApp.Commands;
using Server;
using Server.configuration;
using Server.Services;
using Server.userManagement;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MalifoApp.ViewModels
{
    public class ServerViewModel : ViewModel
    {
        
        private MalifoServer server;

        public int Port { get; set; }

        public bool Started { get; private set; }
        public bool Stopped
        {
            get
            {
                return !Started;
            }
        }

        private ICommand startServerCommand;
        public ICommand StartServerCommand
        {
            get
            {
                if (startServerCommand == null)
                {
                    startServerCommand = new RelayCommand(p => ExecuteStartServerCommand(p));
                }
                return startServerCommand;
            }
        }

        private ICommand stopServerCommand;
        public ICommand StopServerCommand
        {
            get
            {
                if (stopServerCommand == null)
                {
                    stopServerCommand = new RelayCommand(p => ExecuteStopServerCommand(p));
                }
                return stopServerCommand;
            }
        }

        private void ExecuteStopServerCommand(object p)
        {
            if (server != null)
            {
                server.StopServer();
                server.Dispose();
            }
        }

        private void ExecuteStartServerCommand(object p)
        {
            ServerConfiguration config = new ServerConfiguration() { LocalAddress = IPAddress.Any, Port = Port };
            server = new MalifoServer(config);

            ServiceManager.Instance.AddService(typeof(LoginRequest), new UserService(UserManager.Instance, ClientManager.Instance));
            GameEngineService gameEngine = new GameEngineService();
            ServiceManager.Instance.AddService(typeof(DrawFromMainDeck), gameEngine);
            ServiceManager.Instance.AddService(typeof(DrawFromPersonalDeck), gameEngine);
            ServiceManager.Instance.AddService(typeof(NewPlayer), gameEngine);
            ServiceManager.Instance.AddService(typeof(PlayerDeckChange), gameEngine);
            ServiceManager.Instance.AddService(typeof(ShufflePlayerDeck), gameEngine);
            ServiceManager.Instance.AddService(typeof(ShuffleMainDeck), gameEngine);
            ServiceManager.Instance.AddService(typeof(DiscardCard), gameEngine);
            ServiceManager.Instance.AddService(typeof(AcknowledgeMainDraw), gameEngine);


            server.StartServer();

            Started = true;
            OnPropertyChanged("Started");
            OnPropertyChanged("Stopped");
        }
    }
}
