using System;
namespace Common.Email.Package.Exceptions
{
    public class EmailToAddressNotFoundException : Exception
    {
        public EmailToAddressNotFoundException(string message) : base(message)
        {
        }

        public EmailToAddressNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}