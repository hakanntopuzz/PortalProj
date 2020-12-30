using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationBuildSettingsController : BaseController
    {
        #region ctor

        private readonly IApplicationReaderService applicationReaderService;
        private readonly IApplicationBuildSettingsViewModelFactory viewModelFactory;
        private readonly IApplicationBuildSettingsService buildSettingsService;

        public ApplicationBuildSettingsController(
            IUserSessionService userSessionService,
            IApplicationReaderService applicationReaderService,
            IApplicationBuildSettingsViewModelFactory viewModelFactory,
            IApplicationBuildSettingsService buildSettingsService) : base(userSessionService)
        {
            this.applicationReaderService = applicationReaderService;
            this.viewModelFactory = viewModelFactory;
            this.buildSettingsService = buildSettingsService;
        }

        #endregion

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public ApplicationBuildSettings GetApplicationBuildSettings(int applicationId)
        {
            return buildSettingsService.GetApplicationBuildSettings(applicationId);
        }

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            var application = applicationReaderService.GetApplication(id);
            if (!ApplicationExists(application))
            {
                return ApplicationNotFoundResult();
            }

            var buildSettings = buildSettingsService.GetApplicationBuildSettings(id);

            var viewModel = viewModelFactory.CreateApplicationBuildSettingsViewModel(buildSettings, application);

            return View(ViewNames.Edit, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationBuildSettingsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Edit, viewModel);
            }

            SetCurrentUserInfo(viewModel);

            var result = buildSettingsService.AddOrUpdateApplicationBuildSettings(viewModel.BuildSettings);
            if (!result.IsSuccess)
            {
                SetErrorResultMessageTempData(result.Message);

                return View(ViewNames.Edit, viewModel);
            }

            SetSuccessResultMessageTempData(result.Message);

            return RedirectToAction(BaseActionNames.Edit, ControllerNames.ApplicationBuildSettings, new { id = viewModel.BuildSettings.ApplicationId });
        }

        void SetCurrentUserInfo(ApplicationBuildSettingsViewModel viewModel)
        {
            viewModel.BuildSettings.RecordUpdateInfo = new RecordUpdateInfo { CreatedBy = CurrentUserId, ModifiedBy = CurrentUserId };
        }
    }
}
