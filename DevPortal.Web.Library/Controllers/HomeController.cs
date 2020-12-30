using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region ctor

        readonly IApplicationReportService applicationReportService;

        readonly IApplicationReaderService applicationReaderService;

        readonly ISvnAdminService svnAdminService;

        readonly IGeneralSettingsService generalSettingsService;

        readonly IShortcutService shortcutService;

        readonly IJenkinsService jenkinsService;

        readonly INugetService nugetService;

        readonly IFavouritePageService favouritePageService;

        readonly ISettings settings;

        public HomeController(
            IUserSessionService userSessionService,
            IApplicationReportService applicationReportService,
            IApplicationReaderService applicationReaderService,
            ISvnAdminService svnAdminService,
            IGeneralSettingsService generalSettingsService,
            IShortcutService shortcutService,
            IJenkinsService jenkinsService,
            INugetService nugetService,
            IFavouritePageService favouritePageService,
            ISettings settings) : base(userSessionService)
        {
            this.applicationReportService = applicationReportService;
            this.applicationReaderService = applicationReaderService;
            this.svnAdminService = svnAdminService;
            this.generalSettingsService = generalSettingsService;
            this.shortcutService = shortcutService;
            this.jenkinsService = jenkinsService;
            this.nugetService = nugetService;
            this.favouritePageService = favouritePageService;
            this.settings = settings;
        }

        #endregion

        public IActionResult Index()
        {
            var favouritePagesHomePageCount = settings.FavouritePagesHomePageCount;

            var model = new HomeViewModel
            {
                ApplicationStats = applicationReportService.GetApplicationStats(),
                LastUpdatedApplications = applicationReaderService.GetLastUpdatedApplications(),
                LastUpdatedSvnRepositories = svnAdminService.GetLastUpdatedRepositories(),
                SvnRepositoryCount = svnAdminService.GetRepositoryCount(),
                JenkinsUrl = generalSettingsService.GetJenkinsUrl().ToString(),
                SonarQubeUrl = generalSettingsService.GetSonarQubeUrl().ToString(),
                RedmineUrl = generalSettingsService.GetRedmineUrl().ToString(),
                RedmineProjects = shortcutService.GetFavouriteRedmineProjects(),
                RedmineWikiPages = shortcutService.GetFavouriteRedmineWikiPages(),
                ToolPages = shortcutService.GetToolPages(),
                JenkinsFailedJobs = jenkinsService.GetFailedJobs(),
                LastUpdatedNugetPackages = nugetService.GetLastUpdatedNugetPackages(),
                NugetPackagesStats = nugetService.GetNugetPackagesStats(),
                FavouritePages = favouritePageService.GetFavouritePageLinksByCount(CurrentUserId, favouritePagesHomePageCount)
            };

            return View(model);
        }
    }
}