using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.ViewModels
{
    public class SaveFileDialogViewModel : ViewModel, IDialogViewModel
    {              
        public string FileName { get; set; }
        public string[] FileNames { get; set; }
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }
        public bool RestoreDirectory { get; set; }
        public string SafeFileName { get; set; }
        public string[] SafeFileNames { get; set; }
        public string Title { get; set; }
        public bool ValidateNames { get; set; }
        public bool Result { get; set; }
        public string DefaultExtension { get; set; }
        public bool OverwritePrompt { get; set; }
    }
}
