using System;
using System.Runtime.Serialization;

namespace Common.Email.Package.Exceptions
{
    public class EmailConfigurationNotFoundException : Exception
    {
        public EmailConfigurationNotFoundException(string message) : base(message)
        {
        }

        public EmailConfigurationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}