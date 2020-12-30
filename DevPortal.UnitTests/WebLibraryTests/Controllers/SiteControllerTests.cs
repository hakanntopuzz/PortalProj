using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Controllers;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SiteControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<ISettings> settings;

        SiteController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            settings = new StrictMock<ISettings>();

            controller = new SiteController(
                userSessionService.Object,
                settings.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            settings.VerifyAll();
        }

        #endregion

        [Test]
        public void About_NoCondition_ReturnView()
        {
            //Arrange

            // Act
            var result = controller.About();

            // Assert
            result.Should().BeViewResult();
        }

        [Test]
        public void Version_NoCondition_ReturnVersion()
        {
            //Arrange
            var version = "1.2.0.1748";

            settings.SetupGet(x => x.ApplicationVersion).Returns(version);

            // Act
            var result = controller.Version();

            // Assert
            result.Should().BeContentResult().WithContent(version);
        }
    }
}