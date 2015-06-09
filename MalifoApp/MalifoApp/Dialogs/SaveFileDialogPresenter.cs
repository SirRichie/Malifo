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
    public class SaveFileDialogPresenter : IDialogBoxPresenter<SaveFileDialogViewModel>
    {
        public string FileExtension { get; set; }

        public void Show(SaveFileDialogViewModel viewModel)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = viewModel.FileName;
            dlg.Filter = viewModel.Filter;
            dlg.InitialDirectory = viewModel.InitialDirectory;
            dlg.RestoreDirectory = viewModel.RestoreDirectory;
            dlg.Title = viewModel.Title;
            dlg.ValidateNames = viewModel.ValidateNames;
            dlg.DefaultExt = viewModel.DefaultExtension;
            dlg.OverwritePrompt = viewModel.OverwritePrompt;

            var result = dlg.ShowDialog();
            viewModel.Result = (result != null) && result.Value;

            viewModel.FileName = dlg.FileName;
            viewModel.FileNames = dlg.FileNames;
            viewModel.Filter = dlg.Filter;
            viewModel.InitialDirectory = dlg.InitialDirectory;
            viewModel.RestoreDirectory = dlg.RestoreDirectory;
            viewModel.SafeFileName = dlg.SafeFileName;
            viewModel.SafeFileNames = dlg.SafeFileNames;
            viewModel.Title = dlg.Title;
            viewModel.ValidateNames = dlg.ValidateNames;
            viewModel.OverwritePrompt = dlg.OverwritePrompt;
        }
    }
}
