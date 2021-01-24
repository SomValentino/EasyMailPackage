using System.Linq;
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
        }

        [Test]
        public async Task SendAsync_WhenCalled_ReturnsMessageResultWithSuccessAndEmptyException()
        {
            var message = _testEmailMessageBuilder.AddFromAddress("yourname","ToEmailAddress")
                .AddToAddresses("Receipent name","FromEmailAddress")
                .AddSubject("Test Email")
                .AddBody("Test Email")
                .AddAttachment(_messageHelper.Attachment).Build();
            
            
            var messageResult = await _emailService.SendAsync(message,_messageHelper.TestEmailConfiguration);

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
            _messageHelper.TestEmailConfiguration.Username = "i";
            
            var messageResult = await _emailService.SendAsync(message,_messageHelper.TestEmailConfiguration);

            Assert.That(messageResult, Is.Not.Null);
            Assert.That(messageResult.Status,Is.EqualTo(Status.Failure)); 
            Assert.That(messageResult.Exception, Is.InstanceOf<EmailConnectionException>());
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public async Task
            GetEmailMessageFromServerViaImapAsync_GivenImapEmailConfigurationAndBatchSize_ReturnExtractedEmails(int batchSize)
        {
            var extractedMessages =
                await _emailService.GetEmailMessageFromServerViaImapAsync(_messageHelper.ImapEmailConfiguration, batchSize);

            var expectedCount = batchSize > extractedMessages.Count() ? extractedMessages.Count() : batchSize;

            Assert.That(extractedMessages, Is.Not.Null);
            Assert.That(extractedMessages.Count(), Is.EqualTo(expectedCount));
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public async Task
            GetEmailMessageFromServerViaImapAsync_GivenPopEmailConfigurationAndBatchSize_ReturnExtractedEmails(int batchSize)
        {
            var extractedMessages =
                await _emailService.GetEmailMessageFromServerViaPopAsync(_messageHelper.Pop3EmailConfiguration, batchSize);

            var expectedCount = batchSize > extractedMessages.Count() ? extractedMessages.Count() : batchSize;

            Assert.That(extractedMessages, Is.Not.Null);
            Assert.That(extractedMessages.Count(), Is.EqualTo(expectedCount));
        }

    }
}