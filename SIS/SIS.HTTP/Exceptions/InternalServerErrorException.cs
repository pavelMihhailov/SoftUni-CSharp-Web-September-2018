using System;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public const string DefaultMessage = "The Server has encountered an error.";

        public InternalServerErrorException(string message) : base(message)
        {
        }

        public static object ThrowServerError() => throw new InternalServerErrorException(DefaultMessage);
    }
}
