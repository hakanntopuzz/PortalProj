using DevPortal.Framework.Abstract;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.SvnAdmin.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class SvnAdminController : BaseController
    {
        #region ctor

        readonly ISvnAdminService svnAdminService;

        readonly ISvnAdminViewModelFactory viewModelFactory;

        public SvnAdminController(
            IUserSessionService userSessionService,
            ISvnAdminService svnAdminService,
            ISvnAdminViewModelFactory viewModelFactory) : base(userSessionService)
        {
            this.svnAdminService = svnAdminService;
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        #region get repository list

        public IActionResult Index()
        {
            var result = svnAdminService.GetRepositoriesByLastUpdatedOrder();

            if (!result.IsSuccess)
            {
                return View(ViewNames.Error, result.Message);
            }

            var model = viewModelFactory.CreateSvnRepositoryListViewModel(result.Value);

            return View(ViewNames.Index, model);
        }

        #endregion

        #region create svn repository folder

        [HttpGet]
        public IActionResult CreateSvnRepositoryFolder()
        {
            return SvnRepositoryFolderView(null);
        }

        IActionResult SvnRepositoryFolderView(SvnRepositoryFolder folder)
        {
            var model = viewModelFactory.CreateSvnRepositoryFolderViewModel(folder);

            return View(ViewNames.Create, model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSvnRepositoryFolder(SvnRepositoryFolder folder)
        {
            if (!ModelState.IsValid)
            {
                return SvnRepositoryFolderView(folder);
            }

            var result = await svnAdminService.CreateSvnRepositoryFolderAsync(folder);

            if (!result.IsSuccess)
            {
                return SvnRepositoryFolderErrorView(folder, result.Message);
            }

            return RedirectToIndexWithSuccessMessage(result.Message);
        }

        IActionResult RedirectToIndexWithSuccessMessage(string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(SvnAdminControllerActionNames.Index);
        }

        IActionResult SvnRepositoryFolderErrorView(SvnRepositoryFolder folder, string message)
        {
            SetErrorResultMessageTempData(message);

            return SvnRepositoryFolderView(folder);
        }

        #endregion
    }
}