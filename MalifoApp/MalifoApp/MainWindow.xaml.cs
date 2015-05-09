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

        private DeckViewModel personalDeck;
        public DeckViewModel PersonalDeck
        {
            get
            {
                return personalDeck;
            }
            set
            {
                personalDeck = value;
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

        public IList<PlayerViewModel> Players { get; set; }
        


        public MainWindow()
        {
            // register ourselves as the data context so we can use bindings
            this.DataContext = this;

            // initialize view models
            GameLog = new GameLogViewModel(new GameLog(new List<GameLogEvent>()));
            MainDeck = new DeckViewModel(new Deck(createTestDeck(54)));
            MainDeck.CardsDrawnEvent += MainDeck_CardsDrawnEvent;
            PersonalDeck = new DeckViewModel(new Deck(createTestDeck(13)));
            PersonalDeck.CardsDrawnEvent += PersonalDeck_CardsDrawnEvent;
            Players = createTestPlayers();
            
            


            InitializeComponent();

        }

        void PersonalDeck_CardsDrawnEvent(IList<Card> cards)
        {
            String text = "zieht (personal) ";
            foreach (Card card in cards)
            {
                text += card.ShortText + ", ";
            }

            GameLog.Add(new GameLogEvent() { Text = text, Playername = "Player 1", Timestamp = DateTime.Now });

            Players[0].LastPersonalDraw = cards;
        }

        void MainDeck_CardsDrawnEvent(IList<Card> cards)
        {
            String text = "zieht ";
            foreach (Card card in cards)
            {
                text += card.ShortText + ", ";
            }

            GameLog.Add(new GameLogEvent() { Text = text, Playername = "Player 1", Timestamp = DateTime.Now });

            Players[0].LastMainDraw = cards;
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
            System.Diagnostics.Debug.WriteLine("layout updated");
            if (LogViewer.Document.Blocks.LastBlock != null)
                (LogViewer.Document.Blocks.LastBlock as Paragraph).Inlines.LastInline.BringIntoView();
        }
    }
}
