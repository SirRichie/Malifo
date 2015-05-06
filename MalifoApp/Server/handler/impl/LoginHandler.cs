using Common.types;
using Common.types.impl;
using Server.Services;
using Server.userManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.handler.impl
{
    public class LoginHandler : IHandler
    {
        UserService _userService;
        public LoginHandler()
        {
            _userService = ServiceManager.Instance.UserService;
        }
        public Response HandleRequest(Request request)
        {
            LoginRequest req = request as LoginRequest;
            _userService.UserLogin(req.ClientHash, req.UserName);
            return new LoginResponse()
            {
               
            };
        }

        public Type GetHandledType()
        {
            return typeof (LoginRequest);
        }
    }
}
