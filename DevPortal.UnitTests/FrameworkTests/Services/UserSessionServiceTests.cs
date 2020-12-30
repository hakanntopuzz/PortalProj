using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Services;
using FluentAssertions;
using NUnit.Framework;
using System.Security.Claims;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserSessionServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IHttpContextWrapper> httpContextWrapper;

        UserSessionService service;

        [SetUp]
        public void Initialize()
        {
            httpContextWrapper = new StrictMock<IHttpContextWrapper>();

            service = new UserSessionService(httpContextWrapper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            httpContextWrapper.VerifyAll();
        }

        #endregion

        #region GetCurrentUserId

        [Test]
        public void GetCurrentUserId_NoCondition_ReturnUserId()
        {
            //Arrange
            var userId = 1;
            var claimsIdentity = new ClaimsIdentity("cookies");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            httpContextWrapper.Setup(s => s.GetCurrentUser()).Returns(claimsPrincipal);

            //Act
            var result = service.GetCurrentUserId();

            //Assert
            result.Should().Be(userId);
        }

        #endregion
    }
}