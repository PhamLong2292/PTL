using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Utilities.Exceptions
{
    public class PTLException : Exception
    {
        public PTLException()
        {
        }

        public PTLException(string message)
            : base(message)
        {
        }

        public PTLException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}