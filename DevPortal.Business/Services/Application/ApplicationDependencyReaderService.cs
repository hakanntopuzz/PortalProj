using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Services
{
    public class ApplicationDependencyReaderService : IApplicationDependencyReaderService
    {
        #region ctor

        readonly IApplicationRepository applicationRepository;

        readonly IApplicationDependencyRepository applicationDependencyRepository;

        readonly INugetPackageService nugetPackageService;

        public ApplicationDependencyReaderService(
            IApplicationRepository applicationRepository,
            IApplicationDependencyRepository applicationDependencyRepository,
            INugetPackageService nugetPackageService)
        {
            this.applicationRepository = applicationRepository;
            this.applicationDependencyRepository = applicationDependencyRepository;
            this.nugetPackageService = nugetPackageService;
        }

        #endregion

        public ApplicationDependency GetApplicationDependency(int id)
        {
            var applicationDependency = applicationDependencyRepository.GetApplicationDependencyById(id);

            if (applicationDependency == null)
            {
                return null;
            }

            applicationDependency.RecordUpdateInfo = applicationDependencyRepository.GetApplicationDependencyUpdateInfo(id);

            return applicationDependency;
        }

        public ICollection<ApplicationDependenciesExportListItem> GetApplicationDependenciesExportList(int applicationId)
        {
            return applicationRepository.GetApplicationDependencies(applicationId);
        }

        public ICollection<DatabaseDependenciesExportListItem> GetDatabaseDependenciesExportList(int applicationId)
        {
            return applicationRepository.GetDatabaseDependencies(applicationId);
        }

        public ICollection<ExternalDependenciesExportListItem> GetExternalDependenciesExportList(int applicationId)
        {
            return applicationRepository.GetFullExternalDependenciesByApplicationId(applicationId);
        }

        public ICollection<NugetPackageDependenciesExportListItem> GetNugetPackageDependenciesExportList(int applicationId)
        {
            var packages = applicationRepository.GetNugetPackageDependencies(applicationId);
            SetNugetPackageUrls(packages);

            return packages;
        }

        public ICollection<ApplicationDependency> GetApplicationDependencies(int applicationId)
        {
            return applicationRepository.GetApplicationDependenciesByApplicationId(applicationId);
        }

        public ICollection<ExternalDependency> GetExternalDependencies(int applicationId)
        {
            return applicationRepository.GetExternalDependenciesByApplicationId(applicationId);
        }

        public ICollection<NugetPackageDependency> GetNugetPackageDependencies(int applicationId)
        {
            var packages = applicationRepository.GetNugetPackageDependenciesByApplicationId(applicationId);
            SetNugetPackageUrls(packages);

            return packages;
        }

        #region helper methods

        void SetNugetPackageUrls(ICollection<NugetPackageDependency> packages)
        {
            packages.ToList().ForEach(q => q.PackageUrl = nugetPackageService.GetNugetPackageUrl(q.NugetPackageName).ToString());
        }

        void SetNugetPackageUrls(ICollection<NugetPackageDependenciesExportListItem> packages)
        {
            packages.ToList().ForEach(q => q.PackageUrl = nugetPackageService.GetNugetPackageUrl(q.Name).ToString());
        }

        #endregion
    }
}