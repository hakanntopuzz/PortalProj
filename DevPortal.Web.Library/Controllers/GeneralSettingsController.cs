using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    [PolicyBasedAuthorize(Policy.Admin)]
    public class GeneralSettingsController : BaseController
    {
        #region ctor

        readonly IGeneralSettingsService generalSettingsService;

        readonly IGeneralSettingsViewModelFactory viewModelFactory;

        public GeneralSettingsController(
            IUserSessionService userSessionService,
            IGeneralSettingsService generalSettingsService,
            IGeneralSettingsViewModelFactory viewModelFactory) : base(userSessionService)
        {
            this.generalSettingsService = generalSettingsService;
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        public IActionResult Index()
        {
            return Edit();
        }

        public IActionResult Edit()
        {
            var generalSettings = generalSettingsService.GetGeneralSettings();

            return GeneralSettingsView(generalSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GeneralSettings generalSettings)
        {
            if (!ModelState.IsValid)
            {
                SetErrorResultMessageTempData(Messages.UpdateFails);

                return GeneralSettingsView(generalSettings);
            }

            SetUserInfoForUpdate(generalSettings);

            var updateResult = generalSettingsService.UpdateGeneralSettings(generalSettings);

            if (!updateResult.IsSuccess)
            {
                SetErrorResultMessageTempData(updateResult.Message);

                return GeneralSettingsView(generalSettings);
            }

            SetSuccessResultMessageTempData(updateResult.Message);

            return RedirectToAction(GeneralSettingsControllerActionNames.Edit, ControllerNames.GeneralSettings);
        }

        void SetUserInfoForUpdate(GeneralSettings generalSettings)
        {
            generalSettings.ModifiedBy = CurrentUserId;
        }

        IActionResult GeneralSettingsView(GeneralSettings generalSettings)
        {
            var viewModel = viewModelFactory.CreateGeneralSettingsViewModel(generalSettings);

            return View(ViewNames.Edit, viewModel);
        }
    }
}