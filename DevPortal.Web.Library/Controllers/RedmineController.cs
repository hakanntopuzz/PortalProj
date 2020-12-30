using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class RedmineController : BaseController
    {
        #region ctor

        readonly IApplicationReaderService applicationReaderService;

        readonly IRedmineViewModelFactory redmineViewModelFactory;

        readonly IDatabaseReaderService databaseReaderService;

        readonly IDatabaseGroupService databaseGroupService;

        public RedmineController(
            IUserSessionService userSessionService,
            IApplicationReaderService applicationReaderService,
            IRedmineViewModelFactory redmineViewModelFactory,
            IDatabaseReaderService databaseReaderService,
            IDatabaseGroupService databaseGroupService) : base(userSessionService)
        {
            this.applicationReaderService = applicationReaderService;
            this.redmineViewModelFactory = redmineViewModelFactory;
            this.databaseReaderService = databaseReaderService;
            this.databaseGroupService = databaseGroupService;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var applicationGroups = applicationReaderService.GetApplicationGroups();
            var model = redmineViewModelFactory.CreateApplicationRedmineProjectsViewModel(applicationGroups);

            return View(ViewNames.Index, model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(RedmineTableParam tableParam)
        {
            var projects = await applicationReaderService.GetFilteredApplicationRedmineProjectListAsync(tableParam);
            var model = redmineViewModelFactory.CreateRedmineProjectListModel(projects);

            return Ok(model);
        }

        #endregion

        #region database projects

        public IActionResult DatabaseProjects()
        {
            var databaseGroups = databaseGroupService.GetDatabaseGroups();
            var model = redmineViewModelFactory.CreateDatabaseRedmineProjectsViewModel(databaseGroups);

            return View(ViewNames.DatabaseProjects, model);
        }

        [HttpPost]
        public async Task<IActionResult> DatabaseProjects(DatabaseRedmineProjectTableParam tableParam)
        {
            var projects = await databaseReaderService.GetFilteredDatabaseRedmineProjectListAsync(tableParam);
            var model = redmineViewModelFactory.CreateRedmineProjectListModel(projects);

            return Ok(model);
        }

        #endregion
    }
}