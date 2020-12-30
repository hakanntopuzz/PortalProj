using DevPortal.Business.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class ApplicationCsvExportService : IApplicationCsvExportService
    {
        #region ctor

        readonly ICsvService csvService;

        readonly IApplicationFactory applicationFactory;

        readonly IApplicationReaderService applicationReaderService;

        public ApplicationCsvExportService(
            ICsvService applicationCsvService,
            IApplicationFactory applicationFactory,
            IApplicationReaderService applicationReaderService)
        {
            this.csvService = applicationCsvService;
            this.applicationFactory = applicationFactory;
            this.applicationReaderService = applicationReaderService;
        }

        #endregion

        #region export applications as csv

        public CsvServiceResult ExportApplicationsAsCsv(int applicationGroupId, string applicationName)
        {
            var applications = FilterApplicationsForCsv(applicationGroupId, applicationName);
            var encodedBytes = csvService.ExportToCsv(applications, CsvColumnNames.ApplicationList);

            return CsvServiceResult.Success(encodedBytes);
        }

        ICollection<ApplicationExportListItem> FilterApplicationsForCsv(int applicationGroupId, string applicationName)
        {
            var filterApplications = applicationReaderService.FilterApplications(applicationGroupId, applicationName);

            return applicationFactory.CreateApplicationExportListItems(filterApplications);
        }

        #endregion
    }
}