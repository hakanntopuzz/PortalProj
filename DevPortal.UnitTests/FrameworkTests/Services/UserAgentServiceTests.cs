using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Services;
using FluentAssertions;
using NUnit.Framework;
using UAParser;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserAgentServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IHttpContextWrapper> wrapper;

        UserAgentService service;

        [SetUp]
        public void Initialize()
        {
            wrapper = new StrictMock<IHttpContextWrapper>();

            service = new UserAgentService(
                wrapper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            wrapper.VerifyAll();
        }

        #endregion

        [Test]
        public void GetUserAgent_NoCondition_ReturnUserAgentModel()
        {
            // Arrange
            string userAgentString = "";
            string ipAddress = "";

            wrapper.Setup(x => x.GetUserAgentString()).Returns(userAgentString);
            wrapper.Setup(x => x.GetUserIpAddress()).Returns(ipAddress);
            var parser = Parser.GetDefault();
            parser.ParseUserAgent(userAgentString);

            // Act
            var result = service.GetUserAgent();

            // Assert
            result.BrowserName.Should().Be("Other");
            result.BrowserVersion.Should().NotBeNull();
        }
    }
}