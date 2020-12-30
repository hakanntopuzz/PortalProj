using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class ApplicationGroupController : BaseController
    {
        #region ctor

        readonly IApplicationGroupService applicationGroupService;

        readonly IApplicationGroupViewModelFactory applicationGroupViewModelFactory;

        readonly IUrlGeneratorService urlHelper;

        readonly IApplicationReaderService applicationReaderService;

        public ApplicationGroupController(
            IUserSessionService userSessionService,
            IApplicationGroupService applicationGroupService,
            IApplicationGroupViewModelFactory viewModelFactory,
            IUrlGeneratorService urlHelper,
            IApplicationReaderService applicationReaderService) : base(userSessionService)
        {
            this.applicationGroupService = applicationGroupService;
            this.applicationGroupViewModelFactory = viewModelFactory;
            this.urlHelper = urlHelper;
            this.applicationReaderService = applicationReaderService;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var applicationGroups = applicationGroupService.GetApplicationGroups();

            var viewModel = applicationGroupViewModelFactory.CreateApplicationGroupsViewModel(applicationGroups);

            return View(ViewNames.Index, viewModel);
        }

        #endregion

        #region add application group

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Add()
        {
            return CreateApplicationGroupAddView();
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ApplicationGroup applicationGroup)
        {
            if (!ModelState.IsValid)
            {
                return CreateApplicationGroupAddView();
            }

            SetUserInfoForAdd(applicationGroup);

            var addResult = applicationGroupService.AddApplicationGroup(applicationGroup);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return CreateApplicationGroupAddView();
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return RedirectToAction(ApplicationGroupControllerActionNames.Detail, ControllerNames.ApplicationGroup, new { id = addResult.Value });
        }

        void SetUserInfoForAdd(ApplicationGroup applicationGroup)
        {
            int userId = CurrentUserId;
            applicationGroup.RecordUpdateInfo.CreatedBy = userId;
            applicationGroup.RecordUpdateInfo.ModifiedBy = userId;
        }

        ActionResult CreateApplicationGroupAddView()
        {
            var groupStatus = applicationGroupService.GetApplicationGroupStatusList();

            var viewModel = applicationGroupViewModelFactory.CreateApplicationGroupAddViewModel(groupStatus);

            return View(ViewNames.Add, viewModel);
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return ApplicationGroupNotFoundResult();
            }

            var applicationGroup = applicationGroupService.GetApplicationGroupById(id);

            if (applicationGroup == null)
            {
                return ApplicationGroupNotFoundResult();
            }

            var applicationList = applicationReaderService.GetApplicationsByGroupId(id);

            var viewModel = applicationGroupViewModelFactory.CreateDetailApplicationGroup(applicationGroup, applicationList);

            return View(ViewNames.Detail, viewModel);
        }

        #endregion

        #region edit

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return ApplicationGroupNotFoundResult();
            }

            var applicationGroup = applicationGroupService.GetApplicationGroupById(id);

            if (applicationGroup == null)
            {
                return ApplicationGroupNotFoundResult();
            }

            var statusList = applicationGroupService.GetApplicationGroupStatusList();
            var model = applicationGroupViewModelFactory.CreateEditApplicationGroup(applicationGroup, statusList);

            return View(ViewNames.Edit, model);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditApplicationGroupView(model);
            }

            SetUserInfoForUpdate(model.ApplicationGroup);

            var result = applicationGroupService.UpdateApplicationGroup(model.ApplicationGroup);

            if (!result.IsSuccess)
            {
                SetErrorResultMessageTempData(result.Message);

                return EditApplicationGroupView(model);
            }

            SetSuccessResultMessageTempData(result.Message);

            return RedirectToAction(ApplicationGroupControllerActionNames.Detail, ControllerNames.ApplicationGroup, new { id = model.ApplicationGroup.Id });
        }

        IActionResult EditApplicationGroupView(ApplicationGroupViewModel model)
        {
            model.Status = applicationGroupService.GetApplicationGroupStatusList();

            return View(ViewNames.Edit, model);
        }

        void SetUserInfoForUpdate(ApplicationGroup applicationGroup)
        {
            applicationGroup.RecordUpdateInfo.ModifiedBy = CurrentUserId;
        }

        #endregion

        IActionResult ApplicationGroupNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.ApplicationGroupNotFound);

            return RedirectToAction(ApplicationGroupControllerActionNames.Index, ControllerNames.ApplicationGroup);
        }

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id)
        {
            var result = applicationGroupService.DeleteApplicationGroup(id);
            var redirectUrl = urlHelper.GenerateUrl(ApplicationGroupControllerActionNames.Index, ControllerNames.ApplicationGroup);

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