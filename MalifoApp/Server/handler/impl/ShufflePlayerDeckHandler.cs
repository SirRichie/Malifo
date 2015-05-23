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
    public class ShufflePlayerDeckHandler : IHandler
    {

        private GameEngineService gameEngine;

        public ShufflePlayerDeckHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(ShufflePlayerDeck)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            ShufflePlayerDeck req = request as ShufflePlayerDeck;

            gameEngine.ShufflePlayerDeck(req.PlayerName, UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(ShufflePlayerDeck);
        }
    }
}
