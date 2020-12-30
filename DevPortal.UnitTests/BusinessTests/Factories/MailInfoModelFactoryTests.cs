using AB.Framework.UnitTests;
using DevPortal.Business.Factories;
using DevPortal.Framework.Model;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MailInfoModelFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        MailInfoModelFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new MailInfoModelFactory();
        }

        #endregion

        #region create mail info

        [Test]
        public void CreateMailInfo_NoCondition_ReturnMailInfo()
        {
            // Arrange
            string subject = "subject";
            string recipient = "recipient";
            string mailBody = "mailBody";

            var expectedResult = new MailInfo
            {
                Recipient = recipient,
                Subject = subject,
                MailBody = mailBody
            };

            // Act
            var result = factory.CreateMailInfo(recipient, subject, mailBody);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region create password reset model

        [Test]
        public void CreatePasswordResetModel_NoCondition_ReturnPasswordResetModel()
        {
            // Arrange
            string resetUrl = "resetUrl";

            var passwordResetModel = new PasswordResetModel
            {
                ResetUrl = resetUrl
            };

            // Act
            var result = factory.CreatePasswordResetModel(resetUrl);

            // Assert
            result.Should().BeEquivalentTo(passwordResetModel);
            result.ResetUrl.Should().Be(resetUrl);
            result.Should().NotBeNull();
        }

        #endregion
    }
}