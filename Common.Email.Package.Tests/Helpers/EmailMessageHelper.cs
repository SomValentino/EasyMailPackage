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
        public TestEmailConfiguration Pop3EmailConfiguration { get; }
        public TestEmailConfiguration ImapEmailConfiguration { get;}

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
                ServerAddress = "smtp.gmail.com",
                Username = "valazom@gmail.com",
                Password = "Az0m-12345@#",
                Port = 465
            };

            Pop3EmailConfiguration = new TestEmailConfiguration
            {
                ServerAddress = "pop.gmail.com",
                Username = "valazom@gmail.com",
                Password = "Az0m-12345@#",
                Port = 995,
                RequireSSL = true
            };

            ImapEmailConfiguration = new TestEmailConfiguration
            {
                ServerAddress = "imap.gmail.com",
                Username = "valazom@gmail.com",
                Password = "Az0m-12345@#",
                Port = 993,
                RequireSSL = true
            };
        }
        
        
    }
}