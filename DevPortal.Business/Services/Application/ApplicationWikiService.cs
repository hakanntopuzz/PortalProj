using DevPortal.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.Business.Services
{
    public class ApplicationWikiService : BaseWikiService, IApplicationWikiService
    {
        #region ctor

        public ApplicationWikiService(IWikiTextService wikiTextService) : base(wikiTextService)
        { }

        #endregion

        #region export application dependencies

        public string ExportApplicationDependenciesAsWikiText(IEnumerable<ApplicationDependency> applicationDependencies)
        {
            if (applicationDependencies == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            AppendHeader(builder, ExportFileContentNames.ApplicationDependenciesHeaderText);

            var headerColumns = new[] { ExportFileContentNames.ApplicationName, ExportFileContentNames.ApplicationGroup };
            AppendTableHeader(builder, headerColumns);

            foreach (var application in applicationDependencies)
            {
                if (HasRedmineProject(application))
                {
                    AppendApplicationDependencyRowWithRedmineProjectLink(builder, application);
                }
                else
                {
                    var rowData = new[] { application.Name, application.ApplicationGroupName };
                    AppendTableRow(builder, rowData);
                }
            }

            return builder.ToString();
        }

        void AppendApplicationDependencyRowWithRedmineProjectLink(StringBuilder builder, ApplicationDependency application)
        {
            var applicationNameWithProjectLink = wikiTextService.GenerateWikiLink(application.RedmineProjectName, application.Name);
            var rowData = new[] { applicationNameWithProjectLink, application.ApplicationGroupName };
            AppendTableRow(builder, rowData);
        }

        #endregion

        #region export database dependencies

        public string ExportDatabaseDependenciesAsWikiText(IEnumerable<DatabaseDependency> databaseDependencies)
        {
            if (databaseDependencies == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            AppendHeader(builder, ExportFileContentNames.DatabaseDependenciesHeaderText);

            var headerColumns = new[] { ExportFileContentNames.DatabaseName, ExportFileContentNames.DatabaseGroup };
            AppendTableHeader(builder, headerColumns);

            foreach (var database in databaseDependencies)
            {
                if (HasRedmineProject(database))
                {
                    AppendDatabaseDependencyRowWithRedmineProjectLink(builder, database);
                }
                else
                {
                    var rowData = new[] { database.Name, database.DatabaseGroupName };
                    AppendTableRow(builder, rowData);
                }
            }

            return builder.ToString();
        }

        void AppendDatabaseDependencyRowWithRedmineProjectLink(StringBuilder builder, DatabaseDependency database)
        {
            var databaseNameWithProjectLink = wikiTextService.GenerateWikiLink(database.RedmineProjectName, database.Name);
            var rowData = new[] { databaseNameWithProjectLink, database.DatabaseGroupName };
            AppendTableRow(builder, rowData);
        }

        #endregion

        #region export external dependencies

        public string ExportExternalDependenciesAsWikiText(IEnumerable<ExternalDependency> externalDependencies)
        {
            if (externalDependencies == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            AppendHeader(builder, ExportFileContentNames.ExternalDependenciesHeaderText);

            var headerColumns = new[] { ExportFileContentNames.DependencyName, ExportFileContentNames.Description };
            AppendTableHeader(builder, headerColumns);

            foreach (var dependency in externalDependencies)
            {
                var rowData = new[] { dependency.Name, dependency.Description };
                AppendTableRow(builder, rowData);
            }

            return builder.ToString();
        }

        #endregion

        #region export nuget package dependencies

        public string ExportNugetPackageDependenciesAsWikiText(IEnumerable<NugetPackageDependency> nugetPackageDependencies)
        {
            if (nugetPackageDependencies == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            AppendHeader(builder, ExportFileContentNames.NugetPackageDependenciesHeaderText);

            var headerColumns = new[] { ExportFileContentNames.NugetPackageName, ExportFileContentNames.NugetPackageUrl };
            AppendTableHeader(builder, headerColumns);

            foreach (var nugetPackage in nugetPackageDependencies)
            {
                var rowData = new[] { nugetPackage.NugetPackageName, nugetPackage.PackageUrl };
                AppendTableRow(builder, rowData);
            }

            return builder.ToString();
        }

        #endregion

        #region has redmine project

        static bool HasRedmineProject(ApplicationDependency application)
        {
            return !string.IsNullOrEmpty(application.RedmineProjectName);
        }

        static bool HasRedmineProject(DatabaseDependency database)
        {
            return !string.IsNullOrEmpty(database.RedmineProjectName);
        }

        static bool HasRedmineProject(ApplicationWikiExportListItem application)
        {
            return !string.IsNullOrEmpty(application.RedmineProjectName);
        }

        #endregion

        #region export applications as wiki text

        public string ExportApplicationsAsWikiText(IEnumerable<ApplicationWikiExportListItem> applications)
        {
            if (applications == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            AppendHeader(builder, ExportFileContentNames.ApplicationListHeaderText);

            var headerColumns = new[] {
                ExportFileContentNames.ApplicationName,
                ExportFileContentNames.ApplicationGroup,
                ExportFileContentNames.ApplicationType,
                ExportFileContentNames.Status
            };
            AppendTableHeader(builder, headerColumns);

            foreach (var application in applications)
            {
                if (HasRedmineProject(application))
                {
                    AppendApplicationWikiExportListItemRowWithRedmineProjectLink(builder, application);
                }
                else
                {
                    var rowData = new[] { application.Name, application.ApplicationGroupName, application.ApplicationTypeName, application.Status };
                    AppendTableRow(builder, rowData);
                }
            }

            return builder.ToString();
        }

        void AppendApplicationWikiExportListItemRowWithRedmineProjectLink(StringBuilder builder, ApplicationWikiExportListItem application)
        {
            var applicationNameWithProjectLink = wikiTextService.GenerateWikiLink(application.RedmineProjectName, application.Name);
            var rowData = new[] { applicationNameWithProjectLink, application.ApplicationGroupName, application.ApplicationTypeName, application.Status };
            AppendTableRow(builder, rowData);
        }

        #endregion
    }
}