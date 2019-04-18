using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Email.Package.Configuration;
using Common.Email.Package.MessageInformation.Interfaces;

namespace Common.Email.Package.Services
{
    public interface IEmailService
    {
         IEmailConfiguration emailConfiguration {set;}
         HashSet<string> AuthenticationMechanisms {set;}
         Task<EmailMessageResult> SendAsync(IEmailMessage message);
    }
}