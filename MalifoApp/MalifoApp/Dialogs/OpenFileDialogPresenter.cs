﻿using MalifoApp.ViewModels;
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
        public string FileExtension { get; set; }

        public void Show(OpenFileDialogViewModel viewModel)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = viewModel.Multiselect;
            dlg.ReadOnlyChecked = viewModel.ReadOnlyChecked;
            dlg.ShowReadOnly = viewModel.ShowReadOnly;
            dlg.FileName = viewModel.FileName;
            dlg.Filter = viewModel.Filter;
            dlg.InitialDirectory = viewModel.InitialDirectory;
            dlg.RestoreDirectory = viewModel.RestoreDirectory;
            dlg.Title = viewModel.Title;
            dlg.ValidateNames = viewModel.ValidateNames;
            dlg.DefaultExt = viewModel.DefaultExtension;            

            var result = dlg.ShowDialog();
            viewModel.Result = (result != null) && result.Value;

            viewModel.Multiselect = dlg.Multiselect;
            viewModel.ReadOnlyChecked = dlg.ReadOnlyChecked;
            viewModel.ShowReadOnly = dlg.ShowReadOnly;
            viewModel.FileName = dlg.FileName;
            viewModel.FileNames = dlg.FileNames;
            viewModel.Filter = dlg.Filter;
            viewModel.InitialDirectory = dlg.InitialDirectory;
            viewModel.RestoreDirectory = dlg.RestoreDirectory;
            viewModel.SafeFileName = dlg.SafeFileName;
            viewModel.SafeFileNames = dlg.SafeFileNames;
            viewModel.Title = dlg.Title;
            viewModel.ValidateNames = dlg.ValidateNames;
        }
    }
}
