using System;

namespace Common.Email.Package.Exceptions
{
    public class EmailProcessAttachmentException : Exception
    {
        public EmailProcessAttachmentException(string message) : base(message)
        {
                
        }

        public EmailProcessAttachmentException(string message, Exception innerException): base(message,innerException)
        {
            
        }
    }
}