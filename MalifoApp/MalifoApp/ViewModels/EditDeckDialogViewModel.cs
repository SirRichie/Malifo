using Common;
using MalifoApp.Commands;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MalifoApp.ViewModels
{
    public class EditDeckDialogViewModel : ViewModel, IUserDialogViewModel
    {
        public virtual bool IsModal { get { return true; } }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public virtual event EventHandler DialogClosing;

        private DeckViewModel deck;
        public DeckViewModel Deck { get { return deck; } }

        private DeckViewModel playerDeck;
        public DeckViewModel PlayerDeck { get { return playerDeck; } }

        public EditDeckDialogViewModel(DeckViewModel playerDeck)
        {
            this.deck = new DeckViewModel(CardRegistry.Instance.LoadDefaultDeck());
            this.playerDeck = (DeckViewModel) playerDeck.Clone();
        }

        private ICommand addToPlayerDeckCommand;
        public ICommand AddToPlayerDeckCommand
        {
            get
            {
                if (addToPlayerDeckCommand == null)
                {
                    addToPlayerDeckCommand = new RelayCommand(p => ExecuteAddToPlayerDeckCommand(p));
                }
                return addToPlayerDeckCommand;
            }
        }

        private ICommand removeFromPlayerDeckCommand;
        public ICommand RemoveFromPlayerDeckCommand
        {
            get
            {
                if (removeFromPlayerDeckCommand == null)
                {
                    removeFromPlayerDeckCommand = new RelayCommand(p => ExecuteRemoveFromPlayerDeckCommand(p));
                }
                return removeFromPlayerDeckCommand;
            }
        }

        private void ExecuteAddToPlayerDeckCommand(object p)
        {
            if (!(p is CardViewModel))
                return;
            CardViewModel card = p as CardViewModel;
            PlayerDeck.AddCard(card);
            OnPropertyChanged("PlayerDeck");
        }

        private void ExecuteRemoveFromPlayerDeckCommand(object p)
        {
            if (!(p is CardViewModel))
                return;
            CardViewModel card = p as CardViewModel;
            PlayerDeck.RemoveCard(card);
            OnPropertyChanged("PlayerDeck");
        }

        public ICommand OkCommand { get { return new RelayCommand(p => Ok(p)); } }
        protected virtual void Ok(object parameter)
        {
            if (this.OnOk != null)
                this.OnOk(this);
            Close();
        }

        public ICommand CancelCommand { get { return new RelayCommand(p => Cancel(p)); } }
        protected virtual void Cancel(object parameter)
        {
            if (this.OnCancel != null)
                this.OnCancel(this);
            Close();
        }

        public Action<EditDeckDialogViewModel> OnOk { get; set; }
        public Action<EditDeckDialogViewModel> OnCancel { get; set; }

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
    }
}
