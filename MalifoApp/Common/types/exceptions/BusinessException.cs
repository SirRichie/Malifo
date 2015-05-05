using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.exceptions
{
    public class BusinessException : Exception
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
    }
}
