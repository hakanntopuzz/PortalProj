using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationSonarqubeProjectController : BaseController
    {
        #region ctor

        readonly IApplicationSonarqubeProjectService applicationSonarqubeService;

        readonly IApplicationReaderService applicationReaderService;

        readonly IGeneralSettingsService generalSettingsService;

        readonly IApplicationSonarQubeProjectViewModelFactory viewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        public ApplicationSonarqubeProjectController(
            IUserSessionService userSessionService,
            IApplicationSonarqubeProjectService applicationSonarqubeService,
            IApplicationReaderService applicationReaderService,
            IGeneralSettingsService generalSettingsService,
            IApplicationSonarQubeProjectViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper,
            IRouteValueFactory routeValueFactory)
            : base(userSessionService)
        {
            this.applicationSonarqubeService = applicationSonarqubeService;
            this.applicationReaderService = applicationReaderService;
            this.generalSettingsService = generalSettingsService;
            this.viewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
        }

        #endregion

        #region index

        public IActionResult Index(int applicationId)
        {
            var projects = applicationSonarqubeService.GetSonarqubeProjects(applicationId);

            if (!SonarqubeProjectsExist(projects))
            {
                //TODO: Javascriptte success durumuna bakılmıyor, kayıt adedine bakılıyor. Bu if'e gerek olmayabilir.
                return Json(ClientDataResult.Error(projects));
            }

            return Json(ClientDataResult.Success(projects));
        }

        static bool SonarqubeProjectsExist(ICollection<SonarqubeProject> projects)
        {
            return projects != null;
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

            var projectTypeList = applicationSonarqubeService.GetSonarQubeProjectTypes();
            var projectUrl = generalSettingsService.GetSonarqubeProjectUrl().ToString();
            var viewModel = viewModelFactory.CreateApplicationSonarQubeProjectViewModel(application, projectTypeList, projectUrl);

            return View(ViewNames.Add, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ApplicationSonarQubeProjectViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Add, viewModel);
            }

            SetUserInfoForAdd(viewModel.ApplicationSonarQubeProject);

            var addResult = applicationSonarqubeService.AddApplicationSonarQubeProject(viewModel.ApplicationSonarQubeProject);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return View(ViewNames.Add, viewModel);
            }

            return AddSuccessView(viewModel.ApplicationSonarQubeProject.ApplicationId, addResult.Message);
        }

        void SetUserInfoForAdd(SonarqubeProject sonarqubeProject)
        {
            int userId = CurrentUserId;
            sonarqubeProject.RecordUpdateInfo.CreatedBy = userId;
            sonarqubeProject.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView(int applicationId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationControllerActionNames.Detail, ControllerNames.Application, new { id = applicationId });
        }

        #endregion

        #region detail

        public IActionResult Detail(int id = 0)
        {
            var sonarQubeProject = applicationSonarqubeService.GetSonarQubeProject(id);

            if (!SonarqubeProjectExists(sonarQubeProject))
            {
                return ProjectNotFoundResult();
            }

            var sonarQubeProjectTypes = applicationSonarqubeService.GetSonarQubeProjectTypes();
            var sonarQubeProjectUrl = generalSettingsService.GetSonarqubeProjectUrl().ToString();
            var application = applicationReaderService.GetApplication(sonarQubeProject.ApplicationId);
            var viewModel = viewModelFactory.CreateApplicationSonarQubeProjectViewModel(application, sonarQubeProject, sonarQubeProjectTypes, sonarQubeProjectUrl);

            return View(ViewNames.Detail, viewModel);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            var project = applicationSonarqubeService.GetSonarQubeProject(id);

            if (!SonarqubeProjectExists(project))
            {
                return ProjectNotFoundResult();
            }

            var projectTypeList = applicationSonarqubeService.GetSonarQubeProjectTypes();
            var projectUrl = generalSettingsService.GetSonarqubeProjectUrl().ToString();
            var application = applicationReaderService.GetApplication(project.ApplicationId);
            var viewModel = viewModelFactory.CreateEditApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl);

            return View(ViewNames.Edit, viewModel);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationSonarQubeProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelErrorView(model);
            }

            SetUserInfoForUpdate(model.ApplicationSonarQubeProject);

            var updateResult = applicationSonarqubeService.UpdateApplicationSonarQubeProject(model.ApplicationSonarQubeProject);

            if (!updateResult.IsSuccess)
            {
                return UpdateErrorView(model, updateResult.Message);
            }

            return UpdateSuccessView(model.ApplicationSonarQubeProject.SonarqubeProjectId, updateResult.Message);
        }

        #endregion

        static bool SonarqubeProjectExists(SonarqubeProject project)
        {
            return project != null;
        }

        IActionResult ProjectNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.SonarQubeProjectNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        IActionResult UpdateModelErrorView(ApplicationSonarQubeProjectViewModel model)
        {
            return CreateApplicationSonarQubeProjectEditView(ViewNames.Edit, model);
        }

        IActionResult UpdateErrorView(ApplicationSonarQubeProjectViewModel model, string message)
        {
            SetErrorResultMessageTempData(message);

            return CreateApplicationSonarQubeProjectEditView(ViewNames.Edit, model);
        }

        ActionResult CreateApplicationSonarQubeProjectEditView(string viewNames, ApplicationSonarQubeProjectViewModel model = null)
        {
            var projectTypeList = applicationSonarqubeService.GetSonarQubeProjectTypes();
            var projectUrl = generalSettingsService.GetSonarqubeProjectUrl().ToString();
            var application = applicationReaderService.GetApplication(model.ApplicationSonarQubeProject.ApplicationId);
            var viewModel = viewModelFactory.CreateApplicationSonarQubeProjectViewModel(application, model.ApplicationSonarQubeProject, projectTypeList, projectUrl);

            return View(viewNames, viewModel);
        }

        IActionResult UpdateSuccessView(int sonarqubeProjectId, string message)
        {
            SetSuccessResultMessageTempData(message);

            return RedirectToAction(ApplicationSonarqubeProjectControllerActionNames.Detail, ControllerNames.ApplicationSonarqubeProject, new { id = sonarqubeProjectId });
        }

        void SetUserInfoForUpdate(SonarqubeProject sonarqubeProject)
        {
            sonarqubeProject.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = applicationSonarqubeService.DeleteApplicationSonarQubeProject(id);
            var urlParams = routeValueFactory.CreateRouteValuesForGenerateUrl(applicationId);
            var redirectUrl = urlHelper.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, urlParams);

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