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
    public class DrawMainDeckHandler : IHandler
    {

        GameEngineService gameEngine;

        public DrawMainDeckHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(DrawFromMainDeck)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            DrawFromMainDeck req = request as DrawFromMainDeck;

            gameEngine.DrawFromMainDeck(req.NumberOfCards, UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(DrawFromMainDeck);
        }
    }
}
