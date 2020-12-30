using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationDependencyController : BaseController
    {
        #region ctor

        readonly IApplicationReaderService applicationReaderService;

        readonly IApplicationDependencyWriterService applicationDependencyWriterService;

        readonly IApplicationDependencyReaderService applicationDependencyReaderService;

        readonly IApplicationDependencyViewModelFactory viewModelFactory;

        readonly IUrlGeneratorService urlGeneratorService;

        readonly IRouteValueFactory routeValueFactory;

        public ApplicationDependencyController(
            IUserSessionService userSessionService,
            IApplicationReaderService applicationReaderService,
            IApplicationDependencyWriterService applicationDependencyWriterService,
            IApplicationDependencyReaderService applicationDependencyReaderService,
            IApplicationDependencyViewModelFactory viewModelFactory,
            IUrlGeneratorService urlGeneratorService,
            IRouteValueFactory routeValueFactory)
            : base(userSessionService)
        {
            this.applicationReaderService = applicationReaderService;
            this.applicationDependencyWriterService = applicationDependencyWriterService;
            this.applicationDependencyReaderService = applicationDependencyReaderService;
            this.viewModelFactory = viewModelFactory;
            this.urlGeneratorService = urlGeneratorService;
            this.routeValueFactory = routeValueFactory;
        }

        #endregion

        #region add

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add(int applicationId)
        {
            var model = CreateAddViewModel(applicationId);

            return View(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ApplicationDependency applicationDependency)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(applicationDependency);
            }

            SetUserInfoForAdd(applicationDependency);

            var addResult = applicationDependencyWriterService.AddApplicationDependency(applicationDependency);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(applicationDependency);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView(applicationDependency.DependentApplicationId);
        }

        IActionResult AddErrorView(ApplicationDependency applicationDependency)
        {
            var model = CreateAddViewModel(applicationDependency.DependedApplicationId);
            model.ApplicationDependency = applicationDependency;

            return View(ViewNames.Add, model);
        }

        ApplicationDependencyViewModel CreateAddViewModel(int applicationId)
        {
            var applicationLists = applicationReaderService.GetApplications();
            var applicationGroups = applicationReaderService.GetApplicationGroups();

            return viewModelFactory.CreatApplicationDependencyViewModelAddView(applicationId, applicationLists, applicationGroups);
        }

        void SetUserInfoForAdd(ApplicationDependency applicationDependency)
        {
            int userId = CurrentUserId;
            applicationDependency.RecordUpdateInfo.CreatedBy = userId;
            applicationDependency.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView(int applicationId)
        {
            var redirectUrl = urlGeneratorService.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId);

            return Redirect(redirectUrl);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return ApplicationDependencyNotFoundResult();
            }

            var applicationDependency = applicationDependencyReaderService.GetApplicationDependency(id);

            if (ApplicationDependencyExists(applicationDependency))
            {
                return ApplicationDependencyNotFoundResult();
            }

            var model = viewModelFactory.CreateApplicationDependencyViewModel(applicationDependency);

            return View(ViewNames.Detail, model);
        }

        IActionResult ApplicationDependencyNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.ApplicationDependencyNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        static bool ApplicationDependencyExists(ApplicationDependency applicationDependency)
        {
            return applicationDependency == null;
        }

        #endregion

        #region get applications by group id

        [HttpGet]
        public IActionResult GetApplicationsByGroupId(int applicationGroupId)
        {
            var applications = applicationReaderService.GetApplicationsByGroupId(applicationGroupId);

            return Json(applications);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            return EditView(id);
        }

        IActionResult EditView(int applicationDependencyId)
        {
            var applicationDependency = applicationDependencyReaderService.GetApplicationDependency(applicationDependencyId);

            if (ApplicationDependencyExists(applicationDependency))
            {
                return DetailErrorView(Messages.ApplicationDependencyNotFound);
            }

            var model = viewModelFactory.CreateApplicationDependencyEditViewModel(applicationDependency);

            return View(ViewNames.Edit, model);
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(ApplicationDependencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Edit, model);
            }

            SetUserInfoForUpdate(model.ApplicationDependency);

            var serviceResult = applicationDependencyWriterService.UpdateApplicationDependency(model.ApplicationDependency);

            if (serviceResult.IsSuccess)
            {
                SetSuccessResultMessageTempData(serviceResult.Message);

                return UpdateSuccessView(model.ApplicationDependency.Id);
            }

            SetErrorResultMessageTempData(serviceResult.Message);

            return EditView(model.ApplicationDependency.Id);
        }

        void SetUserInfoForUpdate(ApplicationDependency applicationDependency)
        {
            applicationDependency.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        IActionResult UpdateSuccessView(int applicationDependencyId)
        {
            return RedirectToAction(ApplicationDependencyControllerActionNames.Detail, ControllerNames.ApplicationDependency, new { id = applicationDependencyId });
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = applicationDependencyWriterService.DeleteApplicationDependency(id);

            var urlParams = routeValueFactory.CreateRouteValuesForGenerateUrl(applicationId);
            var redirectUrl = urlGeneratorService.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, urlParams);

            if (!result.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl));
            }

            SetSuccessResultMessageTempData(result.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl));
        }

        #endregion
    }
}