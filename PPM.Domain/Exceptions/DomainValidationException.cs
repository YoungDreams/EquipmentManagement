using System;

namespace PPM.Entities.Exceptions
{
    public class DomainValidationException : ApplicationException
    {
        public DomainValidationException(string message):base(message)
        {
        }
    }
}
