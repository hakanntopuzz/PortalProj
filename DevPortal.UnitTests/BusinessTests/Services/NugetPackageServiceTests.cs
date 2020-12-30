using AB.Framework.UnitTests;
using DevPortal.Business.Services;
using DevPortal.Framework.Abstract;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetPackageServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ISettings> settings;

        NugetPackageService service;

        [SetUp]
        public void Initialize()
        {
            settings = new StrictMock<ISettings>();

            service = new NugetPackageService(settings.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            settings.VerifyAll();
        }

        #endregion

        #region Get Nuget Package Root Url

        [Test]
        public void GetNugetPackageRootUrl_NoCondition_ReturnString()
        {
            // Arrange
            var siteUrl = "http://www.example.com/";

            settings.Setup(x => x.SiteUrl).Returns(siteUrl);

            // Act
            var result = service.GetNugetPackageRootUrl();

            // Assert
            result.Should().Be(siteUrl + "/nuget/packages/");
        }

        #endregion

        #region get filtered jq table

        [Test]
        public void GetNugetPackageUrl_NoCondition_ReturnString()
        {
            // Arrange
            var siteUrl = "http://www.example.com";
            var rootUrl = siteUrl + "/nuget/packages/";
            string packageName = "packageName";
            var url = $"{rootUrl}{packageName}";
            settings.Setup(x => x.SiteUrl).Returns(siteUrl);

            // Act
            var result = service.GetNugetPackageUrl(packageName);

            // Assert
            result.Should().Be(url);
        }

        #endregion
    }
}