using System;
namespace Common.Email.Package.Exceptions
{
    public class EmailFromAddressNotFoundException : Exception
    {
        public EmailFromAddressNotFoundException(string message) : base(message)
        {
        }

        public EmailFromAddressNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}