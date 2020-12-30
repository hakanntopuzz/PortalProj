using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.SvnAdmin.Model;
using DevPortal.Web.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class HomeControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationReportService> applicationReportService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<ISvnAdminService> svnAdminService;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IShortcutService> shortcutService;

        StrictMock<IJenkinsService> jenkinsService;

        StrictMock<INugetService> nugetService;

        StrictMock<IFavouritePageService> favouritePageService;

        StrictMock<ISettings> settings;

        HomeController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationReportService = new StrictMock<IApplicationReportService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            svnAdminService = new StrictMock<ISvnAdminService>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            shortcutService = new StrictMock<IShortcutService>();
            jenkinsService = new StrictMock<IJenkinsService>();
            nugetService = new StrictMock<INugetService>();
            favouritePageService = new StrictMock<IFavouritePageService>();
            settings = new StrictMock<ISettings>();

            controller = new HomeController(
                userSessionService.Object,
                applicationReportService.Object,
                applicationReaderService.Object,
                svnAdminService.Object,
                generalSettingsService.Object,
                shortcutService.Object,
                jenkinsService.Object,
                nugetService.Object,
                favouritePageService.Object,
                settings.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationReportService.VerifyAll();
            applicationReaderService.VerifyAll();
            svnAdminService.VerifyAll();
            generalSettingsService.VerifyAll();
            shortcutService.VerifyAll();
            jenkinsService.VerifyAll();
            nugetService.VerifyAll();
            favouritePageService.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        [Test]
        public void Index_NoCondition_ReturnView()
        {
            //Arrange
            var applicationStats = new ApplicationStats
            {
                ApplicationCount = 75
            };

            var userId = 1;
            var favouritePagesHomePageCount = 10;

            var lastUpdatedApplications = new List<ApplicationListItem>();
            var svnReposResult = SvnRepositoryListResult.Success(new List<SvnRepositoryFolderListItem>());
            var svnRepoCount = 92;
            var jenkinsUrl = new Uri("http://www.example.com/jenkins/jenkins-url");
            var sonarqubeUrl = new Uri("http://wwww.example.com/sonarqube-url");
            var redmineUrl = new Uri("http://wwww.example.com/redmine-url");
            var redmineProjects = new List<Link>();
            var redmineWikiPages = new List<Link>();
            var toolPages = new List<Link>();
            var jenkinsFailedJobs = new List<JenkinsJobItem>();
            var lastUpdatedNugetPackages = new List<LastUpdatedNugetPackageListItem>();
            var nugetPackagesStats = new NugetPackagesStats();
            var favouritePages = new List<Link>();

            settings.Setup(x => x.FavouritePagesHomePageCount).Returns(favouritePagesHomePageCount);
            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(userId);
            applicationReportService.Setup(x => x.GetApplicationStats()).Returns(applicationStats);
            applicationReaderService.Setup(x => x.GetLastUpdatedApplications()).Returns(lastUpdatedApplications);
            svnAdminService.Setup(x => x.GetLastUpdatedRepositories()).Returns(svnReposResult);
            svnAdminService.Setup(x => x.GetRepositoryCount()).Returns(svnRepoCount);
            generalSettingsService.Setup(x => x.GetJenkinsUrl()).Returns(jenkinsUrl);
            generalSettingsService.Setup(x => x.GetSonarQubeUrl()).Returns(sonarqubeUrl);
            generalSettingsService.Setup(x => x.GetRedmineUrl()).Returns(redmineUrl);
            shortcutService.Setup(x => x.GetFavouriteRedmineProjects()).Returns(redmineProjects);
            shortcutService.Setup(x => x.GetFavouriteRedmineWikiPages()).Returns(redmineWikiPages);
            shortcutService.Setup(x => x.GetToolPages()).Returns(toolPages);
            jenkinsService.Setup(x => x.GetFailedJobs()).Returns(jenkinsFailedJobs);
            nugetService.Setup(x => x.GetLastUpdatedNugetPackages()).Returns(lastUpdatedNugetPackages);
            nugetService.Setup(x => x.GetNugetPackagesStats()).Returns(nugetPackagesStats);
            favouritePageService.Setup(s => s.GetFavouritePageLinksByCount(userId, favouritePagesHomePageCount)).Returns(favouritePages);

            // Act
            var result = controller.Index();

            // Assert
            var resultModel = result.Should().BeViewResult().ModelAs<HomeViewModel>();
            resultModel.ApplicationStats.Should().Be(applicationStats);
            resultModel.LastUpdatedApplications.Should().BeSameAs(lastUpdatedApplications);
            resultModel.LastUpdatedSvnRepositories.Should().BeSameAs(svnReposResult);
            resultModel.SvnRepositoryCount.Should().Be(svnRepoCount);
            resultModel.JenkinsUrl.Should().Be(jenkinsUrl.ToString());
            resultModel.SonarQubeUrl.Should().Be(sonarqubeUrl.ToString());
            resultModel.RedmineUrl.Should().Be(redmineUrl.ToString());
            resultModel.RedmineProjects.Should().BeSameAs(redmineProjects);
            resultModel.RedmineWikiPages.Should().BeSameAs(redmineWikiPages);
            resultModel.JenkinsFailedJobs.Should().BeSameAs(jenkinsFailedJobs);
            resultModel.LastUpdatedNugetPackages.Should().BeSameAs(lastUpdatedNugetPackages);
            resultModel.NugetPackagesStats.Should().Be(nugetPackagesStats);
            resultModel.FavouritePages.Should().BeSameAs(favouritePages);
        }
    }
}