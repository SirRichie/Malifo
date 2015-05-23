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
            ShuffleMainDeck req = request as ShuffleMainDeck;

            gameEngine.ShuffleMainDeck(UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(ShuffleMainDeck);
        }
    }
}
