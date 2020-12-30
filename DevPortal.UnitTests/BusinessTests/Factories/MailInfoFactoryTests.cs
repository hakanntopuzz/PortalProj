using AB.Framework.TextTemplates.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MailInfoFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ISettings> settings;

        StrictMock<ITextTemplateService> textTemplateService;

        StrictMock<IMailInfoModelFactory> mailInfoModelFactory;

        MailInfoFactory factory;

        [SetUp]
        public void Initialize()
        {
            settings = new StrictMock<ISettings>();
            textTemplateService = new StrictMock<ITextTemplateService>();
            mailInfoModelFactory = new StrictMock<IMailInfoModelFactory>();

            factory = new MailInfoFactory(
                settings.Object,
                textTemplateService.Object,
                mailInfoModelFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            settings.VerifyAll();
            textTemplateService.VerifyAll();
            mailInfoModelFactory.VerifyAll();
        }

        #endregion

        #region CreateForgotPasswordMailInfo

        [Test]
        public void CreateForgotPasswordMailInfo_NoCondition_ReturnMailInfo()
        {
            // Arrange
            var emailAddress = "dev@activebuilder.com";
            var resetUrl = "resetUrl";
            var mailBody = "mailBody";
            var templateFileName = "Root-Path\\PasswordReset.hbs";
            var subject = Messages.ForgotPasswordMailSubject;

            var passwordResetModel = new PasswordResetModel();

            var mailInfo = new MailInfo
            {
                MailBody = mailBody,
                Recipient = emailAddress,
                Subject = subject
            };

            const string mailTemplateRootPath = "Root-Path";
            settings.SetupGet(x => x.MailTemplateRootPath).Returns(mailTemplateRootPath);
            mailInfoModelFactory.Setup(s => s.CreatePasswordResetModel(resetUrl)).Returns(passwordResetModel);
            textTemplateService.Setup(s => s.GetTemplateContent(templateFileName, passwordResetModel)).Returns(mailBody);

            mailInfoModelFactory.Setup(s => s.CreateMailInfo(emailAddress, subject, mailBody)).Returns(mailInfo);

            // Act
            var result = factory.CreateForgotPasswordMailInfo(emailAddress, resetUrl);

            // Assert
            result.Should().NotBeNull();
            result.Recipient.Should().Be(emailAddress);
            result.Subject.Should().Be(subject);
            result.MailBody.Should().Be(mailBody);
        }

        #endregion
    }
}