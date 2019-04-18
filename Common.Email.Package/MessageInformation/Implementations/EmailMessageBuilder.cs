using System.Collections.Generic;
using System.IO;
using Common.Email.Package.MessageInformation.Interfaces;

namespace Common.Email.Package.MessageInformation.Implementations
{
    public class EmailMessageBuilder : IEmailMessageBuilder
    {
        private List<Attachment> attachments;
        private List<IEmailAddress> ToAddresses;
        private List<IEmailAddress> CCAddresses;
        private List<IEmailAddress> BCCAddresses;
        private IEmailAddress FromAddress;
        private string subject;
        private string body;
        public EmailMessageBuilder()
        {
            attachments = new List<Attachment>();
            ToAddresses = new List<IEmailAddress>();
            CCAddresses = new List<IEmailAddress>();
            BCCAddresses = new List<IEmailAddress>();
        }
        public IEmailMessageBuilder AddAttachment(Attachment attachment)
        {
            attachments.Add(attachment);
            return this;
        }

        public IEmailMessageBuilder AddAttachment(string fileName, Stream fileStreame, string mediaType, string mediaSubType)
        {
            attachments.Add(new Attachment{ FileName = fileName, FileStream = fileStreame,
                        MediaType = mediaType,MediaSubType = mediaSubType});
            return this;
        }

        public IEmailMessageBuilder AddAttachment(IEnumerable<Attachment> attachments)
        {
            this.attachments.AddRange(attachments);
            return this;
        }

        public IEmailMessageBuilder AddBCCAddresses(IEmailAddress emailAddress)
        {
            BCCAddresses.Add(emailAddress);
            return this;
        }

        public IEmailMessageBuilder AddBCCAddresses(IEnumerable<IEmailAddress> emailAddresses)
        {
            BCCAddresses.AddRange(emailAddresses);
            return this;
        }

        public IEmailMessageBuilder AddBCCAddresses(string name, string address)
        {
            BCCAddresses.Add(new EmailAddress{Name = name, Address = address});
            return this;
        }

        public IEmailMessageBuilder AddBody(string body)
        {
            this.body = body;
            return this;
        }

        public IEmailMessageBuilder AddCCAddresses(IEmailAddress emailAddress)
        {
            CCAddresses.Add(emailAddress);
            return this;
        }

        public IEmailMessageBuilder AddCCAddresses(IEnumerable<IEmailAddress> emailAddresses)
        {
            CCAddresses.AddRange(emailAddresses);
            return this;
        }

        public IEmailMessageBuilder AddCCAddresses(string name, string address)
        {
            CCAddresses.Add(new EmailAddress{Name = name, Address = address});
            return this;
        }

        public IEmailMessageBuilder AddFromAddress(string name, string address)
        {
            FromAddress = new EmailAddress {Name = name, Address = address};
            return this;
        }

        public IEmailMessageBuilder AddFromAddress(IEmailAddress emailAddress)
        {
            FromAddress = emailAddress;
            return this;
        }

        public IEmailMessageBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public IEmailMessageBuilder AddToAddresses(IEmailAddress emailAddress)
        {
            ToAddresses.Add(emailAddress);
            return this;
        }

        public IEmailMessageBuilder AddToAddresses(IEnumerable<IEmailAddress> emailAddresses)
        {
            ToAddresses.AddRange(emailAddresses);
            return this;
        }

        public IEmailMessageBuilder AddToAddresses(string name, string address)
        {
            ToAddresses.Add(new EmailAddress {Name = name, Address = address});
            return this;
        }

        public IEmailMessage Build()
        {
            var message = new EmailMessage{
                ToAddress = this.ToAddresses,
                CCAddress = this.CCAddresses,
                BccAddress = this.BCCAddresses,
                FromAddress = this.FromAddress,
                Subject = this.subject,
                Content = this.body,
                Attachments = this.attachments
            };
            return message;
        }
    }
}