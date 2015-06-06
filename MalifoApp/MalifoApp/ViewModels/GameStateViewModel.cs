using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.ViewModels
{
    public class GameStateViewModel : ViewModel<GameState>
    {
        public GameLogViewModel GameLog
        {
            get
            {
                if (Model == null) return new GameLogViewModel(null, false);
                return new GameLogViewModel(Model.GameLog, ConnectedAsFatemaster());
            }
            private set
            {
                Model.GameLog = value.Model;
                OnPropertyChanged("GameLog");
            }
        }

        public DeckViewModel MainDeck
        {
            get
            {
                if (Model == null) return new DeckViewModel(null);
                return new DeckViewModel(Model.MainDeck);
            }
            private set
            {
                Model.MainDeck = value.Deck;
                OnPropertyChanged("MainDeck");
            }
        }

        public IList<PlayerViewModel> Players
        {
            get
            {
                if (Model == null) return new List<PlayerViewModel>();
                return Model.Players.Values.Select(p => new PlayerViewModel(p)).ToList();
            }
            //private set
            //{
            //    Model.Players = value.Select(pvm => pvm.Model).ToList();
            //    OnPropertyChanged("Players");
            //}
        }

        public string LastDrawPlayer
        {
            get
            {
                if (Model == null) return "";
                return Model.LastDrawPlayer;
            }
            set
            {
                Model.LastDrawPlayer = value;
                OnPropertyChanged("LastDrawPlayer");
            }
        }

        public ConnectionViewModel Connection { private get; set; }

        public GameStateViewModel(GameState model)
            : base(model)
        {

        }

        private bool ConnectedAsFatemaster()
        {
            if (Connection == null) return false;
            return !Model.Players.ContainsKey(Connection.Username);
        }

        public PlayerViewModel GetPlayer(string playername)
        {
            return new PlayerViewModel(Model.Players[playername]);
        }

        public void NewGameState(GameState newGameState)
        {
            Model = newGameState;

            Model.GameLog = newGameState.GameLog;
            Model.MainDeck = newGameState.MainDeck;
            Model.Players = newGameState.Players;
            Model.LastDrawPlayer = newGameState.LastDrawPlayer;

            OnPropertyChanged("GameLog");
            OnPropertyChanged("MainDeck");
            OnPropertyChanged("Players");
            OnPropertyChanged("LastDrawPlayer");
            OnPropertyChanged("Model");
        }
    }
}
