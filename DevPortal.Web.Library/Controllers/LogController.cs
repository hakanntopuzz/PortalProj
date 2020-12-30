using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Log.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace DevPortal.Web.Library.Controllers
{
    public class LogController : BaseController
    {
        #region ctor

        readonly ILogService logService;

        readonly IApplicationReaderService applicationReaderService;

        readonly ILogViewModelFactory logViewModelFactory;

        public LogController(
            IUserSessionService userSessionService,
            ILogService logService,
            IApplicationReaderService applicationReaderService,
            ILogViewModelFactory viewModelFactory) : base(userSessionService)
        {
            this.logService = logService;
            this.applicationReaderService = applicationReaderService;
            this.logViewModelFactory = viewModelFactory;
        }

        #endregion

        #region Index

        public IActionResult Index(string physicalPath = null)
        {
            var applicationGroups = applicationReaderService.GetApplicationGroups();
            var model = logViewModelFactory.CreateLogViewModel(applicationGroups, physicalPath);

            return View(ViewNames.Index, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResultFileListPartial(string physicalPath)
        {
            if (!PhysicalPathExist(physicalPath))
            {
                return Json(ClientDataResult.Error("Dosya yolu boş olamaz."));
            }

            var files = logService.GetFilePathGenericList(physicalPath);

            if (IsFileListEmpty(files))
            {
                return Json(ClientDataResult.Error(files));
            }

            return Json(ClientDataResult.Success(files));
        }

        static bool IsFileListEmpty(Collection<LogFileModel> files)
        {
            return files.Count == 0;
        }

        static bool PhysicalPathExist(string physicalPath)
        {
            return !string.IsNullOrEmpty(physicalPath);
        }

        #endregion

        #region LogDetails

        [HttpGet]
        public IActionResult LogDetails(string path)
        {
            var logFile = logService.GetFileContent(path);

            if (!LogFileExists(logFile))
            {
                return EmptyLogFileDetailResult();
            }

            var viewModel = logViewModelFactory.CreateLogFileViewModel(logFile);

            return View(ViewNames.LogDetails, viewModel);
        }

        static bool LogFileExists(LogFileModel logFile)
        {
            return logFile != null;
        }

        IActionResult EmptyLogFileDetailResult()
        {
            var model = logViewModelFactory.CreateLogViewModel();

            return View(ViewNames.LogDetails, model);
        }

        #endregion

        #region DownloadLogFile

        public ActionResult DownloadLogFile(string path)
        {
            var file = logService.GetFileContents(path);

            return File(file.FileContent, ContentTypes.ForceDownload, file.Name);
        }

        #endregion

        #region GetApplications

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetApplicationsList(int applicationGroupId)
        {
            var model = logViewModelFactory.CreateLogViewModel();

            model.Applications = applicationReaderService.GetApplicationsWithLogByApplicationGroup(applicationGroupId);

            return Json(ClientDataResult.Success(model.Applications));
        }

        #endregion
    }
}