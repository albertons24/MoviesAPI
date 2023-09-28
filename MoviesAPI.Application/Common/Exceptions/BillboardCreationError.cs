using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Common.Exceptions
{
    public class BillboardCreationError : Exception
    {
        public BillboardCreationError() : base() { }

        public BillboardCreationError(string message) : base(message) { }

        public BillboardCreationError(string message, Exception innerException) : base(message, innerException) { }
    }
}
