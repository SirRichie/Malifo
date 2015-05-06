using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ServiceManager
    {
        private static readonly Lazy<ServiceManager> _lazy =
            new Lazy<ServiceManager>(() => new ServiceManager());

        public static ServiceManager Instance { get { return _lazy.Value; } }

        public UserService UserService { get; set; }

        private ServiceManager()
        {

        }
    }
}
