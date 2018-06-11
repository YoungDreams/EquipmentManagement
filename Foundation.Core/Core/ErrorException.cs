using System;

namespace Foundation.Core
{
    public abstract class ErrorException : ApplicationException
    {
        protected ErrorException(string message)
            : base(message)
        {
        }
    }
}
