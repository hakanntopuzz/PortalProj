using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class DatabaseGroupController : BaseController
    {
        #region ctor

        readonly IDatabaseGroupService databaseGroupService;

        readonly IDatabaseReaderService databaseReaderService;

        readonly IDatabaseGroupViewModelFactory viewModelFactory;

        public DatabaseGroupController(
            IUserSessionService userSessionService,
            IDatabaseGroupService databaseGroupService,
            IDatabaseReaderService databaseReaderService,
            IDatabaseGroupViewModelFactory viewModelFactory) : base(userSessionService)
        {
            this.databaseGroupService = databaseGroupService;
            this.databaseReaderService = databaseReaderService;
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var databaseGroups = databaseGroupService.GetDatabaseGroups();
            var viewModel = viewModelFactory.CreateDatabaseGroupViewModel(databaseGroups);

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
        public IActionResult Add(DatabaseGroup databaseGroup)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(databaseGroup);
            }

            SetUserInfoForAdd(databaseGroup);

            var addResult = databaseGroupService.AddDatabaseGroup(databaseGroup);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(databaseGroup);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView();
        }

        IActionResult AddErrorView(DatabaseGroup databaseGroup)
        {
            var model = CreateAddViewModel();
            model.DatabaseGroup = databaseGroup;

            return View(ViewNames.Add, model);
        }

        void SetUserInfoForAdd(DatabaseGroup databaseGroup)
        {
            int userId = CurrentUserId;
            databaseGroup.RecordUpdateInfo.CreatedBy = userId;
            databaseGroup.RecordUpdateInfo.ModifiedBy = userId;
        }

        DatabaseGroupViewModel CreateAddViewModel()
        {
            return viewModelFactory.CreateDatabaseGroupAddView();
        }

        IActionResult AddSuccessView()
        {
            return RedirectToAction(DatabaseGroupControllerActionNames.Index, ControllerNames.DatabaseGroup);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var databaseGroup = databaseGroupService.GetDatabaseGroup(id);

            if (DatabaseGroupExists(databaseGroup))
            {
                return DetailErrorView(Messages.DatabaseGroupNotFound);
            }

            var databases = databaseReaderService.GetDatabasesByDatabaseGroupId(id);
            var model = viewModelFactory.CreateDatabaseGroupDetailViewModel(databaseGroup, databases);

            return View(ViewNames.Detail, model);
        }

        static bool DatabaseGroupExists(DatabaseGroup databaseGroup)
        {
            return databaseGroup == null;
        }

        IActionResult DetailErrorView(string message)
        {
            SetErrorResultMessageTempData(message);

            return RedirectToAction(DatabaseGroupControllerActionNames.Index, ControllerNames.DatabaseGroup);
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
        public IActionResult Edit(DatabaseGroup databaseGroup)
        {
            if (!ModelState.IsValid)
            {
                return EditErrorView(databaseGroup);
            }

            SetUserInfoForUpdate(databaseGroup);

            var serviceResult = databaseGroupService.UpdateDatabaseGroup(databaseGroup);

            if (serviceResult.IsSuccess)
            {
                SetSuccessResultMessageTempData(serviceResult.Message);

                return EditSuccessView(databaseGroup.Id);
            }

            SetErrorResultMessageTempData(serviceResult.Message);

            return EditView(databaseGroup.Id);
        }

        IActionResult EditErrorView(DatabaseGroup databaseGroup)
        {
            var model = CreateEditViewModel(databaseGroup);

            return View(ViewNames.Edit, model);
        }

        IActionResult EditView(int databaseGroupId)
        {
            var databaseGroup = databaseGroupService.GetDatabaseGroup(databaseGroupId);

            if (DatabaseGroupExists(databaseGroup))
            {
                return DetailErrorView(Messages.DatabaseGroupNotFound);
            }

            var model = viewModelFactory.CreateDatabaseGroupEditViewModel(databaseGroup);

            return View(ViewNames.Edit, model);
        }

        void SetUserInfoForUpdate(DatabaseGroup databaseGroup)
        {
            databaseGroup.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        DatabaseGroupViewModel CreateEditViewModel(DatabaseGroup databaseGroup)
        {
            return viewModelFactory.CreateDatabaseGroupEditViewModel(databaseGroup);
        }

        IActionResult EditSuccessView(int databaseGroupId)
        {
            return RedirectToAction(DatabaseGroupControllerActionNames.Detail, ControllerNames.DatabaseGroup, new { id = databaseGroupId });
        }

        #endregion

        #region delete

        public JsonResult Delete(int id)
        {
            var deleteResult = databaseGroupService.DeleteDatabaseGroup(id);

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