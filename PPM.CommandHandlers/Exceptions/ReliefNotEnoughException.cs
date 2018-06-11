using System;

namespace PPM.CommandHandlers.Exceptions
{
    public class ReliefNotEnoughException : ApplicationException
    {
        public ReliefNotEnoughException(string message):base(message)
        {
            
        }
    }
}
