using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class SodukoException : Exception
    {
        public SodukoException()
        {
        }

        public SodukoException(string message)
            : base(message)
        {
        }

        public SodukoException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
