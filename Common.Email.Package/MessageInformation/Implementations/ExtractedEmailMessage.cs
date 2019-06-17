using System.Collections.Generic;

namespace Common.Email.Package.MessageInformation.Implementations
{
    public class ExtractedEmailMessage
    {
        public string FromAddress { get; set; }
        public IList<string> ToAddress { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
        public IList<string> Attachments { get; set; }
        public string MessageUniqueId { get; set; }
    }
}