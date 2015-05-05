using Common.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.handler
{
    public interface IHandler
    {
        Response HandleRequest(Request request);
        Type GetHandledType();
    }
}
