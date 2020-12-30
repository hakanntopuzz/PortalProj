using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ExternalDependencyController : BaseController
    {
        #region ctor

        readonly IApplicationReaderService applicationReaderService;

        readonly IExternalDependencyService externalDependencyService;

        readonly IExternalDependencyViewModelFactory viewModelFactory;

        public ExternalDependencyController(
            IUserSessionService userSessionService,
            IApplicationReaderService applicationReaderService,
            IExternalDependencyService externalDependencyService,
            IExternalDependencyViewModelFactory viewModelFactory)
            : base(userSessionService)
        {
            this.applicationReaderService = applicationReaderService;
            this.externalDependencyService = externalDependencyService;
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return ExternalDependencyNotFoundResult();
            }

            var externalDependency = externalDependencyService.GetExternalDependencyById(id);

            if (ExternalDependencyExists(externalDependency))
            {
                return ExternalDependencyNotFoundResult();
            }

            var model = viewModelFactory.CreateExternalDependencyViewModel(externalDependency);

            return View(ViewNames.Detail, model);
        }

        IActionResult ExternalDependencyNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.ExternalDependencyNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        static bool ExternalDependencyExists(ExternalDependency externalDependent)
        {
            return externalDependent == null;
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

            var model = viewModelFactory.CreateAddExternalDependencyViewModel(application.Name, applicationId);

            return View(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ExternalDependencyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Add, viewModel);
            }

            viewModel.ExternalDependency.RecordUpdateInfo.CreatedBy = CurrentUserId;

            var result = externalDependencyService.AddExternalDependency(viewModel.ExternalDependency);

            if (!result.IsSuccess)
            {
                SetErrorResultMessageTempData(result.Message);

                return View(ViewNames.Add, viewModel);
            }

            SetSuccessResultMessageTempData(result.Message);

            return RedirectToAction(ExternalDependencyControllerActionNames.Detail, ControllerNames.ExternalDependency, new { id = result.Value });
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            return EditView(id);
        }

        IActionResult EditView(int externalDependencyId)
        {
            var externalDependency = externalDependencyService.GetExternalDependencyById(externalDependencyId);

            if (ExternalDependencyExists(externalDependency))
            {
                return DetailErrorView(Messages.ExternalDependencyNotFound);
            }

            var model = viewModelFactory.CreateEditExternalDependencyViewModel(externalDependency);

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
        public IActionResult Edit(ExternalDependencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Edit, model);
            }

            SetUserInfoForUpdate(model.ExternalDependency);

            var serviceResult = externalDependencyService.UpdateExternalDependency(model.ExternalDependency);

            if (serviceResult.IsSuccess)
            {
                SetSuccessResultMessageTempData(serviceResult.Message);

                return UpdateSuccessView(model.ExternalDependency.Id);
            }

            SetErrorResultMessageTempData(serviceResult.Message);

            return EditView(model.ExternalDependency.Id);
        }

        void SetUserInfoForUpdate(ExternalDependency externalDependency)
        {
            externalDependency.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        IActionResult UpdateSuccessView(int externalDependencyId)
        {
            return RedirectToAction(ExternalDependencyControllerActionNames.Detail, ControllerNames.ExternalDependency, new { id = externalDependencyId });
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id)
        {
            var externalDependency = externalDependencyService.GetExternalDependencyById(id);

            SetUserInfoForUpdate(externalDependency);

            var result = externalDependencyService.DeleteExternalDependency(externalDependency);

            if (!result.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(string.Empty, result.Message));
            }

            SetSuccessResultMessageTempData(result.Message);

            return Json(RedirectableClientActionResult.Success(result.Value, result.Message));
        }

        #endregion
    }
}