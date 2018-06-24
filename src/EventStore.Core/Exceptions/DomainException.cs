using System;

namespace EventStore.Core.Exceptions
{
    public class DomainException: Exception
    {        
        public DomainException(string message = null, Exception innerException = default(Exception))
            :base(message, innerException) { }        
    }
}
