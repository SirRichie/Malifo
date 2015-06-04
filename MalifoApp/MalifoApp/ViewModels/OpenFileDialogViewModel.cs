using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.ViewModels
{
    public class OpenFileDialogViewModel : ViewModel, IDialogViewModel
    {
        public bool Result { get; set; }
        public string DefaultExt { get; set; }
        public string InitialDirectory { get; set; }
    }
}
