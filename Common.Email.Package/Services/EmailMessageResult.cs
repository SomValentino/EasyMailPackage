using Common.Email.Package.Enums;
using Common.Email.Package.Exceptions;

namespace Common.Email.Package.Services
{
    public class EmailMessageResult
    {
        public Status EmailStatus{ get; set; }
        public EmailConnectionException Exception { get; set; }
    }
}