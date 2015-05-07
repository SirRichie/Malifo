using Common.models;
using MalifoApp.ViewModels;
using System;
using System.Collections.Generic;
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
        private DeckViewModel mainDeck;
        public DeckViewModel MainDeck
        {
            get
            {
                return mainDeck;
            }
            set
            {
                mainDeck = value;
            }
        }

        private GameLogViewModel gameLog;
        public GameLogViewModel GameLog
        {
            get
            {
                return gameLog;
            }
            set
            {
                gameLog = value;
            }
        }


        public MainWindow()
        {
            // register ourselves as the data context so we can use bindings
            this.DataContext = this;

            // initialize view models
            GameLog = new GameLogViewModel(new GameLog(new List<GameLogEvent>()));
            MainDeck = new DeckViewModel(new Deck(createTestDeck(54)), GameLog);
            


            InitializeComponent();

            LogViewer.SourceUpdated += LogViewer_SourceUpdated;
            LogViewer.MouseEnter += LogViewer_MouseEnter;
            LogViewer.DataContextChanged += LogViewer_DataContextChanged;
            LogViewer.RequestBringIntoView += LogViewer_RequestBringIntoView;
            LogViewer.TargetUpdated += LogViewer_TargetUpdated;
            LogViewer.TextInput += LogViewer_TextInput;

            LogViewer.Document.SourceUpdated += Document_SourceUpdated;
        }

        void Document_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("document source updated");
        }

        void LogViewer_TextInput(object sender, TextCompositionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("text input");
        }

        void LogViewer_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("target updated");
        }

        void LogViewer_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("request bring into view");
        }

        void LogViewer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("data context changed");
        }

        void LogViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("mouse entered");
        }

        void LogViewer_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SourceUpdated: " + e);
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
                Card card = new Card(i.ToString());
                card.ShortText = i.ToString();
                deck.Push(card);
            }

            return deck;
        }

        private void LogViewer_LayoutUpdated(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("layout updated");
            if (LogViewer.Document.Blocks.LastBlock != null)
                (LogViewer.Document.Blocks.LastBlock as Paragraph).Inlines.LastInline.BringIntoView();
        }

        private void LogViewer_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("source updated");
        }

        private void LogViewer_TextInput_1(object sender, TextCompositionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("text input");
        }
    }
}
