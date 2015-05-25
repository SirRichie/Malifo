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
    public class DiscardCardHandler : IHandler
    {

        GameEngineService gameEngine;

        public DiscardCardHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(DiscardCard)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            DiscardCard req = request as DiscardCard;

            gameEngine.DiscardCard(req.Card, UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(DiscardCard);
        }
    }
}
