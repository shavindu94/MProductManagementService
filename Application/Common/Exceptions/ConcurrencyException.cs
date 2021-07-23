using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public  class ConcurrencyException : Exception
    {
        public ConcurrencyException()
           : base()
        {
        }

        public ConcurrencyException(string message)
            : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConcurrencyException(string name, object key)
            : base($"Entity \"{name}\" ({key}) updated by another user")
        {

        }

    }
}
