using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    public class NugetPackageDependencyController : BaseController
    {
        #region ctor

        readonly INugetPackageDependencyService nugetPackageDependencyService;

        readonly INugetPackageDependencyViewModelFactory nugetPackageDependencyViewModelFactory;

        readonly INugetService nugetService;

        readonly INugetPackageService nugetPackageService;

        readonly IApplicationReaderService applicationReaderService;

        readonly IUrlGeneratorService urlGeneratorService;

        readonly IRouteValueFactory routeValueFactory;

        public NugetPackageDependencyController(
             INugetService nugetService,
             IUserSessionService userSessionService,
             INugetPackageDependencyService nugetPackageDependencyService,
             INugetPackageDependencyViewModelFactory nugetPackageDependencyViewModelFactory,
             INugetPackageService nugetPackageService,
             IApplicationReaderService applicationReaderService,
             IUrlGeneratorService urlGeneratorService,
             IRouteValueFactory routeValueFactory)
            : base(userSessionService)
        {
            this.nugetService = nugetService;
            this.nugetPackageDependencyService = nugetPackageDependencyService;
            this.nugetPackageDependencyViewModelFactory = nugetPackageDependencyViewModelFactory;
            this.nugetPackageService = nugetPackageService;
            this.applicationReaderService = applicationReaderService;
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

        NugetPackageDependencyViewModel CreateAddViewModel(int applicationId)
        {
            var application = applicationReaderService.GetApplication(applicationId);
            return nugetPackageDependencyViewModelFactory.CreateNugetPackageDependencyViewModelAddView(application);
        }

        [HttpPost]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        [ValidateAntiForgeryToken]
        public IActionResult Add(NugetPackageDependency nugetPackageDependency)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorView(nugetPackageDependency);
            }

            SetUserInfoForAdd(nugetPackageDependency);

            var addResult = nugetPackageDependencyService.AddNugetPackageDependency(nugetPackageDependency);

            if (!addResult.IsSuccess)
            {
                SetErrorResultMessageTempData(addResult.Message);

                return AddErrorView(nugetPackageDependency);
            }

            SetSuccessResultMessageTempData(addResult.Message);

            return AddSuccessView(nugetPackageDependency.DependentApplicationId);
        }

        IActionResult AddErrorView(NugetPackageDependency nugetPackageDependency)
        {
            var model = CreateAddViewModel(nugetPackageDependency.DependentApplicationId);
            model.NugetPackageDependency = nugetPackageDependency;

            return View(ViewNames.Add, model);
        }

        void SetUserInfoForAdd(NugetPackageDependency nugetPackageDependency)
        {
            int userId = CurrentUserId;
            nugetPackageDependency.RecordUpdateInfo.CreatedBy = userId;
            nugetPackageDependency.RecordUpdateInfo.ModifiedBy = userId;
        }

        IActionResult AddSuccessView(int applicationId)
        {
            var redirectUrl = urlGeneratorService.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId);

            return Redirect(redirectUrl);
        }

        public List<NugetPackage> GetFilteredNugetPackages()
        {
            return nugetService.GetGroupedNugetPackages();
        }

        public IActionResult GetNugetPackageRootUrl()
        {
            var url = nugetPackageService.GetNugetPackageRootUrl();

            return Content(url.ToString());
        }

        #endregion

        #region detail

        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return NugetPackageDependencyNotFoundResult();
            }

            var nugetPackageDependency = nugetPackageDependencyService.GetNugetPackageDependencyById(id);

            if (NugetPackageDependencyExists(nugetPackageDependency))
            {
                return NugetPackageDependencyNotFoundResult();
            }

            var model = nugetPackageDependencyViewModelFactory.CreateNugetPackageDependencyViewModel(nugetPackageDependency);

            return View(ViewNames.Detail, model);
        }

        IActionResult NugetPackageDependencyNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.NugetPackageDependencyNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        static bool NugetPackageDependencyExists(NugetPackageDependency nugetPackageDependency)
        {
            return nugetPackageDependency == null;
        }

        #endregion

        #region delete

        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public JsonResult Delete(int id, int applicationId)
        {
            var result = nugetPackageDependencyService.DeleteNugetPackageDependency(id);

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