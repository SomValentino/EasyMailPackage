using Common.Email.Package.Enums;
using Common.Email.Package.Services;
using Common.Email.Package.Tests.Helpers;
using NUnit.Framework;

namespace Common.Email.Package.Tests.IntegrationTests
{
    [TestFixture]
    public class EmailServiceTests
    {
        private IEmailService _emailService;
        private EmailMessageHelper _messageHelper;
        private TestEmailMessageBuilder _testEmailMessageBuilder;
        
        [SetUp]
        public void Initialize()
        {
            _emailService = new Services.EmailService();
            _emailService.emailConfiguration = _messageHelper.TestEmailConfiguration;
            _messageHelper = new EmailMessageHelper();
            _testEmailMessageBuilder = new TestEmailMessageBuilder();
        }

        [Test]
        public void SendAsync_WhenCalled_ReturnsMessageResultWithSuccessAndEmptyException()
        {
            var message = _testEmailMessageBuilder.AddFromAddress(_messageHelper.FromAddress)
                .AddToAddresses("valentine", "valentine.azom@entelect.co.za")
                .AddSubject("Test Email")
                .AddBody("Test Email")
                .AddAttachment(_messageHelper.Attachment).Build();

            var messageResult = _emailService.SendAsync(message);
            
            Assert.That(messageResult, Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Success));
            Assert.That(messageResult.Exception, Is.Null);

        }
        
    }
}