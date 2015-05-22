using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.ViewModels
{
    public class EditDeckDialogViewModel : IUserDialogViewModel
    {
        public virtual bool IsModal { get { return true; } }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public virtual event EventHandler DialogClosing;

        private DeckViewModel deck;
        public DeckViewModel Deck;

        public EditDeckDialogViewModel(DeckViewModel deck)
        {
            this.deck = deck;
        }

        public Action<EditDeckDialogViewModel> OnOk { get; set; }
        public Action<EditDeckDialogViewModel> OnCancel { get; set; }
    }
}
