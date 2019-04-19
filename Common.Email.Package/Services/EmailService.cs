using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Email.Package.Configuration;
using Common.Email.Package.Enums;
using Common.Email.Package.Exceptions;
using Common.Email.Package.MessageInformation.Interfaces;
using MimeKit;

namespace Common.Email.Package.Services
{
    public class EmailService : IEmailService
    {
        public IEmailConfiguration emailConfiguration { get; set; }
        public HashSet<string> AuthenticationMechanisms { get; set; }
        
        public EmailContentFormat EmailBodyFormat { get; set; }
        public EmailMessageResult messageResult {get; set;}

        public EmailService()
        {
            AuthenticationMechanisms = new HashSet<string>();
            messageResult = new EmailMessageResult();
        }

        public async Task<EmailMessageResult> SendAsync(IEmailMessage message)
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
                    await emailClient.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort, true);
                    // if the user has added extra authentication mechnanisms add them to the email client
                    if (AuthenticationMechanisms != null && AuthenticationMechanisms.Any())
                    {
                        foreach (var authMechanism in AuthenticationMechanisms)
                        {
                            emailClient.AuthenticationMechanisms.Add(authMechanism);
                        }
                    }

                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    await emailClient.AuthenticateAsync(emailConfiguration.SmtpUsername,
                        emailConfiguration.SmtpPassword);
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
                SetExceptionResult<EmailConnectionException>(ex);
                return messageResult;
            }
        }

        private void SetExceptionResult<T>(Exception ex) where T : Exception
        {
            messageResult.Status = Status.Failure;
            messageResult.Exception = (T)ex;
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
                    builder.Attachments.Add(attachment.FileName, attachment.FileStream,
                        new ContentType(attachment.MediaType, attachment.MediaSubType));
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