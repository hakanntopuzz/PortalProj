using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class DatabaseDependencyController : BaseController
    {
        #region ctor

        readonly IDatabaseDependencyService databaseDependencyService;

        readonly IDatabaseDependencyViewModelFactory viewModelFactory;

        readonly IDatabaseReaderService databaseReaderService;

        readonly IUrlGeneratorService urlGeneratorService;

        readonly IRouteValueFactory routeValueFactory;

        public DatabaseDependencyController(
            IUserSessionService userSessionService,
            IDatabaseDependencyService databaseDependencyService,
            IDatabaseDependencyViewModelFactory viewModelFactory,
            IDatabaseReaderService databaseReaderService,
            IUrlGeneratorService urlGeneratorService,
            IRouteValueFactory routeValueFactory)
            : base(userSessionService)
        {
            this.databaseDependencyService = databaseDependencyService;
            this.viewModelFactory = viewModelFactory;
            this.databaseReaderService = databaseReaderService;
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
        public IActionResult Add(DatabaseDependency databaseDependency)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(databaseDependency);
            }

            SetUserInfoForAdd(databaseDependency);

            var addResult = databaseDependencyService.AddDatabaseDependency(databaseDependency);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(databaseDependency);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView(databaseDependency.ApplicationId);
        }

        IActionResult AddErrorView(DatabaseDependency databaseDependency)
        {
            var model = CreateAddViewModel(databaseDependency.ApplicationId);
            model.DatabaseDependency = databaseDependency;

            return View(ViewNames.Add, model);
        }

        DatabaseDependencyViewModel CreateAddViewModel(int applicationId)
        {
            var databaseGroups = databaseReaderService.GetDatabaseGroups();
            var databases = databaseReaderService.GetDatabases();

            return viewModelFactory.CreatDatabaseDependencyViewModelAddView(applicationId, databaseGroups, databases);
        }

        void SetUserInfoForAdd(DatabaseDependency databaseDependency)
        {
            int userId = CurrentUserId;
            databaseDependency.RecordUpdateInfo.CreatedBy = userId;
            databaseDependency.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView(int applicationId)
        {
            var redirectUrl = urlGeneratorService.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId);

            return Redirect(redirectUrl);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            return EditView(id);
        }

        IActionResult EditView(int databaseDependencyId)
        {
            var databaseDependency = databaseDependencyService.GetDatabaseDependency(databaseDependencyId);

            if (DatabaseDependencyExists(databaseDependency))
            {
                return DetailErrorView(Messages.DatabaseDependencyNotFound);
            }

            var model = viewModelFactory.CreateDatabaseDependencyEditViewModel(databaseDependency);

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
        public IActionResult Edit(DatabaseDependencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditErrorView(model.DatabaseDependency);
            }

            SetUserInfoForUpdate(model.DatabaseDependency);

            var serviceResult = databaseDependencyService.UpdateDatabaseDependency(model.DatabaseDependency);

            if (serviceResult.IsSuccess)
            {
                SetSuccessResultMessageTempData(serviceResult.Message);

                return UpdateSuccessView(model.DatabaseDependency.Id);
            }

            SetErrorResultMessageTempData(serviceResult.Message);

            return EditView(model.DatabaseDependency.Id);
        }

        IActionResult EditErrorView(DatabaseDependency databaseDependency)
        {
            var model = CreateEditViewModel(databaseDependency);

            return View(ViewNames.Edit, model);
        }

        DatabaseDependencyViewModel CreateEditViewModel(DatabaseDependency databaseDependency)
        {
            return viewModelFactory.CreateDatabaseDependencyEditViewModel(databaseDependency);
        }

        void SetUserInfoForUpdate(DatabaseDependency databaseDependency)
        {
            databaseDependency.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        IActionResult UpdateSuccessView(int databaseDependencyId)
        {
            var redirectUrl = urlGeneratorService.GenerateUrl(DatabaseDependencyControllerActionNames.Detail, ControllerNames.DatabaseDependency, databaseDependencyId);

            return Redirect(redirectUrl);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return DatabaseDependencyNotFoundResult();
            }

            var databaseDependency = databaseDependencyService.GetDatabaseDependency(id);

            if (DatabaseDependencyExists(databaseDependency))
            {
                return DatabaseDependencyNotFoundResult();
            }

            var model = viewModelFactory.CreateDatabaseDependencyViewModel(databaseDependency);

            return View(ViewNames.Detail, model);
        }

        IActionResult DatabaseDependencyNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.DatabaseDependencyNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        static bool DatabaseDependencyExists(DatabaseDependency databaseDependency)
        {
            return databaseDependency == null;
        }

        #endregion

        #region get databases by database group id

        [HttpGet]
        public IActionResult GetDatabasesByDatabaseGroupId(int databaseGroupId)
        {
            var databases = databaseReaderService.GetDatabasesByDatabaseGroupId(databaseGroupId);

            return Json(databases);
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = databaseDependencyService.DeleteDatabaseDependency(id);

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