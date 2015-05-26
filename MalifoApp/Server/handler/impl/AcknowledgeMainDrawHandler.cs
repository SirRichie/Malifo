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
    public class AcknowledgeMainDrawHandler : IHandler
    {

        GameEngineService gameEngine;

        public AcknowledgeMainDrawHandler()
        {
            gameEngine = ServiceManager.Instance.GetServiceByType(typeof(AcknowledgeMainDraw)) as GameEngineService;
        }



        public Response HandleRequest(Request request)
        {
            AcknowledgeMainDraw req = request as AcknowledgeMainDraw;

            gameEngine.AcknowledgeMainDraw(UserManager.Instance.GetUserInfoByHash(req.ClientHash));

            return null;
        }

        public Type GetHandledType()
        {
            return typeof(AcknowledgeMainDraw);
        }
    }
}
