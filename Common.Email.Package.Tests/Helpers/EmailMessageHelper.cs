using System.Collections.Generic;
using System.IO;
using Common.Email.Package.MessageInformation;
using Common.Email.Package.MessageInformation.Interfaces;

namespace Common.Email.Package.Tests.Helpers
{
    public class EmailMessageHelper
    {
        public List<IEmailAddress> ToAddresses { get; }
        public EmailAddress FromAddress { get; }
        public List<IEmailAddress> CcAddresses { get; }
        public List<IEmailAddress> BccAddress { get; }
        public List<Attachment> Attachment { get; }
        public TestEmailConfiguration TestEmailConfiguration { get; }

        public EmailMessageHelper()
        {
            ToAddresses = new List<IEmailAddress>
            {
                new EmailAddress
                {
                    Name = "John",
                    Address = "John@gmail.com"
                },
                new EmailAddress
                {
                    Name = "Joe",
                    Address = "joe@gmail.com"
                }
            };
            FromAddress = new EmailAddress
            {
                Name = "val",
                Address = "val@gmail.com"
            };
            CcAddresses = new List<IEmailAddress>
            {
                new EmailAddress
                {
                    Name = "Mary",
                    Address = "mary@gmail.com"
                }
            };
            BccAddress = new List<IEmailAddress>
            {
                new EmailAddress
                {
                    Name = "ben",
                    Address = "ben@gmail.com"
                }
            };
            Attachment = new List<Attachment>
            {
                new Attachment
                {
                    FileName = "EmailFile.txt",
                }
            };
            TestEmailConfiguration = new TestEmailConfiguration
            {
                SmtpServer = "smtp.gmail.com",
                SmtpUsername = "YOUREMAIL@gmail.com",
                SmtpPassword = "YOURPASSWORD",
                SmtpPort = 465
            };
        }
        
        
    }
}