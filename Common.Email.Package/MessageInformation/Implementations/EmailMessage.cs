using System.Collections.Generic;
using Common.Email.Package.MessageInformation.Interfaces;

namespace Common.Email.Package.MessageInformation.Implementations
{
    public class EmailMessage : IEmailMessage
    {
        public List<IEmailAddress> ToAddress { get; set; }
        public List<IEmailAddress> CCAddress { get; set; }
        public List<IEmailAddress> BccAddress { get; set; }
        public IEmailAddress FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}