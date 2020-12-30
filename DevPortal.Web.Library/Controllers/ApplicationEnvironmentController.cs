using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationEnvironmentController : BaseController
    {
        #region ctor

        readonly IApplicationEnvironmentService applicationEnvironmentService;

        readonly IApplicationReaderService applicationReaderService;

        readonly IApplicationEnvironmentViewModelFactory viewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        readonly IEnvironmentService environmentService;

        public ApplicationEnvironmentController(
            IUserSessionService userSessionService,
            IApplicationEnvironmentService applicationEnvironmentService,
            IApplicationReaderService applicationReaderService,
            IApplicationEnvironmentViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper,
            IRouteValueFactory routeValueFactory,
            IEnvironmentService environmentService)
            : base(userSessionService)
        {
            this.applicationEnvironmentService = applicationEnvironmentService;
            this.applicationReaderService = applicationReaderService;
            this.viewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
            this.environmentService = environmentService;
        }

        #endregion

        #region index

        public IActionResult Index(int applicationId)
        {
            var environments = applicationEnvironmentService.GetApplicationEnvironments(applicationId);

            if (!EnvironmentsExist(environments))
            {
                return Json(ClientDataResult.Error(environments));
            }

            return Json(ClientDataResult.Success(environments));
        }

        static bool EnvironmentsExist(ICollection<ApplicationEnvironment> environments)
        {
            return environments != null;
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

            var applicationEnvironment = viewModelFactory.CreateApplicationEnvironment(applicationId, application.Name);

            var model = viewModelFactory.CreateApplicationEnvironmentViewModel(applicationEnvironment);

            return CreateApplicationEnvironmentAddView(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]

        //TODO: Parametre olarak ViewModel kullanılmamalı. ApplicationEnvironment tipi kullanılmalı.
        public IActionResult Add(ApplicationEnvironmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return CreateApplicationEnvironmentAddView(ViewNames.Add, viewModel);
            }

            SetUserInfoForAdd(viewModel.ApplicationEnvironment);

            var addResult = applicationEnvironmentService.AddApplicationEnvironment(viewModel.ApplicationEnvironment);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return CreateApplicationEnvironmentAddView(ViewNames.Add, viewModel);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            var model = viewModelFactory.CreateApplicationEnvironment(viewModel.ApplicationEnvironment);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = model.ApplicationId });
        }

        void SetUserInfoForAdd(ApplicationEnvironment applicationEnvironment)
        {
            int userId = CurrentUserId;
            applicationEnvironment.RecordUpdateInfo.CreatedBy = userId;
            applicationEnvironment.RecordUpdateInfo.ModifiedBy = userId;
        }

        ActionResult CreateApplicationEnvironmentAddView(string viewNames, ApplicationEnvironmentViewModel model = null)
        {
            var environments = applicationEnvironmentService.GetEnvironmentsDoesNotExist(model.ApplicationEnvironment.ApplicationId);
            var viewModel = viewModelFactory.CreateApplicationEnvironmentViewModel(model, environments);

            return View(viewNames, viewModel);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            var applicationEnvironment = applicationEnvironmentService.GetApplicationEnvironment(id);

            if (!ApplicationEnvironmentExists(applicationEnvironment))
            {
                return DetailErrorView(Messages.ApplicationEnvironmentNotFound);
            }

            var viewModel = viewModelFactory.CreateApplicationEnvironmentViewModel(applicationEnvironment);

            return CreateApplicationEnvironmentEditView(ViewNames.Edit, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationEnvironmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelErrorView(model);
            }

            SetUserInfoForUpdate(model.ApplicationEnvironment);

            var updateResult = applicationEnvironmentService.UpdateApplicationEnvironment(model.ApplicationEnvironment);

            if (!updateResult.IsSuccess)
            {
                return UpdateErrorView(model, updateResult.Message);
            }

            return UpdateSuccessView(model.ApplicationEnvironment.ApplicationId, updateResult.Message);
        }

        void SetUserInfoForUpdate(ApplicationEnvironment applicationEnvironment)
        {
            applicationEnvironment.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        IActionResult UpdateModelErrorView(ApplicationEnvironmentViewModel model)
        {
            return CreateApplicationEnvironmentEditView(ViewNames.Edit, model);
        }

        IActionResult UpdateErrorView(ApplicationEnvironmentViewModel model, string message)
        {
            SetErrorResultMessageTempData(message);

            return CreateApplicationEnvironmentEditView(ViewNames.Edit, model);
        }

        IActionResult UpdateSuccessView(int applicationId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = applicationId });
        }

        ActionResult CreateApplicationEnvironmentEditView(string viewNames, ApplicationEnvironmentViewModel model = null)
        {
            //TODO: Uygulama ortamının ortam bilgisi + uygulamaya ait uygulama ortamlarında bulunmayan ortamlar listelenmeli. İki liste yapılıp birleştirilebilir.
            //Mevcut uygulama ortamlarından birine ait ortam seçilememeli.
            var environments = environmentService.GetEnvironments();
            var viewModel = viewModelFactory.CreateEditApplicationEnvironmentViewModel(model.ApplicationEnvironment, environments);

            return View(viewNames, viewModel);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var applicationEnvironment = applicationEnvironmentService.GetApplicationEnvironment(id);

            if (!ApplicationEnvironmentExists(applicationEnvironment))
            {
                return DetailErrorView(Messages.ApplicationEnvironmentNotFound);
            }

            var model = viewModelFactory.CreateApplicationEnvironmentDetailViewModel(applicationEnvironment);

            return DetailSuccessView(model);
        }

        static bool ApplicationEnvironmentExists(ApplicationEnvironment applicationEnvironment)
        {
            return applicationEnvironment != null;
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        IActionResult DetailSuccessView(ApplicationEnvironmentViewModel model)
        {
            return View(ViewNames.Detail, model);
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = applicationEnvironmentService.DeleteApplicationEnvironment(id);
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

        #region GetEnvironments

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetEnvironmentList(int applicationId)
        {
            var environments = applicationEnvironmentService.GetApplicationEnvironmentsHasLog(applicationId);

            if (!EnvironmentExists(environments))
            {
                return Json(ClientDataResult.Error(Messages.ApplicationEnvironmentNotFound));
            }

            return Json(ClientDataResult.Success(environments));
        }

        static bool EnvironmentExists(ICollection<ApplicationEnvironment> environments)
        {
            return environments != null && environments.Count > 0;
        }

        #endregion
    }
}