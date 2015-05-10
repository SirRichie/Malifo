using Common.types.exceptions;
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

        private Dictionary<Type, IService> _messageTypeToService;

        private ServiceManager()
        {
            _messageTypeToService = new Dictionary<Type, IService>();
        }

        public void AddService(Type type, IService service){
            if(_messageTypeToService.ContainsKey(type)){
                throw new BusinessException(String.Format("Service for Type {0} is already defined: {1}", type.ToString(), _messageTypeToService[type].GetType().ToString()));
            }
            _messageTypeToService[type] = service;
        }

        public IService GetServiceByType(Type type)
        {
            if (!_messageTypeToService.ContainsKey(type))
            {
                throw new BusinessException(String.Format("Service for Type {0} is not defined", type.ToString()));
            }
            return _messageTypeToService[type];
        }
    }
}
