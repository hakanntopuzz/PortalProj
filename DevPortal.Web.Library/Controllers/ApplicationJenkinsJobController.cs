using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationJenkinsJobController : BaseController
    {
        #region ctor

        readonly IApplicationJenkinsJobService applicationJenkinsJobService;

        readonly IApplicationReaderService applicationReaderService;

        readonly IGeneralSettingsService generalSettingsService;

        readonly IApplicationJenkinsJobViewModelFactory viewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        public ApplicationJenkinsJobController(
            IUserSessionService userSessionService,
            IApplicationJenkinsJobService applicationJenkinsJobService,
            IApplicationReaderService applicationReaderService,
            IGeneralSettingsService generalSettingsService,
            IApplicationJenkinsJobViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper,
            IRouteValueFactory routeValueFactory)
            : base(userSessionService)
        {
            this.applicationJenkinsJobService = applicationJenkinsJobService;
            this.applicationReaderService = applicationReaderService;
            this.generalSettingsService = generalSettingsService;
            this.viewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
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

            var jenkinsJobTypeList = applicationJenkinsJobService.GetJenkinsJobTypes();
            var jenkinsJobUrl = generalSettingsService.GetJenkinsJobUrl().ToString();
            var viewModel = viewModelFactory.CreateApplicationJenkinsJobViewModel(application, jenkinsJobTypeList, jenkinsJobUrl);

            return View(ViewNames.Add, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ApplicationJenkinsJobViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Add, viewModel);
            }

            SetUserInfoForAdd(viewModel.ApplicationJenkinsJob);

            var addResult = applicationJenkinsJobService.AddApplicationJenkinsJob(viewModel.ApplicationJenkinsJob);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return View(ViewNames.Add, viewModel);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = viewModel.ApplicationJenkinsJob.ApplicationId });
        }

        void SetUserInfoForAdd(JenkinsJob job)
        {
            int userId = CurrentUserId;
            job.RecordUpdateInfo.CreatedBy = userId;
            job.RecordUpdateInfo.ModifiedBy = userId;
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var applicationJenkinsJob = applicationJenkinsJobService.GetApplicationJenkinsJob(id);

            if (!ApplicationJenkinsJobExists(applicationJenkinsJob))
            {
                return DetailErrorView(Messages.ApplicationJenkinsJobNotFound);
            }

            var jenkinsJobTypeList = applicationJenkinsJobService.GetJenkinsJobTypes();
            var jenkinsJobUrl = generalSettingsService.GetJenkinsJobUrl().ToString();
            var application = applicationReaderService.GetApplication(applicationJenkinsJob.ApplicationId);
            var applicationJenkinsJobViewModel = viewModelFactory.CreateDetailApplicationJenkinsJobViewModel(application, applicationJenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            return View(ViewNames.Detail, applicationJenkinsJobViewModel);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            var applicationJenkinsJob = applicationJenkinsJobService.GetApplicationJenkinsJob(id);

            if (!ApplicationJenkinsJobExists(applicationJenkinsJob))
            {
                return DetailErrorView(Messages.ApplicationJenkinsJobNotFound);
            }

            var jenkinsJobTypeList = applicationJenkinsJobService.GetJenkinsJobTypes();
            var jenkinsJobUrl = generalSettingsService.GetJenkinsJobUrl().ToString();
            var application = applicationReaderService.GetApplication(applicationJenkinsJob.ApplicationId);
            var viewModel = viewModelFactory.CreateEditApplicationJenkinsJobViewModel(application, applicationJenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            return View(ViewNames.Edit, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationJenkinsJobViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelErrorView(model);
            }

            SetUserInfoForUpdate(model.ApplicationJenkinsJob);

            var updateResult = applicationJenkinsJobService.UpdateApplicationJenkinsJob(model.ApplicationJenkinsJob);

            if (!updateResult.IsSuccess)
            {
                return UpdateErrorView(model, updateResult.Message);
            }

            return UpdateSuccessView(model.ApplicationJenkinsJob.JenkinsJobId, updateResult.Message);
        }

        void SetUserInfoForUpdate(JenkinsJob job)
        {
            job.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = applicationJenkinsJobService.DeleteApplicationJenkinsJob(id);

            var urlParams = routeValueFactory.CreateRouteValuesForGenerateUrl(applicationId);
            var redirectUrl = urlHelper.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, urlParams);

            if (!result.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl));
            }

            SetSuccessResultMessageTempData(result.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl));
        }

        #endregion

        IActionResult UpdateModelErrorView(ApplicationJenkinsJobViewModel model)
        {
            return CreateApplicationJenkinsJobEditView(ViewNames.Edit, model);
        }

        IActionResult UpdateErrorView(ApplicationJenkinsJobViewModel model, string message)
        {
            SetErrorResultMessageTempData(message);

            return CreateApplicationJenkinsJobEditView(ViewNames.Edit, model);
        }

        ActionResult CreateApplicationJenkinsJobEditView(string viewNames, ApplicationJenkinsJobViewModel model = null)
        {
            var jenkinsJobTypeList = applicationJenkinsJobService.GetJenkinsJobTypes();
            var jenkinsJobUrl = generalSettingsService.GetJenkinsJobUrl().ToString();
            var application = applicationReaderService.GetApplication(model.ApplicationJenkinsJob.ApplicationId);
            var viewModel = viewModelFactory.CreateEditApplicationJenkinsJobViewModel(application, model.ApplicationJenkinsJob, jenkinsJobTypeList, jenkinsJobUrl);

            return View(viewNames, viewModel);
        }

        IActionResult UpdateSuccessView(int jenkinsJobId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationJenkinsJobControllerActionNames.Detail, ControllerNames.ApplicationJenkinsJob, new { id = jenkinsJobId });
        }

        static bool ApplicationJenkinsJobExists(JenkinsJob applicationJenkinsJob)
        {
            return applicationJenkinsJob != null;
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        [HttpGet]
        public IActionResult GetJenkinsJobs(int applicationId)
        {
            var result = applicationReaderService.GetJenkinsJobs(applicationId);

            return Json(ClientDataResult.Success(result));
        }
    }
}