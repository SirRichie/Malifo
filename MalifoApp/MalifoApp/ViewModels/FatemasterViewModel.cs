using Common.models;
using MalifoApp.Commands;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MalifoApp.ViewModels
{
    public class FatemasterViewModel : ViewModel
    {
        private GameStateViewModel gameState;
        public GameStateViewModel GameState
        {
            get
            {
                return gameState;
            }
            set
            {
                gameState = value;
            }
        }

        private ICollection<IDialogViewModel> dialogs;
        private ConnectionViewModel connection;

        public FatemasterViewModel(GameStateViewModel gameState, ConnectionViewModel connection, ICollection<IDialogViewModel> dialogs)
        {
            GameState = gameState;
            this.connection = connection;
            this.dialogs = dialogs;
        }

        private ICommand openDialogCommand;
        public ICommand OpenDialogCommand
        {
            get
            {
                if (openDialogCommand == null)
                {
                    openDialogCommand = new RelayCommand(p => ExecuteOpenDialogCommand(p));
                }
                return openDialogCommand;
            }
        }

        private ICommand editPlayerDeckCommand;
        public ICommand EditPlayerDeckCommand
        {
            get
            {
                if (editPlayerDeckCommand == null)
                {
                    editPlayerDeckCommand = new RelayCommand(p => ExecuteEditPlayerDeckCommand(p));
                }
                return editPlayerDeckCommand;
            }
        }

        private ICommand shufflePlayerDeckCommand;
        public ICommand ShufflePlayerDeckCommand
        {
            get
            {
                if (shufflePlayerDeckCommand == null)
                {
                    shufflePlayerDeckCommand = new RelayCommand(p => ExecuteShufflePlayerDeckCommand(p));
                }
                return shufflePlayerDeckCommand;
            }
        }

        private void ExecuteShufflePlayerDeckCommand(object p)
        {
            string playername = p as string;
            connection.ShufflePlayerDeck(playername);
        }

        private void ExecuteEditPlayerDeckCommand(object p)
        {
            string playername = p as string;
            
            //System.Diagnostics.Debug.WriteLine("playername is " + playername + "; newplayername is " + newPlayername);

            //Players.Add(new PlayerViewModel(new Player() { Name = newPlayername }));

            DeckViewModel playerDeck = GameState.GetPlayer(playername).Deck;

            dialogs.Add(new EditDeckDialogViewModel(playerDeck)
            {
                OnOk = sender =>
                {
                    connection.PlayerDeckChange(playername, sender.PlayerDeck);
                }
            });
        }

        private void ExecuteOpenDialogCommand(object p)
        {
            
        }
    }
}
