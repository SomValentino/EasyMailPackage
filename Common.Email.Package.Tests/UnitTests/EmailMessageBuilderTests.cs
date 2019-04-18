using System.Collections.Generic;
using System.IO;
using Common.Email.Package.MessageInformation;
using Common.Email.Package.MessageInformation.Interfaces;
using Common.Email.Package.Tests.Helpers;
using NUnit.Framework;

namespace Common.Email.Package.Tests.UnitTests
{
    [TestFixture]
    public class EmailMessageBuilderTests
    {
        private TestEmailMessageBuilder _testEmailMessageBuilder;
        private EmailMessageHelper _messageHelper;

        [SetUp]
        public void Initialize()
        {
            _testEmailMessageBuilder = new TestEmailMessageBuilder();
            _messageHelper = new EmailMessageHelper();
        }
        
        [Test]
        public void GivenEmailParameters_WhenCalled_ReturnsAnEmailMessageObject()
        {
            string subject = "Test email";
            string body = "Test body";
            var emailMessage = _testEmailMessageBuilder
                .AddToAddresses(_messageHelper.ToAddresses)
                .AddFromAddress(_messageHelper.FromAddress)
                .AddCCAddresses(_messageHelper.CcAddresses)
                .AddBCCAddresses(_messageHelper.BccAddress)
                .AddSubject(subject)
                .AddBody(body)
                .AddAttachment(_messageHelper.Attachment).Build();
            
            Assert.That(emailMessage,Is.Not.Null);
            Assert.That(emailMessage.ToAddress,Is.EquivalentTo(_messageHelper.ToAddresses));
            Assert.That(emailMessage.BccAddress, Is.EquivalentTo(_messageHelper.BccAddress));
            Assert.That(emailMessage.CCAddress, Is.EquivalentTo(_messageHelper.CcAddresses));
            Assert.That(emailMessage.Attachments,Is.EquivalentTo(_messageHelper.Attachment));
            Assert.AreEqual(emailMessage.FromAddress,_messageHelper.FromAddress);
            Assert.That(emailMessage.Subject, Is.EqualTo(subject));
            Assert.That(emailMessage.Content,Is.EqualTo(body));
        }
    }
}