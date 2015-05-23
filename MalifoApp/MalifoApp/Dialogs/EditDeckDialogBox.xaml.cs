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
using System.Windows.Shapes;

namespace MalifoApp.Dialogs
{
    /// <summary>
    /// Interaction logic for MinimalDialogBox.xaml
    /// </summary>
    public partial class EditDeckDialogBox : Window
    {
        public EditDeckDialogBox()
        {
            InitializeComponent();
        }

        protected void HandleDoubleClickMainDeck(object sender, MouseButtonEventArgs e)
        {
            ((EditDeckDialogViewModel)DataContext).AddToPlayerDeckCommand.Execute(((ListViewItem)sender).Content as CardViewModel);
        }

        protected void HandleDoubleClickPlayerDeck(object sender, MouseButtonEventArgs e)
        {
            ((EditDeckDialogViewModel)DataContext).RemoveFromPlayerDeckCommand.Execute(((ListViewItem)sender).Content as CardViewModel);
        }
    }
}
