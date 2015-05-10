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
                return new GameLogViewModel(Model.GameLog);
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
                return Model.Players.Select(p => new PlayerViewModel(p)).ToList();
            }
            private set
            {
                Model.Players = value.Select(pvm => pvm.Model).ToList();
                OnPropertyChanged("Players");
            }
        }

        public GameStateViewModel(GameState model)
            : base(model)
        {
            
        }

        public void NewGameState(GameState newGameState)
        {
            Model = newGameState;

            Model.GameLog = newGameState.GameLog;
            Model.MainDeck = newGameState.MainDeck;
            Model.Players = newGameState.Players;

            OnPropertyChanged("GameLog");
            OnPropertyChanged("MainDeck");
            OnPropertyChanged("Players");
            OnPropertyChanged("Model");
        }
    }
}
