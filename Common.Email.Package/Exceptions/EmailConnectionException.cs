using System;
namespace Common.Email.Package.Exceptions
{
    public class EmailConnectionException : Exception
    {
        public EmailConnectionException(string message) : base(message)
        {
        }

        public EmailConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}