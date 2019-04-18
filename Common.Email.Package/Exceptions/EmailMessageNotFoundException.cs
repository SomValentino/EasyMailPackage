using System;
namespace Common.Email.Package.Exceptions
{
    public class EmailMessageNotFoundException : Exception
    {
        public EmailMessageNotFoundException(string message) : base(message)
        {
        }

        public EmailMessageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}