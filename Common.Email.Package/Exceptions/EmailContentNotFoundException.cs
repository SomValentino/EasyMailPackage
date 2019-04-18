using System;
namespace Common.Email.Package.Exceptions
{
    public class EmailContentNotFoundException : Exception
    {
        public EmailContentNotFoundException(string message) : base(message)
        {
        }

        public EmailContentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}