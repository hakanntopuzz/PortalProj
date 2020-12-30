using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    public class DependencyListController : BaseController
    {
        #region ctor

        readonly IApplicationDependencyReaderService applicationDependencyReaderService;

        readonly IDatabaseDependencyService databaseDependencyService;

        public DependencyListController(
            IUserSessionService userSessionService,
            IApplicationDependencyReaderService applicationDependencyReaderService,
            IDatabaseDependencyService databaseDependencyService)
            : base(userSessionService)
        {
            this.applicationDependencyReaderService = applicationDependencyReaderService;
            this.databaseDependencyService = databaseDependencyService;
        }

        #endregion

        #region get application dependencies by application id

        public IActionResult GetApplicationDependenciesByApplicationId(int applicationId)
        {
            var applicationDependencies = applicationDependencyReaderService.GetApplicationDependencies(applicationId);

            if (!ApplicationDependenciesExist(applicationDependencies))
            {
                return Json(ClientDataResult.Error(applicationDependencies));
            }

            return Json(ClientDataResult.Success(applicationDependencies));
        }

        static bool ApplicationDependenciesExist(ICollection<ApplicationDependency> applicationDependencies)
        {
            return applicationDependencies != null;
        }

        #endregion

        #region get database dependencies by application id

        public IActionResult GetDatabaseDependenciesByApplicationId(int applicationId)
        {
            var databaseDependencies = databaseDependencyService.GetDatabaseDependenciesByApplicationId(applicationId);

            if (!DatabaseDependenciesExist(databaseDependencies))
            {
                return Json(ClientDataResult.Error(databaseDependencies));
            }

            return Json(ClientDataResult.Success(databaseDependencies));
        }

        static bool DatabaseDependenciesExist(ICollection<DatabaseDependency> databases)
        {
            return databases != null;
        }

        #endregion

        #region get external dependencies by application id

        [HttpGet]
        public IActionResult GetExternalDependenciesByApplicationId(int applicationId)
        {
            var externalDependency = applicationDependencyReaderService.GetExternalDependencies(applicationId);

            if (!ExternalDependenciesExist(externalDependency))
            {
                return Json(ClientDataResult.Error(externalDependency));
            }

            return Json(ClientDataResult.Success(externalDependency));
        }

        static bool ExternalDependenciesExist(ICollection<ExternalDependency> externalDependency)
        {
            return externalDependency != null;
        }

        #endregion

        #region get nuget package dependencies by application id

        [HttpGet]
        public IActionResult GetNugetPackageDependenciesByApplicationId(int applicationId)
        {
            var nugetPackageDependency = applicationDependencyReaderService.GetNugetPackageDependencies(applicationId);

            if (!NugetPackageDependenciesExist(nugetPackageDependency))
            {
                return Json(ClientDataResult.Error(nugetPackageDependency));
            }

            return Json(ClientDataResult.Success(nugetPackageDependency));
        }

        static bool NugetPackageDependenciesExist(ICollection<NugetPackageDependency> nugetPackageDependency)
        {
            return nugetPackageDependency != null;
        }

        #endregion
    }
}