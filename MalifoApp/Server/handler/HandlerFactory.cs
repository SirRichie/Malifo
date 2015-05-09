using Common.types.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Server.handler
{
    public class HandlerFactory
    {
        private static readonly Lazy<HandlerFactory> _lazy =
            new Lazy<HandlerFactory>(() => new HandlerFactory());

        public static HandlerFactory Instance { get { return _lazy.Value; } }

        private IList<IHandler> handlerList;
        private HandlerFactory()
		{
            handlerList = new List<IHandler>();
            GetHanlderFromAssebly();
		}

        public void RegisterHandler(IHandler handler) 
        {
            if (RequestTypeIsHandled(handler.GetHandledType()))
            {
                throw new BusinessException(String.Format("Handler for Type {0} is already registed", handler.GetHandledType().ToString()));
            }
            handlerList.Add(handler);
        }

        public IHandler GetHandlerForRequestType(Type type)
        {
            var handler = GetHandlerByType(type);
            if (handler == null)
            {
                throw new BusinessException(String.Format("Couldn't find Handler for Type {0}", handler.GetHandledType().ToString()));
            }
            return handler;
        }

        private void GetHanlderFromAssebly()
        {           
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;           
            var iDisposableAssemblies = currentAssembly.DefinedTypes.Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(IHandler))).ToList();
            foreach (var typeInfo in iDisposableAssemblies)
            {
                Object handle = Activator.CreateInstance(currentAssembly.GetType(typeInfo.FullName));
                handlerList.Add((IHandler)handle);
            }
        }

        private bool RequestTypeIsHandled(Type type)
        {
            var handler = GetHandlerByType(type);
            if (handler == null)
            {
                return false;
            }
            return true;
        }

        private IHandler GetHandlerByType(Type type)
        {
            try
            {
                var handler = (from item in handlerList where item.GetHandledType() == type select item).Single<IHandler>();
                return handler;
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }      
}
