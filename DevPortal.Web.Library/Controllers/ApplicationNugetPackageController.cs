using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationNugetPackageController : BaseController
    {
        #region ctor

        readonly IApplicationNugetPackageService applicationNugetPackageService;

        readonly IApplicationReaderService applicationReaderService;

        readonly IApplicationNugetPackageViewModelFactory viewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        readonly INugetPackageService nugetPackageService;

        public ApplicationNugetPackageController(
            IUserSessionService userSessionService,
            IApplicationNugetPackageService applicationNugetPackageService,
            IApplicationReaderService applicationReaderService,
            IApplicationNugetPackageViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper,
            IRouteValueFactory routeValueFactory,
            INugetPackageService nugetPackageService) : base(userSessionService)
        {
            this.applicationNugetPackageService = applicationNugetPackageService;
            this.applicationReaderService = applicationReaderService;
            this.viewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
            this.nugetPackageService = nugetPackageService;
        }

        #endregion

        #region index

        public IActionResult Index(int applicationId)
        {
            var packages = applicationNugetPackageService.GetNugetPackages(applicationId);

            return Json(ClientDataResult.Success(packages));
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var applicationNugetPackage = applicationNugetPackageService.GetApplicationNugetPackage(id);

            if (ApplicationNugetPackageExists(applicationNugetPackage))
            {
                return DetailErrorView(Messages.ApplicationNugetPackageNotFound);
            }

            var nugetPackageRootUrl = nugetPackageService.GetNugetPackageRootUrl();
            var application = applicationReaderService.GetApplication(applicationNugetPackage.ApplicationId);
            var applicationNugetPackageViewModel = viewModelFactory.CreateDetailApplicationNugetPackageViewModel(application, applicationNugetPackage, nugetPackageRootUrl.ToString());

            return View(ViewNames.Detail, applicationNugetPackageViewModel);
        }

        #endregion

        #region Edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            var applicationNugetPackage = applicationNugetPackageService.GetApplicationNugetPackage(id);

            if (ApplicationNugetPackageExists(applicationNugetPackage))
            {
                return DetailErrorView(Messages.ApplicationNugetPackageNotFound);
            }

            return CreateApplicationNugetPackageEditView(applicationNugetPackage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(ApplicationNugetPackageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Edit, model);
            }

            model.ApplicationNugetPackage.RecordUpdateInfo.ModifiedBy = CurrentUserId;

            var result = applicationNugetPackageService.UpdateApplicationNugetPackage(model.ApplicationNugetPackage);

            if (result.IsSuccess)
            {
                return UpdateSuccessView(model.ApplicationNugetPackage.NugetPackageId, result.Message);
            }

            return UpdateErrorView(model, result.Message);
        }

        IActionResult UpdateSuccessView(int nugetPackageId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationNugetPackageControllerActionNames.Detail, ControllerNames.ApplicationNugetPackage, new { id = nugetPackageId });
        }

        IActionResult UpdateErrorView(ApplicationNugetPackageViewModel model, string message)
        {
            SetErrorResultMessageTempData(message);

            return CreateApplicationNugetPackageEditView(model.ApplicationNugetPackage);
        }

        #endregion

        #region add

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add(int applicationId)
        {
            var application = applicationReaderService.GetApplication(applicationId);

            if (!ApplicationExists(application))
            {
                return ApplicationNotFoundResult();
            }

            var viewModel = viewModelFactory.CreateApplicationNugetPackageViewModel(application);

            return View(ViewNames.Add, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ApplicationNugetPackageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Add, viewModel);
            }

            SetUserInfo(viewModel);

            var addResult = applicationNugetPackageService.AddApplicationNugetPackage(viewModel.ApplicationNugetPackage);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return View(ViewNames.Add, viewModel);
            }

            return AddSuccessView(viewModel.ApplicationNugetPackage.ApplicationId, addResult.Message);
        }

        void SetUserInfo(ApplicationNugetPackageViewModel viewModel)
        {
            var userId = CurrentUserId;
            viewModel.ApplicationNugetPackage.RecordUpdateInfo.CreatedBy = userId;
            viewModel.ApplicationNugetPackage.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView(int applicationId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = applicationId });
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = applicationNugetPackageService.DeleteApplicationNugetPackage(id);
            var urlParams = routeValueFactory.CreateRouteValuesForGenerateUrl(applicationId);
            var redirectUrl = urlHelper.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, urlParams);

            if (!result.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl, result.Message));
            }

            SetSuccessResultMessageTempData(result.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl, result.Message));
        }

        #endregion

        #region private methods

        static bool ApplicationNugetPackageExists(ApplicationNugetPackage applicationNugetPackage)
        {
            return applicationNugetPackage == null;
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        ActionResult CreateApplicationNugetPackageEditView(ApplicationNugetPackage applicationNugetPackage = null)
        {
            var nugetPackageRootUrl = nugetPackageService.GetNugetPackageRootUrl();
            var application = applicationReaderService.GetApplication(applicationNugetPackage.ApplicationId);
            var applicationNugetPackageViewModel = viewModelFactory.CreateEditApplicationNugetPackageViewModel(application, applicationNugetPackage, nugetPackageRootUrl.ToString());

            return View(ViewNames.Edit, applicationNugetPackageViewModel);
        }

        #endregion
    }
}