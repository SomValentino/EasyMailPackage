using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Email.Package.Configuration;
using Common.Email.Package.Enums;
using Common.Email.Package.Exceptions;
using Common.Email.Package.MessageInformation.Implementations;
using Common.Email.Package.MessageInformation.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;

namespace Common.Email.Package.Services
{
    public class EmailService : IEmailService
    {
        public HashSet<string> AuthenticationMechanisms { get; set; }
        
        public EmailContentFormat EmailBodyFormat { get; set; }
    
        public EmailMessageResult messageResult {get; set;}

        public EmailService()
        {
            AuthenticationMechanisms = new HashSet<string>();
            messageResult = new EmailMessageResult();
        }

        public async Task<EmailMessageResult> SendAsync(IEmailMessage message,IEmailConfiguration emailConfiguration )
        {

            try
            {
                MimeMessage mailMessage = GetInternetAddress(message);
                // check if email configuration has been set if not throw an exception
                if (emailConfiguration == null)
                {
                    throw new EmailConfigurationNotFoundException("Email Configuration is not set");
                }

                using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    await ConnectToServer(emailConfiguration, emailClient);
                    await emailClient.SendAsync(mailMessage);
                    //message was successful update the status of the EmailMessageResult object
                    messageResult.Status = Status.Success;
                    await emailClient.DisconnectAsync(true);
                    return messageResult;
                }
            }
            catch (EmailMessageNotFoundException ex)
            {
                SetExceptionResult<EmailMessageNotFoundException>(ex);
                return messageResult;
            }
            catch (EmailToAddressNotFoundException ex)
            {
                SetExceptionResult<EmailToAddressNotFoundException>(ex);
                return messageResult;
            }
            catch (EmailFromAddressNotFoundException ex)
            {
                SetExceptionResult<EmailFromAddressNotFoundException>(ex);
                return messageResult;
            }
            catch (EmailSubjectNotFoundException ex)
            {
                SetExceptionResult<EmailSubjectNotFoundException>(ex);
                return messageResult;
            }
            catch (EmailConfigurationNotFoundException ex)
            {
                SetExceptionResult<EmailConfigurationNotFoundException>(ex);
                return messageResult;
            }
            catch (System.Exception ex)
            {
                var emailConnectionException = new EmailConnectionException(ex.Message,ex);
                SetExceptionResult<EmailConnectionException>(emailConnectionException);
                return messageResult;
            }
        }

        private async Task ConnectToServer(IEmailConfiguration emailConfiguration, IMailService emailClient)
        {
            await emailClient.ConnectAsync(emailConfiguration.ServerAddress, emailConfiguration.Port,
                emailConfiguration.RequireSSL);
            // if the user has added extra authentication mechnanisms add them to the email client
            if (AuthenticationMechanisms != null && AuthenticationMechanisms.Any())
            {
                foreach (var authMechanism in AuthenticationMechanisms)
                {
                    emailClient.AuthenticationMechanisms.Add(authMechanism);
                }
            }

            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
            await emailClient.AuthenticateAsync(emailConfiguration.Username,
                emailConfiguration.Password);
        }

        public async Task<IEnumerable<ExtractedEmailMessage>> GetEmailMessageFromServerViaImapAsync(IEmailConfiguration imapEmailConfiguration, int batchSize = -1)
        {
            var results = new List<ExtractedEmailMessage>();
            using (var client = new ImapClient())
            {
                await ConnectToServer(imapEmailConfiguration, client);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var messageItems = await inbox.FetchAsync(0, batchSize -1,
                    MessageSummaryItems.UniqueId | MessageSummaryItems.Size | MessageSummaryItems.Flags);
                foreach (var item in messageItems)
                {
                    var message = inbox.GetMessage(item.UniqueId);
                    var extractedEmailMessage = new ExtractedEmailMessage
                    {
                        FromAddress = message.From.Mailboxes.FirstOrDefault()?.Address,
                        ToAddress = message.To.Mailboxes.Select(x => x.Address).ToList(),
                        TextBody = message.TextBody,
                        HtmlBody = message.HtmlBody,
                        Subject = message.Subject,
                        MessageUniqueId = item.UniqueId.ToString(),
                        Attachments = await GetAttachmentsInBase64String(message)
                    };
                    results.Add(extractedEmailMessage);
                }
                client.Disconnect(true);
            }

            return results;
        }

        public async Task DeleteEmailMessageFromImapServerAsync(IList<ExtractedEmailMessage> messages, IEmailConfiguration imapEmailConfiguration)
        {
            using (var client = new ImapClient())
            {
                await ConnectToServer(imapEmailConfiguration, client);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);
                var uids = messages.Select(x => UniqueId.Parse(x.MessageUniqueId)).ToList();

                inbox.AddFlags(uids, MessageFlags.Deleted, false);
                await inbox.ExpungeAsync();
                
                client.Disconnect(true);
            }
        }

        public async Task<IEnumerable<ExtractedEmailMessage>> GetEmailMessageFromServerViaPopAsync(IEmailConfiguration popEmailConfiguration, int batchsize = -1)
        {
            var results = new List<ExtractedEmailMessage>();
            using (var client = new Pop3Client())
            {
                await ConnectToServer(popEmailConfiguration, client);
                var totalMessages = batchsize <= -1 ? client.Count : batchsize;

                for (int i = 0; i < totalMessages; i++)
                {
                    var uid = client.GetMessageUid(i);
                    var message = client.GetMessage(i);

                    var extractedMessage = new ExtractedEmailMessage
                    {
                        FromAddress = message.From.Mailboxes.FirstOrDefault()?.Address,
                        ToAddress = message.To.Mailboxes.Select(x => x.Address).ToList(),
                        HtmlBody = message.HtmlBody,
                        TextBody = message.TextBody,
                        Subject = message.Subject,
                        MessageUniqueId = uid,
                        Attachments = await GetAttachmentsInBase64String(message)
                    };
                    
                    results.Add(extractedMessage);
                }
                
                await client.DisconnectAsync(true);
            }

            return results;
        }

        public async Task DeleteEmailMessageFromPopServerAsync(IList<ExtractedEmailMessage> messages, IEmailConfiguration popEmailConfiguration)
        {
            using (var client = new Pop3Client())
            {
                await ConnectToServer(popEmailConfiguration, client);
                var uids = messages.Select(x => UniqueId.Parse(x.MessageUniqueId)).ToList();

                for (int i = 0; i < client.Count; i++)
                {
                    var message = client.GetMessage(i);
                    var uid = client.GetMessageUid(i);
                    if (uids.Any(x => x.ToString().Equals(uid)))
                    {
                        await client.DeleteMessageAsync(i);
                    }
                }

                await client.DisconnectAsync(true);
            }
        }

        private async Task<IList<string>> GetAttachmentsInBase64String(MimeMessage message)
        {
            var attachments = message.Attachments;
            IList<string> results = null;
            if (attachments != null)
            {
                results = new List<string>();
                foreach (var attachment in attachments)
                {
                    using (var stream = new MemoryStream())
                    {
                        if (attachment is MessagePart)
                        {
                            var part = (MessagePart) attachment;
                            await part.Message.WriteToAsync(stream);
                        }
                        else
                        {
                            var part = (MimePart) attachment;
                            await part.Content.DecodeToAsync(stream);
                        }

                        stream.Seek(0, SeekOrigin.Begin);
                        var streamArray = stream.ToArray();
                        var filestring = Convert.ToBase64String(streamArray);
                        results.Add(filestring);
                    }
                }
            }

            return results;
        }

        private void SetExceptionResult<T>(Exception ex) where T : Exception
        {
            messageResult.Status = Status.Failure;
            messageResult.Exception = (T) ex;
        }

        private MimeMessage GetInternetAddress(IEmailMessage emailMessage){
            var message = new MimeMessage();
            // do a null check to be sure the email object is not null
            ValidateEmailMessage(emailMessage);
            //make sure aleast one email address is included in the to address list
            ValidateAndAddToAddress(emailMessage, message);
            // make the from address exist else throw an exception
            ValidateAndAddFromAddress(emailMessage, message);
            // add Blind copy address
            AddBccAddress(emailMessage, message);
            // add copy to address
            AddCcAddresses(emailMessage, message);
            // make sure there is an email subject;
            ValidateAndAddEmailSubject(emailMessage, message);
            
            var builder = GetEmailBodyBuilder(emailMessage);

            //if the email message has attachements add the attachements
            AddAttachments(emailMessage, builder);
            message.Body = builder.ToMessageBody();
            return message;
        }

        private void ValidateAndAddEmailSubject(IEmailMessage emailMessage, MimeMessage message)
        {
            if (!string.IsNullOrEmpty(emailMessage.Subject) || !string.IsNullOrWhiteSpace(emailMessage.Subject))
            {
                message.Subject = emailMessage.Subject;
            }
            else
            {
                throw new EmailSubjectNotFoundException("The Subject of the email message is empty");
            }
        }

        private BodyBuilder GetEmailBodyBuilder(IEmailMessage emailMessage)
        {
            BodyBuilder builder;
            switch (EmailBodyFormat)
            {
                case EmailContentFormat.Html:
                    builder = new BodyBuilder {HtmlBody = emailMessage.Content};
                    break;
                default:
                    builder = new BodyBuilder {TextBody = emailMessage.Content};
                    break;
            }
            return builder;
        }

        private void AddAttachments(IEmailMessage emailMessage, BodyBuilder builder)
        {
            if (emailMessage.Attachments != null && emailMessage.Attachments.Any())
            {
                foreach (var attachment in emailMessage.Attachments)
                {
                    builder.Attachments.Add(attachment.FileName);
                }
            }
        }

        private void AddCcAddresses(IEmailMessage emailMessage, MimeMessage message)
        {
            if (emailMessage.CCAddress != null && emailMessage.CCAddress.Any())
            {
                message.Cc.AddRange(emailMessage.CCAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
            }
        }

        private void AddBccAddress(IEmailMessage emailMessage, MimeMessage message)
        {
            if (emailMessage.BccAddress != null && emailMessage.BccAddress.Any())
            {
                message.Bcc.AddRange(emailMessage.BccAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
            }
        }

        private void ValidateAndAddFromAddress(IEmailMessage emailMessage, MimeMessage message)
        {
            if (emailMessage.FromAddress != null)
            {
                message.From.Add(new MailboxAddress(emailMessage.FromAddress.Name, emailMessage.FromAddress.Address));
            }
            else
            {
                throw new EmailFromAddressNotFoundException("Email message does not contain any FromAddress email");
            }
        }

        private void ValidateAndAddToAddress(IEmailMessage emailMessage, MimeMessage message)
        {
            if (emailMessage.ToAddress != null && emailMessage.ToAddress.Any())
            {
                message.To.AddRange(emailMessage.ToAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
            }
            else
            {
                throw new EmailToAddressNotFoundException("Email message does not contain any ToAddress emails");
            }
        }

        private void ValidateEmailMessage(IEmailMessage emailMessage)
        {
            if (emailMessage == null)
                throw new EmailMessageNotFoundException("Email Message is null and contains no information");
        }
    }
}