using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UrlGeneratorServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUrlHelper> urlHelper;

        StrictMock<ISettings> settings;

        StrictMock<IRouteValueFactory> routeValueFactory;

        UrlGeneratorService service;

        [SetUp]
        public void Initialize()
        {
            urlHelper = new StrictMock<IUrlHelper>();
            settings = new StrictMock<ISettings>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            service = new UrlGeneratorService(
                settings.Object,
                urlHelper.Object,
                routeValueFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            urlHelper.VerifyAll();
            settings.VerifyAll();
            routeValueFactory.VerifyAll();
        }

        #endregion

        [Test]
        public void RouteUrlWithRouteNameParam_NoCondition_ReturnUrl()
        {
            // Arrange
            var routeName = "/test-route";
            var url = "/test-url";

            urlHelper.Setup(x => x.RouteUrl(routeName)).Returns(url);

            // Act
            var result = service.RouteUrl(routeName);

            // Assert
            result.Should().Be(url);
        }

        [Test]
        public void RouteUrlWithRouteValuesParam_NoCondition_ReturnUrl()
        {
            // Arrange
            var routeName = "/test-route";
            var routeValues = new RouteValueDictionary();
            var url = "/test-url";

            urlHelper.Setup(x => x.RouteUrl(routeName, routeValues)).Returns(url);

            // Act
            var result = service.RouteUrl(routeName, routeValues);

            // Assert
            result.Should().Be(url);
        }

        [Test]
        public void GenerateUrl_NoCondition_ReturnUrl()
        {
            // Arrange
            const string action = "test-action";
            const string controller = "test-controller";
            const int id = 1;
            var routeValues = new RouteValueDictionary();
            const string siteUrl = "site-url";
            const string relativeUrl = "url";
            var url = $"{siteUrl}{relativeUrl}";

            settings.Setup(x => x.SiteUrl).Returns(siteUrl);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(id)).Returns(routeValues);
            urlHelper.Setup(x => x.Action(action, controller, routeValues)).Returns(relativeUrl);

            // Act
            var result = service.GenerateUrl(action, controller, id);

            // Assert
            result.Should().Be(url);
        }

        [Test]
        public void GenerateUrl_NoCondition_ReturnStringUrl()
        {
            // Arrange
            const string action = "test-action";
            const string controller = "test-controller";

            const string siteUrl = "site-url";
            const string relativeUrl = "url";
            var url = $"{siteUrl}{relativeUrl}";

            urlHelper.Setup(x => x.Action(action, controller)).Returns(relativeUrl);
            settings.Setup(x => x.SiteUrl).Returns(siteUrl);

            // Act
            var result = service.GenerateUrl(action, controller);

            // Assert
            result.Should().Be(url);
        }

        [Test]
        public void GenerateUrlWithRouteValueDictionary_NoCondition_ReturnUrl()
        {
            // Arrange
            const string action = "test-action";
            const string controller = "test-controller";
            var routeValues = new RouteValueDictionary();
            const string siteUrl = "site-url";
            const string relativeUrl = "url";
            var url = $"{siteUrl}{relativeUrl}";

            settings.Setup(x => x.SiteUrl).Returns(siteUrl);
            urlHelper.Setup(x => x.Action(action, controller, routeValues)).Returns(relativeUrl);

            // Act
            var result = service.GenerateUrl(action, controller, routeValues);

            // Assert
            result.Should().Be(url);
        }
    }
}