using MalifoApp.ViewModels;
using Microsoft.Win32;
using MvvmDialogs.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.Dialogs
{
    public class OpenFileDialogPresenter : IDialogBoxPresenter<OpenFileDialogViewModel>
    {
        public void Show(OpenFileDialogViewModel viewModel)
        {
            OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = viewModel.DefaultExt, InitialDirectory = viewModel.InitialDirectory };
            bool? result = dlg.ShowDialog();
            viewModel.Result = result.HasValue ? result.Value : false;
        }
    }
}
