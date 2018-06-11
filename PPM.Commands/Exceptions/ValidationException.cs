using System;

namespace PPM.Commands.Exceptions
{
    public class ValidationException:ApplicationException
    {
        public ValidationException(string message):base(message)
        {
        }
    }
}
