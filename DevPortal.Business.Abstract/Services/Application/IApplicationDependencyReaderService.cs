using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationDependencyReaderService
    {
        ApplicationDependency GetApplicationDependency(int id);

        ICollection<ApplicationDependenciesExportListItem> GetApplicationDependenciesExportList(int applicationId);

        ICollection<DatabaseDependenciesExportListItem> GetDatabaseDependenciesExportList(int applicationId);

        ICollection<ExternalDependenciesExportListItem> GetExternalDependenciesExportList(int applicationId);

        ICollection<NugetPackageDependenciesExportListItem> GetNugetPackageDependenciesExportList(int applicationId);

        ICollection<ApplicationDependency> GetApplicationDependencies(int applicationId);

        ICollection<ExternalDependency> GetExternalDependencies(int applicationId);

        ICollection<NugetPackageDependency> GetNugetPackageDependencies(int applicationId);
    }
}