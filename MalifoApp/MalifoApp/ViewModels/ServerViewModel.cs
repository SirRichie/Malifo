using Common.types.clientNotifications;
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
using System.Collections.ObjectModel;
using MvvmDialogs.ViewModels;
using DataPersistor;
using Common.models;


namespace MalifoApp.ViewModels
{
    public class ServerViewModel : ViewModel
    {

        private ObservableCollection<IDialogViewModel> _dialogs;        
        private MalifoServer server;
        private Persistor<GameState> persitor;


        public ServerViewModel(ObservableCollection<IDialogViewModel> dialogs)
        {
            persitor = new Persistor<GameState>();
            _dialogs = dialogs;
        }

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

        private ICommand openFileDialogCommand;
        public ICommand OpenFileDialogCommand
        {
            get
            {
                if (openFileDialogCommand == null)
                {
                    openFileDialogCommand = new RelayCommand(p => ExecuteOpenFileDialogCommand(p));
                }
                return openFileDialogCommand;
            }
        }

        private void ExecuteOpenFileDialogCommand(object p)
        {          
            var openFiledialog = new OpenFileDialogViewModel() {
                DefaultExtension = ".mres", 
                InitialDirectory = "c:\\",
                Filter = "MalifoResources (*.mres) | *.mres"
            };
            _dialogs.Add(openFiledialog);
            if (openFiledialog.Result)
            {
                string fileName = openFiledialog.FileName;
            }
        }

        private ICommand saveFileDialogCommand;
        public ICommand SaveFileDialogCommand
        {
            get
            {
                if (saveFileDialogCommand == null)
                {
                    saveFileDialogCommand = new RelayCommand(p => ExecuteSaveFileDialogCommand(p));
                }
                return saveFileDialogCommand;
            }
        }

        private void ExecuteSaveFileDialogCommand(object p)
        {
            var openFiledialog = new SaveFileDialogViewModel()
            {
                DefaultExtension = ".mres",
                InitialDirectory = "c:\\",
                Filter = "MalifoResources (*.mres) | *.mres",
                OverwritePrompt = true
            };
            _dialogs.Add(openFiledialog);
            if (openFiledialog.Result)
            {               
                string fileName = openFiledialog.FileName;
                var gameEngineService = ServiceManager.Instance.GetServiceByType(typeof(NewPlayer))as GameEngineService;
                var gameState = gameEngineService.GameState;
                if (gameState != null)
                {
                    persitor.Save(gameState, fileName);
                }
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
