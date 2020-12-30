using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Text;

namespace DevPortal.Business.Services
{
    public class ApplicationDependencyExportService : IApplicationDependencyExportService
    {
        #region ctor

        readonly IApplicationDependencyReaderService applicationDependencyReaderService;

        readonly ICsvService csvService;

        readonly IApplicationWikiService applicationWikiExportService;

        readonly IDatabaseDependencyService databaseDependencyService;

        readonly IExternalDependencyService externalDependencyService;

        public ApplicationDependencyExportService(
            IApplicationDependencyReaderService applicationDependencyReaderService,
            ICsvService csvService,
            IApplicationWikiService applicationWikiExportService,
            IDatabaseDependencyService databaseDependencyService,
            IExternalDependencyService externalDependencyService)
        {
            this.applicationDependencyReaderService = applicationDependencyReaderService;
            this.csvService = csvService;
            this.applicationWikiExportService = applicationWikiExportService;
            this.databaseDependencyService = databaseDependencyService;
            this.externalDependencyService = externalDependencyService;
        }

        #endregion

        #region export application dependencies as csv

        public CsvServiceResult ExportApplicationDependenciesAsCsv(int applicationId)
        {
            var dependencies = applicationDependencyReaderService.GetApplicationDependenciesExportList(applicationId);
            var encodedBytes = csvService.ExportToCsv(dependencies, CsvColumnNames.ApplicationDependenciesList);

            return CsvServiceResult.Success(encodedBytes);
        }

        #endregion

        #region export database dependencies as csv

        public CsvServiceResult ExportDatabaseDependenciesAsCsv(int applicationId)
        {
            var dependencies = applicationDependencyReaderService.GetDatabaseDependenciesExportList(applicationId);
            var encodedBytes = csvService.ExportToCsv(dependencies, CsvColumnNames.DatabaseDependenciesList);

            return CsvServiceResult.Success(encodedBytes);
        }

        #endregion

        #region export external dependencies as csv

        public CsvServiceResult ExportExternalDependenciesAsCsv(int applicationId)
        {
            var dependencies = externalDependencyService.GetExternalDependencies(applicationId);
            var encodedBytes = csvService.ExportToCsv(dependencies, CsvColumnNames.ExternalDependenciesList);

            return CsvServiceResult.Success(encodedBytes);
        }

        #endregion

        #region export nuget package dependencies as csv

        public CsvServiceResult ExportNugetPackageDependenciesAsCsv(int applicationId)
        {
            var dependencies = applicationDependencyReaderService.GetNugetPackageDependenciesExportList(applicationId);
            var encodedBytes = csvService.ExportToCsv(dependencies, CsvColumnNames.NugetPackageDependenciesList);

            return CsvServiceResult.Success(encodedBytes);
        }

        #endregion

        #region export application dependencies as wiki text

        public string ExportApplicationDependenciesAsWikiText(int applicationId)
        {
            return GetApplicationDependenciesAsWikiText(applicationId);
        }

        string GetApplicationDependenciesAsWikiText(int applicationId)
        {
            var applicationDependencies = applicationDependencyReaderService.GetApplicationDependencies(applicationId);
            var databaseDependencies = databaseDependencyService.GetDatabaseDependenciesByApplicationId(applicationId);
            var externalDependencies = applicationDependencyReaderService.GetExternalDependencies(applicationId);
            var nugetPackageDependencies = applicationDependencyReaderService.GetNugetPackageDependencies(applicationId);

            var applicationDependenciesWikiText = applicationWikiExportService.ExportApplicationDependenciesAsWikiText(applicationDependencies);
            var databaseDependenciesWikiText = applicationWikiExportService.ExportDatabaseDependenciesAsWikiText(databaseDependencies);
            var externalDependenciesWikiText = applicationWikiExportService.ExportExternalDependenciesAsWikiText(externalDependencies);
            var nugetPackageDependenciesWikiText = applicationWikiExportService.ExportNugetPackageDependenciesAsWikiText(nugetPackageDependencies);

            var builder = new StringBuilder();
            builder.Append(applicationDependenciesWikiText);
            builder.Append("\r\n");
            builder.Append(databaseDependenciesWikiText);
            builder.Append("\r\n");
            builder.Append(externalDependenciesWikiText);
            builder.Append("\r\n");
            builder.Append(nugetPackageDependenciesWikiText);

            return builder.ToString();
        }

        #endregion

        #region export application dependencies as wiki text file

        public FileExportData ExportApplicationDependenciesAsWikiTextFile(int applicationId)
        {
            string wikiText = GetApplicationDependenciesAsWikiText(applicationId);

            return new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(wikiText),
                FileDownloadName = $"uygulama-bagimliliklari-wiki-{applicationId}.txt",
                ContentType = ContentTypes.Txt
            };
        }

        #endregion
    }
}