using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalifoApp.ViewModels
{
    public class ErrorStateViewModel : ViewModel
    {
        private static readonly Lazy<ErrorStateViewModel> _lazy =
            new Lazy<ErrorStateViewModel>(() => new ErrorStateViewModel() { Message = "", Visible = false});

        public static ErrorStateViewModel Instance { get { return _lazy.Value; } }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                Visible = true;
                OnPropertyChanged("Message");
            }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (value)
                {
                    // set a timer to automatically hide this message after a few seconds
                    hide();
                }
                visible = value;
                OnPropertyChanged("Visible");
            }
        }

        private async void hide()
        {
            await Task.Delay(5000);
            Visible = false;
        }
    }
}
