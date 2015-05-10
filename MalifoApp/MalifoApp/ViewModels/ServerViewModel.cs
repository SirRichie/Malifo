using MalifoApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MalifoApp.ViewModels
{
    public class ServerViewModel
    {
        public bool Started { get; private set; }

        private ICommand startServerCommand;
        public ICommand ConnectCommand
        {
            get
            {
                if (startServerCommand == null)
                {
                    startServerCommand = new RelayCommand(p => ExecuteStartServerCommand(p));
                }
                return startServerCommand;
            }
        }

        private void ExecuteStartServerCommand(object p)
        {
            // todo: start server
        }
    }
}
