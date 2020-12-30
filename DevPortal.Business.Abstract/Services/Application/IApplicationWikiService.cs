using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationWikiService
    {
        string ExportApplicationsAsWikiText(IEnumerable<ApplicationWikiExportListItem> applications);

        string ExportApplicationDependenciesAsWikiText(IEnumerable<ApplicationDependency> applicationDependencies);

        string ExportDatabaseDependenciesAsWikiText(IEnumerable<DatabaseDependency> databaseDependencies);

        string ExportExternalDependenciesAsWikiText(IEnumerable<ExternalDependency> externalDependencies);

        string ExportNugetPackageDependenciesAsWikiText(IEnumerable<NugetPackageDependency> nugetPackageDependencies);
    }
}