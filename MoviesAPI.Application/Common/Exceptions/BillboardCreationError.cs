using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Common.Exceptions
{
    public class BillboardCreationException : Exception
    {
        public BillboardCreationException() : base() { }

        public BillboardCreationException(string message) : base(message) { }

        public BillboardCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
