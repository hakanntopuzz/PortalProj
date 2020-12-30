using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class DatabaseTypeController : BaseController
    {
        #region ctor

        readonly IDatabaseTypeService databaseTypeService;

        readonly IDatabaseReaderService databaseReaderService;

        readonly IDatabaseTypeViewModelFactory viewModelFactory;

        public DatabaseTypeController(
            IUserSessionService userSessionService,
            IDatabaseTypeService databaseTypeService,
            IDatabaseReaderService databaseReaderService,
            IDatabaseTypeViewModelFactory viewModelFactory) : base(userSessionService)
        {
            this.databaseTypeService = databaseTypeService;
            this.databaseReaderService = databaseReaderService;
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var databaseTypes = databaseTypeService.GetDatabaseTypes();
            var viewModel = viewModelFactory.CreateDatabaseTypeViewModel(databaseTypes);

            return View(ViewNames.Index, viewModel);
        }

        #endregion

        #region add

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add()
        {
            var model = CreateAddViewModel();

            return View(ViewNames.Add, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(DatabaseType databaseType)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(databaseType);
            }

            SetUserInfoForAdd(databaseType);

            var addResult = databaseTypeService.AddDatabaseType(databaseType);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(databaseType);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView();
        }

        IActionResult AddErrorView(DatabaseType databaseType)
        {
            var model = CreateAddViewModel();
            model.DatabaseType = databaseType;

            return View(ViewNames.Add, model);
        }

        void SetUserInfoForAdd(DatabaseType databaseType)
        {
            int userId = CurrentUserId;
            databaseType.RecordUpdateInfo.CreatedBy = userId;
            databaseType.RecordUpdateInfo.ModifiedBy = userId;
        }

        DatabaseTypeViewModel CreateAddViewModel()
        {
            return viewModelFactory.CreateDatabaseTypeAddView();
        }

        IActionResult AddSuccessView()
        {
            return RedirectToAction(DatabaseTypeControllerActionNames.Index, ControllerNames.DatabaseType);
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
        public IActionResult Edit(DatabaseType databaseType)
        {
            if (!ModelState.IsValid)
            {
                return EditErrorView(databaseType);
            }

            SetUserInfoForUpdate(databaseType);

            var serviceResult = databaseTypeService.UpdateDatabaseType(databaseType);

            if (serviceResult.IsSuccess)
            {
                SetSuccessResultMessageTempData(serviceResult.Message);

                return EditSuccessView(databaseType.Id);
            }

            SetErrorResultMessageTempData(serviceResult.Message);

            return EditView(databaseType.Id);
        }

        IActionResult EditErrorView(DatabaseType databaseType)
        {
            var model = CreateEditViewModel(databaseType);

            return View(ViewNames.Edit, model);
        }

        IActionResult EditView(int databaseTypeId)
        {
            var databaseType = databaseTypeService.GetDatabaseType(databaseTypeId);

            if (DatabaseTypeExists(databaseType))
            {
                return DetailErrorView(Messages.DatabaseTypeNotFound);
            }

            var model = viewModelFactory.CreateDatabaseTypeEditViewModel(databaseType);

            return View(ViewNames.Edit, model);
        }

        void SetUserInfoForUpdate(DatabaseType databaseType)
        {
            databaseType.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        DatabaseTypeViewModel CreateEditViewModel(DatabaseType databaseType)
        {
            return viewModelFactory.CreateDatabaseTypeEditViewModel(databaseType);
        }

        IActionResult EditSuccessView(int databaseTypeId)
        {
            return RedirectToAction(DatabaseTypeControllerActionNames.Detail, ControllerNames.DatabaseType, new { id = databaseTypeId });
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var databaseType = databaseTypeService.GetDatabaseType(id);

            if (DatabaseTypeExists(databaseType))
            {
                return DetailErrorView(Messages.DatabaseTypeNotFound);
            }

            var databases = databaseReaderService.GetDatabaseByDatabaseTypeId(id);
            var model = viewModelFactory.CreateDatabaseTypeDetailViewModel(databaseType, databases);

            return View(ViewNames.Detail, model);
        }

        static bool DatabaseTypeExists(DatabaseType databaseType)
        {
            return databaseType == null;
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(DatabaseTypeControllerActionNames.Index, ControllerNames.DatabaseType);
        }

        #endregion

        #region delete

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id)
        {
            var deleteResult = databaseTypeService.DeleteDatabaseType(id);

            if (!deleteResult.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(deleteResult.RedirectUrl, deleteResult.Message));
            }

            SetSuccessResultMessageTempData(deleteResult.Message);

            return Json(RedirectableClientActionResult.Success(deleteResult.RedirectUrl, deleteResult.Message));
        }

        #endregion
    }
}