using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class DatabaseController : BaseController
    {
        #region ctor

        readonly IDatabaseWriterService databaseWriterService;

        readonly IDatabaseReaderService databaseReaderService;

        readonly IDatabaseViewModelFactory viewModelFactory;

        readonly IDatabaseTypeService databaseTypeService;

        readonly IUrlGeneratorService urlHelper;

        public DatabaseController(
            IUserSessionService userSessionService,
            IDatabaseWriterService databaseWriterService,
            IDatabaseReaderService databaseReaderService,
            IDatabaseViewModelFactory viewModelFactory,
            IDatabaseTypeService databaseTypeService,
            IUrlGeneratorService urlHelper) : base(userSessionService)
        {
            this.databaseWriterService = databaseWriterService;
            this.databaseReaderService = databaseReaderService;
            this.viewModelFactory = viewModelFactory;
            this.databaseTypeService = databaseTypeService;
            this.urlHelper = urlHelper;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var databaseGroups = databaseReaderService.GetDatabaseGroups();
            var databases = databaseReaderService.GetDatabases();
            var viewModel = viewModelFactory.CreateDatabasesViewModel(databaseGroups, databases);

            return View(ViewNames.Index, viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DatabaseTableParam tableParam)
        {
            var databaseList = await databaseReaderService.GetFilteredDatabaseListAsync(tableParam);

            var databaseListModel = viewModelFactory.CreateDatabaseListModel(databaseList);

            return Ok(databaseListModel);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return DatabaseNotFoundResult();
            }

            var database = databaseReaderService.GetDatabase(id);

            if (!DatabaseExists(database))
            {
                return DatabaseNotFoundResult();
            }

            var model = viewModelFactory.CreateDatabase(database);

            return View(ViewNames.Detail, model);
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
        public IActionResult Add(Database database)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(database);
            }

            SetUserInfoForAdd(database);

            var addResult = databaseWriterService.AddDatabase(database);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(database);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView();
        }

        IActionResult AddErrorView(Database database)
        {
            var model = CreateAddViewModel();
            model.Database = database;

            return View(ViewNames.Add, model);
        }

        void SetUserInfoForAdd(Database database)
        {
            int userId = CurrentUserId;
            database.RecordUpdateInfo.CreatedBy = userId;
            database.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView()
        {
            return RedirectToAction(DatabaseControllerActionNames.Index, ControllerNames.Database);
        }

        DatabaseViewModel CreateAddViewModel()
        {
            var databaseTypes = databaseReaderService.GetDatabaseTypes();
            var databaseGroups = databaseReaderService.GetDatabaseGroups();

            return viewModelFactory.CreateAddViewModel(databaseTypes, databaseGroups);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return DatabaseNotFoundResult();
            }

            var database = databaseReaderService.GetDatabase(id);

            if (!DatabaseExists(database))
            {
                return DatabaseNotFoundResult();
            }

            return EditView(database);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditDatabaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditView(model.Database);
            }

            SetUserInfoForUpdate(model.Database);

            var updateResult = databaseWriterService.UpdateDatabase(model.Database);

            if (!updateResult.IsSuccess)
            {
                SetErrorResultMessageTempData(updateResult.Message);

                return EditView(model.Database);
            }

            SetSuccessResultMessageTempData(updateResult.Message);

            return RedirectToAction(DatabaseControllerActionNames.Detail, new { id = model.Database.Id });
        }

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        IActionResult EditView(Database database)
        {
            var databaseGroups = databaseReaderService.GetDatabaseGroups();
            var databaseTypes = databaseTypeService.GetDatabaseTypes();
            var model = viewModelFactory.CreateEditDatabase(database, databaseGroups, databaseTypes);

            return View(ViewNames.Edit, model);
        }

        void SetUserInfoForUpdate(Database database)
        {
            database.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id)
        {
            var deleteResult = databaseWriterService.DeleteDatabase(id);
            var redirectUrl = urlHelper.GenerateUrl(DatabaseControllerActionNames.Index, ControllerNames.Database);

            if (!deleteResult.IsSuccess)
            {
                return Json(RedirectableClientActionResult.Error(redirectUrl, deleteResult.Message));
            }

            SetSuccessResultMessageTempData(deleteResult.Message);

            return Json(RedirectableClientActionResult.Success(redirectUrl, deleteResult.Message));
        }

        #endregion

        static bool DatabaseExists(Database database)
        {
            return database != null;
        }

        IActionResult DatabaseNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.DatabaseNotFound);

            return RedirectToAction(DatabaseControllerActionNames.Index, ControllerNames.Database);
        }
    }
}