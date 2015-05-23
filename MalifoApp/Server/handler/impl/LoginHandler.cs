using Common.types;
using Common.types.clientNotifications;
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
        GameEngineService _gameEngine;

        public LoginHandler()
        {
            _userService = ServiceManager.Instance.GetServiceByType(typeof(LoginRequest)) as UserService;
            _gameEngine = ServiceManager.Instance.GetServiceByType(typeof(NewPlayer)) as GameEngineService;
        }
        public Response HandleRequest(Request request)
        {
            LoginRequest req = request as LoginRequest;
            _userService.UserLogin(req.ClientHash, req.UserName, req.AsFatemaster);
            _gameEngine.PlayerConnected(req.UserName, req.AsFatemaster);
            return new LoginResponse()
            {
                // noone objects, so we assume that if we wanted to be fatemaster, that worked
                IsFatemaster = req.AsFatemaster
            };
        }

        public Type GetHandledType()
        {
            return typeof(LoginRequest);
        }
    }
}
