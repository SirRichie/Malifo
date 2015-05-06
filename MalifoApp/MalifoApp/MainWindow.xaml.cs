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

        public MainWindow()
        {
            // register ourselves as the data context so we can use bindings
            this.DataContext = this;

            // initialize view models
            MainDeck = new DeckViewModel(new Deck(createTestDeck(54)));

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
                Card card = new Card(i.ToString());
                deck.Push(card);
            }

            return deck;
        }
    }
}
