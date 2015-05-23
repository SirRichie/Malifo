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
    public class ShuffleMainDeckHandler : IHandler
    {

        private GameEngineService gameEngine;

        public ShuffleMainDeckHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(ShuffleMainDeck)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            ShufflePlayerDeck req = request as ShufflePlayerDeck;

            // TODO make change if sender is fatemaster
            //UserManager.Instance.GetUserInfoByHash(req.ClientHash)

            gameEngine.ShuffleMainDeck();

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(ShuffleMainDeck);
        }
    }
}
