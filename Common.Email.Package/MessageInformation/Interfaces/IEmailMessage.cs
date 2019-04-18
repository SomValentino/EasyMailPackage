using System.Collections.Generic;
using MimeKit;

namespace Common.Email.Package.MessageInformation.Interfaces
{
    public interface IEmailMessage
    {
         List<IEmailAddress> ToAddress { get; set; }
         List<IEmailAddress> CCAddress{get; set;}
         List<IEmailAddress> BccAddress {get; set;}
         IEmailAddress FromAddress {get; set;}
         string Subject {get; set;}
         string Content {get; set;}
         List<Attachment> Attachments {get; set;}
    }
}