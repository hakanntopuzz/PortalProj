using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class EmailSenderServiceTests : BaseTestFixture
    {
        #region ctor

        StrictMock<IMailInfoFactory> mailInfoFactory;

        StrictMock<IMailService> mailService;

        EmailSenderService service;

        [SetUp]
        public void Initialize()
        {
            mailInfoFactory = new StrictMock<IMailInfoFactory>();
            mailService = new StrictMock<IMailService>();

            service = new EmailSenderService(
                mailService.Object,
                mailInfoFactory.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            mailInfoFactory.VerifyAll();
            mailService.VerifyAll();
        }

        #endregion

        #region SendForgotPasswordMail

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SendForgotPasswordMail_NoCondition_ReturnBoolean(bool isSendMail)
        {
            // Arrange
            var emailAddress = "dev@activebuilder.com";
            var resetUrl = "resetUrl";

            var mailInfo = new MailInfo();

            mailInfoFactory.Setup(s => s.CreateForgotPasswordMailInfo(emailAddress, resetUrl)).Returns(mailInfo);
            mailService.Setup(s => s.SendMail(mailInfo)).Returns(isSendMail);

            // Act
            var result = service.SendForgotPasswordMail(emailAddress, resetUrl);

            // Assert
            result.Should().Be(isSendMail);
        }

        #endregion
    }
}