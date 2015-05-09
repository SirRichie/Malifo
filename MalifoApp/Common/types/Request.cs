using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types
{
    [Serializable]
    public class Request : ITransferableObject
    {
        public string ClientHash { get; set; }
        public string MessageHash { get; set; }
    }
}
