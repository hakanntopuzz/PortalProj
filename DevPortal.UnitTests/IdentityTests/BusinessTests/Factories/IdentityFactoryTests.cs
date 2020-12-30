using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Security.Claims;

namespace DevPortal.UnitTests.IdentityTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class IdentityFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserAgentService> userAgentService;

        IdentityFactory factory;

        [SetUp]
        public void Initialize()
        {
            userAgentService = new StrictMock<IUserAgentService>();

            factory = new IdentityFactory(userAgentService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            userAgentService.VerifyAll();
        }

        #endregion

        #region CreateUserClaims

        [Test]
        public void CreateUserClaims_NoCondition_ReturnClaimsPrincipal()
        {
            //Arrange
            var user = new User
            {
                EmailAddress = "user1@activebuilder.com",
                UserTypeId = 1,
                SecureId = "45454545",
                FirstName = "User",
                LastName = "1"
            };

            var expectedResult = new ClaimsIdentity("cookies");

            //Act
            var result = factory.CreateUserClaims(user);

            //Assert
            result.Identity.GetType().Should().Be(expectedResult.GetType());
        }

        #endregion

        #region CreateUserLogOnLog

        [Test]
        public void CreateUserLogOnLog_NoCondition_ReturnUserLogOnLog()
        {
            //Arrange

            var emailAddress = "user1@activebuilder.com";
            var ipAddress = "1.1.1.1";
            var userId = 1;

            var userAgent = new UserAgent
            {
                IpAddress = ipAddress
            };

            var expectedResult = new UserLogOnLog();

            userAgentService.Setup(s => s.GetUserAgent()).Returns(userAgent);

            //Act
            var result = factory.CreateUserLogOnLog(emailAddress, userId);

            //Assert
            result.GetType().Should().Be(expectedResult.GetType());
            result.IpAddress.Should().Be(ipAddress);
            result.EmailAddress.Should().Be(emailAddress);
            result.UserId.Should().Be(userId);
        }

        #endregion
    }
}