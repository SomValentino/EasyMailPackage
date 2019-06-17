using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Email.Package.Configuration;
using Common.Email.Package.Enums;
using Common.Email.Package.MessageInformation.Implementations;
using Common.Email.Package.MessageInformation.Interfaces;

namespace Common.Email.Package.Services
{
    public interface IEmailService
    {
         HashSet<string> AuthenticationMechanisms {set;}
         EmailContentFormat EmailBodyFormat { set; }
         Task<EmailMessageResult> SendAsync(IEmailMessage message,IEmailConfiguration smtpEmailConfiguration);

         Task<IEnumerable<ExtractedEmailMessage>> GetEmailMessageFromServerViaImapAsync(
             IEmailConfiguration imapEmailConfiguration, int batchSize = -1);

         Task DeleteEmailMessageFromImapServerAsync(IList<ExtractedEmailMessage> messages,
             IEmailConfiguration imapEmailConfiguration);

         Task<IEnumerable<ExtractedEmailMessage>> GetEmailMessageFromServerViaPopAsync(
             IEmailConfiguration popEmailConfiguration, int batchsize = -1);

         Task DeleteEmailMessageFromPopServerAsync(IList<ExtractedEmailMessage> messages,
             IEmailConfiguration popEmailConfiguration);
    }
}