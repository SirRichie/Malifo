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

        public ConnectionViewModel Connection { get; set; }

        /// <summary>
        /// Reference to the complete game state, this includes players and decks
        /// </summary>
        public GameStateViewModel GameState { get; set; }

        public MainWindow()
        {
            // register ourselves as the data context so we can use bindings
            this.DataContext = this;

            // initialize view models
            GameState = new GameStateViewModel(null);
            Connection = new ConnectionViewModel(GameState);

            InitializeComponent();

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
    }
}
