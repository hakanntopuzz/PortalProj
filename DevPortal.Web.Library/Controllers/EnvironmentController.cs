using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class EnvironmentController : BaseController
    {
        #region ctor

        readonly IEnvironmentService environmentService;

        readonly IEnvironmentViewModelFactory environmentViewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        public EnvironmentController(
            IUserSessionService userSessionService,
            IEnvironmentService environmentService,
            IEnvironmentViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper) : base(userSessionService)
        {
            this.environmentService = environmentService;
            this.environmentViewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var environments = environmentService.GetEnvironments();

            var viewModel = environmentViewModelFactory.CreateEnvironmentsViewModel(environments);

            return View(ViewNames.Index, viewModel);
        }

        #endregion

        #region add environment

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add()
        {
            var model = CreateAddViewModel();

            return View(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Environment environment)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(environment);
            }

            SetUserInfoForAdd(environment);

            var addResult = environmentService.AddEnvironment(environment);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(environment);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView();
        }

        IActionResult AddErrorView(Environment environment)
        {
            var model = CreateAddViewModel();
            model.Environment = environment;

            return View(ViewNames.Add, model);
        }

        void SetUserInfoForAdd(Environment environment)
        {
            int userId = CurrentUserId;
            environment.RecordUpdateInfo.CreatedBy = userId;
            environment.RecordUpdateInfo.ModifiedBy = userId;
        }

        EnvironmentViewModel CreateAddViewModel()
        {
            return environmentViewModelFactory.CreateEnvironmentAddView();
        }

        IActionResult AddSuccessView()
        {
            return RedirectToAction(EnvironmentControllerActionNames.Index, ControllerNames.Environment);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var environment = environmentService.GetEnvironment(id);

            if (EnvironmentExists(environment))
            {
                return DetailErrorView(Messages.EnvironmentNotFound);
            }

            var model = environmentViewModelFactory.CreateEnvironmentDetailViewModel(environment);

            return View(ViewNames.Detail, model);
        }

        static bool EnvironmentExists(Environment environment)
        {
            return environment == null;
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(EnvironmentControllerActionNames.Index, ControllerNames.Environment);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            return EditView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(EnvironmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Edit, model);
            }

            SetUserInfoForUpdate(model.Environment);

            var serviceResult = environmentService.UpdateEnvironment(model.Environment);

            if (serviceResult.IsSuccess)
            {
                SetSuccessResultMessageTempData(serviceResult.Message);

                return UpdateSuccessView(model.Environment.Id);
            }

            SetErrorResultMessageTempData(serviceResult.Message);

            return EditView(model.Environment.Id);
        }

        IActionResult EditView(int environmentId)
        {
            var environment = environmentService.GetEnvironment(environmentId);

            if (EnvironmentExists(environment))
            {
                return DetailErrorView(Messages.EnvironmentNotFound);
            }

            var model = environmentViewModelFactory.CreateEnvironmentEditViewModel(environment);

            return View(ViewNames.Edit, model);
        }

        #endregion

        void SetUserInfoForUpdate(Environment environment)
        {
            environment.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        IActionResult UpdateSuccessView(int environmentId)
        {
            return RedirectToAction(EnvironmentControllerActionNames.Detail, ControllerNames.Environment, new { id = environmentId });
        }

        #region delete

        public JsonResult Delete(int id)
        {
            var result = environmentService.DeleteEnvironment(id);
            var redirectUrl = urlHelper.GenerateUrl(EnvironmentControllerActionNames.Index, ControllerNames.Environment);

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