using System.IO;
using System.Collections.Generic;

namespace Common.Email.Package.MessageInformation.Interfaces
{
    public interface IEmailMessageBuilder
    {
         IEmailMessageBuilder AddToAddresses(IEmailAddress emailAddress);
         IEmailMessageBuilder AddToAddresses(IEnumerable<IEmailAddress> emailAddresses);
         IEmailMessageBuilder AddToAddresses(string name, string address);
         IEmailMessageBuilder AddFromAddress(string name, string address);
         IEmailMessageBuilder AddFromAddress(IEmailAddress emailAddress);
         IEmailMessageBuilder AddCCAddresses(IEmailAddress emailAddress);
         IEmailMessageBuilder AddCCAddresses(IEnumerable<IEmailAddress> emailAddresses);
         IEmailMessageBuilder AddCCAddresses(string name, string address);
         IEmailMessageBuilder AddBCCAddresses(IEmailAddress emailAddress);
         IEmailMessageBuilder AddBCCAddresses(IEnumerable<IEmailAddress> emailAddresses);
         IEmailMessageBuilder AddBCCAddresses(string name, string address);
         IEmailMessageBuilder AddSubject(string subject);
         IEmailMessageBuilder AddBody(string body);
         IEmailMessageBuilder AddAttachment(Attachment attachment);
         IEmailMessageBuilder AddAttachment(string fileName);
         IEmailMessageBuilder AddAttachment(IEnumerable<Attachment> attachments);
         IEmailMessage Build();
    }
}