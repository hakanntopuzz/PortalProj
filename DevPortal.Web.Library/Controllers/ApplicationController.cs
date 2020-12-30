using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationController : BaseController
    {
        #region ctor

        readonly IApplicationWriterService applicationWriterService;

        readonly IApplicationReaderService applicationReaderService;

        readonly IApplicationViewModelFactory viewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        public ApplicationController(
            IUserSessionService userSessionService,
            IApplicationWriterService applicationWriterService,
            IApplicationReaderService applicationReaderService,
            IApplicationViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper) : base(userSessionService)
        {
            this.applicationWriterService = applicationWriterService;
            this.applicationReaderService = applicationReaderService;
            this.viewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
        }

        #endregion

        #region get applications

        public IActionResult Index()
        {
            var applicationGroups = applicationReaderService.GetApplicationGroups();
            var applications = applicationReaderService.GetApplications();

            var viewModel = viewModelFactory.CreateApplicationsViewModel(applicationGroups, applications);

            return View(ViewNames.Index, viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApplicationTableParam tableParam)
        {
            var applicationList = await applicationReaderService.GetFilteredApplicationListAsync(tableParam);
            var applicationListModel = viewModelFactory.CreateApplicationListModel(applicationList);

            return Ok(applicationListModel);
        }

        #endregion

        #region application detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return ApplicationNotFoundResult();
            }

            var application = applicationReaderService.GetApplication(id);

            if (!ApplicationExists(application))
            {
                return ApplicationNotFoundResult();
            }

            var model = viewModelFactory.CreateApplication(application);

            return View(ViewNames.Detail, model);
        }

        #endregion

        #region edit application

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return ApplicationNotFoundResult();
            }

            var application = applicationReaderService.GetApplication(id);

            if (!ApplicationExists(application))
            {
                return ApplicationNotFoundResult();
            }

            return EditView(application);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditView(model.Application);
            }

            SetUserInfoForUpdate(model.Application);

            var updateResult = applicationWriterService.UpdateApplication(model.Application);

            if (!updateResult.IsSuccess)
            {
                SetErrorResultMessageTempData(updateResult.Message);

                return EditView(model.Application);
            }

            SetSuccessResultMessageTempData(updateResult.Message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, new { id = model.Application.Id });
        }

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        IActionResult EditView(Application application)
        {
            var applicationGroups = applicationReaderService.GetApplicationGroups();
            var applicationTypes = applicationReaderService.GetApplicationTypes();
            var applicationStatusList = applicationReaderService.GetApplicationStatusList();
            var model = viewModelFactory.CreateEditApplication(application, applicationGroups, applicationTypes, applicationStatusList);

            return View(ViewNames.Edit, model);
        }

        void SetUserInfoForUpdate(Application application)
        {
            application.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        #endregion

        #region add application

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add()
        {
            var model = CreateAddViewModel();

            return View(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Application application)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(application);
            }

            SetUserInfoForAdd(application);

            var addResult = applicationWriterService.AddApplication(application);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(application);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView(addResult.Value);
        }

        IActionResult AddErrorView(Application application)
        {
            var model = CreateAddViewModel();
            model.Application = application;

            return View(ViewNames.Add, model);
        }

        AddApplicationViewModel CreateAddViewModel()
        {
            var applicationGroup = applicationReaderService.GetApplicationGroups();
            var applicationTypes = applicationReaderService.GetApplicationTypes();
            var applicationStatus = applicationReaderService.GetApplicationStatusList();

            return viewModelFactory.CreateApplicationAddViewModel(applicationGroup, applicationTypes, applicationStatus);
        }

        void SetUserInfoForAdd(Application application)
        {
            int userId = CurrentUserId;
            application.RecordUpdateInfo.CreatedBy = userId;
            application.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView(int applicationId)
        {
            return RedirectToAction(ApplicationControllerActionNames.Detail, new { id = applicationId });
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id)
        {
            var result = applicationWriterService.DeleteApplication(id);
            var redirectUrl = urlHelper.GenerateUrl(ApplicationControllerActionNames.Index, ControllerNames.Application);

            if (!result.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl, result.Message));
            }

            SetSuccessResultMessageTempData(result.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl, result.Message));
        }

        #endregion
    }
}