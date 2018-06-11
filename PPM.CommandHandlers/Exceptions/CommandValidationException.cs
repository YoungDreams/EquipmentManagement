using System;
using System.Collections.Generic;

namespace PPM.CommandHandlers.Exceptions
{
    public class CommandValidationException : ApplicationException
    {
        public IEnumerable<string> ErrorMessages { get; set; }

        public CommandValidationException(IEnumerable<string> errorMessages)
            : base(string.Join(Environment.NewLine, errorMessages))
        {
            ErrorMessages = errorMessages;
        }

        public CommandValidationException(params string[] errorMessages)
            : this((IEnumerable<string>)errorMessages)
        {
        }
    }
}
