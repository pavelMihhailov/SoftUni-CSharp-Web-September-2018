using System;

namespace SIS.HTTP.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string DefaultMessage = "Request is not valid.";

        public BadRequestException(string message)
            : base(message)
        {
        }
        
        public static object ThrowFromInvalidRequest() => throw new BadRequestException(DefaultMessage);
    }
}
