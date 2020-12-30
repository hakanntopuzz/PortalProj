using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;
using DevPortal.Framework.Services;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MailServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ISmtpWrapper> wrapper;

        StrictMock<ISettings> settings;

        StrictMock<IMailMessageFactory> factory;

        MailService service;

        [SetUp]
        public void Initialize()
        {
            wrapper = new StrictMock<ISmtpWrapper>();
            settings = new StrictMock<ISettings>();
            factory = new StrictMock<IMailMessageFactory>();
            service = new MailService(
                wrapper.Object,
                settings.Object,
                factory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            wrapper.VerifyAll();
            settings.VerifyAll();
            factory.VerifyAll();
        }

        #endregion

        [Test]
        public void SendMail_NoCondition_ReturnFalse()
        {
            // Arrange
            var mailInfo = new MailInfo();
            string senderEmailAddress = "senderEmailAddress";
            string senderName = "senderName";
            var mailMessage = new System.Net.Mail.MailMessage();
            settings.Setup(x => x.SenderEmailAddress).Returns(senderEmailAddress);
            settings.Setup(x => x.DefaultMailSenderName).Returns(senderName);
            factory.Setup(x => x.CreateMailMessage(senderEmailAddress, senderName, mailInfo.Subject, mailInfo.Recipient, mailInfo.MailBody)).Returns(mailMessage);
            wrapper.Setup(x => x.SendMail(mailMessage)).Returns(false);

            // Act
            var result = service.SendMail(mailInfo);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void SendMail_NoCondition_ReturnTrue()
        {
            // Arrange
            var mailInfo = new MailInfo();
            string senderEmailAddress = "senderEmailAddress";
            string senderName = "senderName";
            var mailMessage = new System.Net.Mail.MailMessage();
            settings.Setup(x => x.SenderEmailAddress).Returns(senderEmailAddress);
            settings.Setup(x => x.DefaultMailSenderName).Returns(senderName);
            factory.Setup(x => x.CreateMailMessage(senderEmailAddress, senderName, mailInfo.Subject, mailInfo.Recipient, mailInfo.MailBody)).Returns(mailMessage);
            wrapper.Setup(x => x.SendMail(mailMessage)).Returns(true);

            // Act
            var result = service.SendMail(mailInfo);

            // Assert
            result.Should().BeTrue();
        }
    }
}