using Common.Email.Package.MessageInformation.Interfaces;
using Common.Email.Package.Services;
using Common.Email.Package.Tests.Helpers;
using NUnit.Framework;

namespace Common.Email.Package.Tests.UnitTests
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
            _emailService = new EmailService();
            _messageHelper = new EmailMessageHelper();
            _testEmailMessageBuilder = new TestEmailMessageBuilder();
        }

    }
}