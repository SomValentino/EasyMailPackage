using System;

namespace Common.Email.Package.Exceptions
{
    public class EmailSubjectNotFoundException : Exception
    {
        public EmailSubjectNotFoundException(string message) : base(message)
        {
        }

        public EmailSubjectNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}