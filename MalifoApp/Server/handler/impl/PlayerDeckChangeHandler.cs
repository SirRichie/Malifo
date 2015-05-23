using Common.models;
using Common.types;
using Common.types.clientNotifications;
using Server.Services;
using Server.userManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.handler.impl
{
    public class PlayerDeckChangeHandler : IHandler
    {

        private GameEngineService gameEngine;

        public PlayerDeckChangeHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(PlayerDeckChange)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            PlayerDeckChange req = request as PlayerDeckChange;

            gameEngine.PlayerDeckChange(req.PlayerName, req.PlayerDeck, UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(PlayerDeckChange);
        }
    }
}
