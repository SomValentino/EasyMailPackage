using System.Threading.Tasks;
using Common.Email.Package.Enums;
using Common.Email.Package.Exceptions;
using Common.Email.Package.Services;
using Common.Email.Package.Tests.Helpers;
using NUnit.Framework;

namespace Common.Email.Package.Tests.IntegrationTests
{
    [TestFixture]
    public class EmailServiceIntegrationTests
    {
        private IEmailService _emailService;
        private EmailMessageHelper _messageHelper;
        private TestEmailMessageBuilder _testEmailMessageBuilder;
        
        [SetUp]
        public void Initialize()
        {
            _emailService = new EmailService();
            _messageHelper = new EmailMessageHelper();
            _testEmailMessageBuilder = new TestEmailMessageBuilder();
            _emailService.emailConfiguration = _messageHelper.TestEmailConfiguration;
        }

        [Test]
        public async Task SendAsync_WhenCalled_ReturnsMessageResultWithSuccessAndEmptyException()
        {
            var message = _testEmailMessageBuilder.AddFromAddress("yourname","ToEmailAddress")
                .AddToAddresses("Receipent name","FromEmailAddress")
                .AddSubject("Test Email")
                .AddBody("Test Email")
                .AddAttachment(_messageHelper.Attachment).Build();
            _emailService.UseSsl = true;
            
            var messageResult = await _emailService.SendAsync(message);

            Assert.That(messageResult, Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Success)); 
            Assert.That(messageResult.Exception, Is.Null);
        }

        [Test]
        public async Task
            SendAsync_WhenCalledWithInvalidSMTPUsername_ReturnMessageResultWithFailureAndEmailConnectionException()
        {
            var message = _testEmailMessageBuilder.AddFromAddress("yourname","ToEmailAddress")
                .AddToAddresses("Receipent name","FromEmailAddress")
                .AddSubject("Test Email")
                .AddBody("Test Email")
                .AddAttachment(_messageHelper.Attachment).Build();
            _messageHelper.TestEmailConfiguration.SmtpUsername = "i";
            _emailService.UseSsl = true;
            
            var messageResult = await _emailService.SendAsync(message);

            Assert.That(messageResult, Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure)); 
            Assert.That(messageResult.Exception, Is.InstanceOf<EmailConnectionException>());
        }
        
    }
}