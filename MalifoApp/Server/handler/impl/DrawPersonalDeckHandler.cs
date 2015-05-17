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
    public class DrawPersonalDeckHandler : IHandler
    {

        private GameEngineService gameEngine;

        public DrawPersonalDeckHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(DrawFromPersonalDeck)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            DrawFromPersonalDeck req = request as DrawFromPersonalDeck;

            gameEngine.DrawFromPersonalDeck(req.NumberOfCards, UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(DrawFromPersonalDeck);
        }
    }
}
