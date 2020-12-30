using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationSvnController : BaseController
    {
        #region ctor

        readonly IApplicationReaderService applicationReaderService;

        readonly IApplicationSvnService applicationSvnService;

        readonly IApplicationSvnViewModelFactory applicationSvnViewModelFactory;

        readonly IGeneralSettingsService generalSettingsService;

        readonly IUrlGeneratorService urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        public ApplicationSvnController(
            IUserSessionService userSessionService,
            IApplicationReaderService applicationReaderService,
            IApplicationSvnViewModelFactory applicationSvnViewModelFactory,
            IApplicationSvnService applicationSvnService,
            IGeneralSettingsService generalSettingsService,
            IUrlGeneratorService urlHelper,
            IRouteValueFactory routeValueFactory)
            : base(userSessionService)
        {
            this.applicationReaderService = applicationReaderService;
            this.applicationSvnService = applicationSvnService;
            this.applicationSvnViewModelFactory = applicationSvnViewModelFactory;
            this.generalSettingsService = generalSettingsService;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
        }

        #endregion

        static bool ApplicationSvnRepositoryExist(SvnRepository svnRepository)
        {
            return svnRepository != null;
        }

        #region add

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add(int applicationId)
        {
            var application = applicationReaderService.GetApplication(applicationId);

            if (!ApplicationExists(application))
            {
                return ApplicationNotFoundResult();
            }

            var repositoryTypeList = applicationSvnService.GetSvnRepositoryTypes();
            var svnUrl = generalSettingsService.GetSvnUrl().ToString();
            var model = applicationSvnViewModelFactory.CreateApplicationSvnViewModel(applicationId, application.Name, svnUrl, repositoryTypeList);

            return View(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ApplicationSvnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(model);
            }

            SetUserInfoForAdd(model.ApplicationSvn);

            var result = applicationSvnService.AddApplicationSvnRepository(model.ApplicationSvn);

            if (!result.IsSuccess)
            {
                SetErrorResultMessageTempData(result.Message);

                return AddErrorView(model);
            }

            SetSuccessResultMessageTempData(result.Message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = model.ApplicationSvn.ApplicationId });
        }

        IActionResult AddErrorView(ApplicationSvnViewModel model)
        {
            model.SvnRepositoryTypeList = applicationSvnService.GetSvnRepositoryTypes();
            model.ApplicationSvn.SvnUrl = generalSettingsService.GetSvnUrl().ToString();

            return View(ViewNames.Add, model);
        }

        void SetUserInfoForAdd(SvnRepository svnRepository)
        {
            int userId = CurrentUserId;
            svnRepository.RecordUpdateInfo.CreatedBy = userId;
            svnRepository.RecordUpdateInfo.ModifiedBy = userId;
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            var applicationSvnRepository = applicationSvnService.GetApplicationSvnRepository(id);

            if (!ApplicationSvnRepositoryExist(applicationSvnRepository))
            {
                return ApplicationSvnRepositoryNotFoundResult();
            }

            var repositoryTypeList = applicationSvnService.GetSvnRepositoryTypes();
            applicationSvnRepository.SvnUrl = generalSettingsService.GetSvnUrl().ToString();
            var viewModel = applicationSvnViewModelFactory.CreateApplicationSvnEditViewModel(applicationSvnRepository, repositoryTypeList);

            return View(ViewNames.Edit, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationSvnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateErrorView(model);
            }

            SetUserInfoForUpdate(model.ApplicationSvn);

            var updateResult = applicationSvnService.UpdateApplicationSvnRepository(model.ApplicationSvn);

            if (!updateResult.IsSuccess)
            {
                SetErrorResultMessageTempData(updateResult.Message);

                return UpdateErrorView(model);
            }

            return UpdateSuccessView(model.ApplicationSvn.Id, updateResult.Message);
        }

        IActionResult UpdateErrorView(ApplicationSvnViewModel model)
        {
            model.SvnRepositoryTypeList = applicationSvnService.GetSvnRepositoryTypes();
            model.ApplicationSvn.SvnUrl = generalSettingsService.GetSvnUrl().ToString();

            return View(ViewNames.Edit, model);
        }

        IActionResult UpdateSuccessView(int svnRepositoryId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationSvnControllerActionNames.Detail, ControllerNames.ApplicationSvn, new { id = svnRepositoryId });
        }

        void SetUserInfoForUpdate(SvnRepository svnRepository)
        {
            svnRepository.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            var applicationSvnRepository = applicationSvnService.GetApplicationSvnRepository(id);

            if (!ApplicationSvnRepositoryExist(applicationSvnRepository))
            {
                return ApplicationSvnRepositoryNotFoundResult();
            }

            applicationSvnRepository.SvnUrl = generalSettingsService.GetSvnUrl().ToString();
            var viewModel = applicationSvnViewModelFactory.CreateApplicationSvnDetailViewModel(applicationSvnRepository);

            return View(ViewNames.Detail, viewModel);
        }

        #endregion

        #region delete

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]

        //TODO: ValidateAntiforgeryToken eklenmeli.
        public JsonResult Delete(int id, int applicationId)
        {
            var result = applicationSvnService.DeleteApplicationSvnRepository(id);
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

        #region get application svn repositories

        [HttpGet]
        public IActionResult GetSvnRepositories(int applicationId)
        {
            var result = applicationReaderService.GetSvnRepositories(applicationId);

            return Json(ClientDataResult.Success(result));
        }

        #endregion

        IActionResult ApplicationSvnRepositoryNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.ApplicationSvnRepositoryNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }
    }
}