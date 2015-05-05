using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.impl
{
    [Serializable]
    public class LoginRequest: Request
    {
        public string ClientHash { get; set; }
        public string UserName { get; set; }
    }  
}
