using Common.types;
using Common.types.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.handler.impl
{
    public class LoginHandler : IHandler
    {
        public Response HandleRequest(Request request)
        {
            Guid guid = Guid.NewGuid();
            return new LoginResponse()
            {
                UserHash = guid.ToString()
            };
        }

        public Type GetHandledType()
        {
            return typeof (LoginRequest);
        }
    }
}
