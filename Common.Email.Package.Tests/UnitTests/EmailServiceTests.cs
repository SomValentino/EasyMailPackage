using System.Threading.Tasks;
using Common.Email.Package.Enums;
using Common.Email.Package.Exceptions;
using Common.Email.Package.MessageInformation;
using Common.Email.Package.MessageInformation.Implementations;
using Common.Email.Package.Services;
using NUnit.Framework;

namespace Common.Email.Package.Tests.UnitTests
{
    [TestFixture]
    public class EmailServiceTests
    {
        private IEmailService _emailService;
        
        [SetUp]
        public void Initialize()
        {
            _emailService = new EmailService();
        }

        [Test]
        public async Task SendAsync_GivenAnEmptyEmailMessage_ReturnsMessageResultWithFailureAndEmailMessageNotFoundException()
        {
            EmailMessage message = null;
            var messageResult = await _emailService.SendAsync(message);
            Assert.That(messageResult,Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure));
            Assert.That(messageResult.Exception,Is.InstanceOf<EmailMessageNotFoundException>());
        }

        [Test]
        public async Task
            SendAsync_GivenAnEmailMessageWithNoToAddress_ReturnsMessageResultWithFailureAndEmailToAddressNotFoundException()
        {
            EmailMessage message = new EmailMessage();
            var messageResult = await _emailService.SendAsync(message);
            Assert.That(messageResult,Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure));
            Assert.That(messageResult.Exception,Is.InstanceOf<EmailToAddressNotFoundException>());
        }

        [Test]
        public async Task
            SendAsync_GivenAnEmailMessageWithNoFromAddress_ReturnsMessageResultWithFailureAndEmailFromAddressNotFoundException()
        {
            EmailMessage message = new EmailMessage();
            message.ToAddress.Add(new EmailAddress{Name="",Address = ""});
            var messageResult = await _emailService.SendAsync(message);
            Assert.That(messageResult,Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure));
            Assert.That(messageResult.Exception,Is.InstanceOf<EmailFromAddressNotFoundException>());
        }

        [Test]
        public async Task
            SendAsync_GivenEmailMessageWithNoSubject_ReturnsMessageResultWithFailureAndEmailSubjectNotFoundException()
        {
            EmailMessage message = new EmailMessage();
            message.ToAddress.Add(new EmailAddress{Name="",Address = ""});
            message.FromAddress = new EmailAddress{Name ="",Address = ""};
            var messageResult = await _emailService.SendAsync(message);
            Assert.That(messageResult,Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure));
            Assert.That(messageResult.Exception,Is.InstanceOf<EmailSubjectNotFoundException>());
        }
        
        [Test]
        public async Task
            SendAsync_WhenCalledWithNoEmailConfiguration_ReturnsMessageResultWithFailureAndEmailConfigurationNotFoundException()
        {
            EmailMessage message = new EmailMessage();
            message.ToAddress.Add(new EmailAddress{Name="",Address = ""});
            message.FromAddress = new EmailAddress{Name ="",Address = ""};
            message.Subject = "MyName";
            var messageResult = await _emailService.SendAsync(message);
            Assert.That(messageResult,Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure));
            Assert.That(messageResult.Exception,Is.InstanceOf<EmailConfigurationNotFoundException>());
        }
    }
}