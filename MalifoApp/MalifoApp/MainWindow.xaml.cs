using Common;
using Common.models;
using DataPersistor;
using MalifoApp.Commands;
using MalifoApp.ViewModels;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MalifoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Persistor<GameState> persitor;
        private GameState loadedGameState;

        /// <summary>
        /// MVVM compatible reference to dialogs
        /// </summary>
        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } }

        public ConnectionViewModel Connection { get; set; }

        public ServerViewModel Server { get; set; }

        public FatemasterViewModel Fatemaster { get; set; }

        /// <summary>
        /// Reference to the complete game state, this includes players and decks
        /// </summary>
        public GameStateViewModel GameState { get; set; }

       

        /// <summary>
        /// Keeps track of possible errors and displays them for some time
        /// </summary>
        public ErrorStateViewModel ErrorState
        {
            get
            {
                return ErrorStateViewModel.Instance;
            }
        }

        public MainWindow()
        {
            // register ourselves as the data context so we can use bindings
            this.DataContext = this;

            persitor = new Persistor<Common.models.GameState>();

            // initialize view models
            GameState = new GameStateViewModel(null);
            GameState.Players.Add(new PlayerViewModel(new Player() { Name = Properties.Settings.Default.username }));
            Connection = new ConnectionViewModel(GameState)
            {
                ServerPort = Properties.Settings.Default.serverPort,
                ServerAddress = Properties.Settings.Default.serverAddress,
                Username = Properties.Settings.Default.username
            };
            GameState.Connection = Connection;  // we need to have a reference to know if we are the gamemaster
            Server = new ServerViewModel(GameState) { Port = 35000 };
            Fatemaster = new FatemasterViewModel(GameState, Connection, Dialogs);

            InitializeComponent();

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
            var openFiledialog = new OpenFileDialogViewModel()
            {
                DefaultExtension = ".mres",
                InitialDirectory = "c:\\",
                Filter = "MalifoResources (*.mres) | *.mres"
            };
            _dialogs.Add(openFiledialog);
            if (openFiledialog.Result)
            {
                string fileName = openFiledialog.FileName;
                loadedGameState = persitor.LoadData(fileName);
                GameState.NewGameState(loadedGameState);
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
                var gameState = GameState.Model;
                if (gameState != null)
                {
                    persitor.Save(gameState, fileName);
                }
            }
        }    


        /// <summary>
        /// debug method to quickly create decks
        /// </summary>
        /// <param name="amount">number of cards in the deck</param>
        /// <returns></returns>
        private Stack<Card> createTestDeck(int amount)
        {
            Stack<Card> deck = new Stack<Card>(amount);
            for (int i = 0; i < amount; i++)
            {
                Card card = new Card() { Key = CardRegistry.Instance.ShortTexts.Keys.ToList()[i] };
                deck.Push(card);
            }

            return deck;
        }

        private List<PlayerViewModel> createTestPlayers()
        {
            List<PlayerViewModel> result = new List<PlayerViewModel>();

            Player p1 = new Player() { Name = "Player 1", Deck = new Deck(createTestDeck(13)) };
            Player p2 = new Player() { Name = "Player 2", Deck = new Deck(createTestDeck(13)) };
            Player p3 = new Player() { Name = "Player 3", Deck = new Deck(createTestDeck(13)) };
            Player p4 = new Player() { Name = "Player 4", Deck = new Deck(createTestDeck(13)) };

            result.Add(new PlayerViewModel(p1));
            result.Add(new PlayerViewModel(p2));
            result.Add(new PlayerViewModel(p3));
            result.Add(new PlayerViewModel(p4));

            return result;
        }

        private void LogViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if (LogViewer.Document.Blocks.LastBlock != null)
                (LogViewer.Document.Blocks.LastBlock as Paragraph).Inlines.LastInline.BringIntoView();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("closing");
            Server.StopServerCommand.Execute(null);
            Properties.Settings.Default.Save();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("closed");
        }

        private void ControlHandCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("control hand double click: " + sender);
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Dialogs.Add(new OpenFileDialogViewModel() { DefaultExt = ".txt", InitialDirectory = "c:\\" });
        }

    }
}
