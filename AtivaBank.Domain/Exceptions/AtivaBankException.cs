using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtivaBank.Domain.Exceptions
{

    [Serializable]
    public class AtivaBankException : Exception
    {
        public AtivaBankException() { }
        public AtivaBankException(string message) : base(message) { }
        public AtivaBankException(string message, Exception inner) : base(message, inner) { }
        protected AtivaBankException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
