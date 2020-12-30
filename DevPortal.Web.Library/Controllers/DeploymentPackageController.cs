using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class DeploymentPackageController : BaseController
    {
        #region ctor

        readonly IDeploymentPackageService deploymentPackageService;

        readonly IDeploymentPackageViewModelFactory deploymentPackageViewModelFactory;

        public DeploymentPackageController(
            IUserSessionService userSessionService,
            IDeploymentPackageService deploymentPackageService,
            IDeploymentPackageViewModelFactory deploymentPackageViewModelFactory) : base(userSessionService)
        {
            this.deploymentPackageService = deploymentPackageService;
            this.deploymentPackageViewModelFactory = deploymentPackageViewModelFactory;
        }

        #endregion

        #region Index

        public IActionResult Index()
        {
            var model = deploymentPackageViewModelFactory.CreateDeploymentPackageViewModel();

            return View(ViewNames.Index, model);
        }

        #endregion

        #region GetApplicationList

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetApplicationList(int environmentId)
        {
            var applications = deploymentPackageService.GetApplicationsByEnvironmentId(environmentId);

            return Json(ClientDataResult.Success(applications));
        }

        #endregion

        #region Database

        public IActionResult Database()
        {
            var model = deploymentPackageViewModelFactory.CreateDatabaseDeploymentPackageViewModel();

            return View(ViewNames.Database, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetDatabaseDeploymentPackagesList(int environmentId)
        {
            var deploymentPackages = deploymentPackageService.GetDatabaseDeploymentPackages(environmentId);

            return Json(ClientDataResult.Success(deploymentPackages));
        }

        #endregion
    }
}