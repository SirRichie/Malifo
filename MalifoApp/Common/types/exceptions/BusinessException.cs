using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.exceptions
{
    [Serializable]
    public class BusinessException : Exception, ITransferableObject
    {
        public BusinessException()
            : base()
        {

        }

        public BusinessException(string msg)
            : base(msg)
        {

        }

        public BusinessException(string msg, Exception e)
            : base(msg, e)
        {

        }
  
        protected BusinessException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }


        public string ClientHash
        { get; set;}


    }
}
