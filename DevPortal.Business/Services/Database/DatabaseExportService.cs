using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevPortal.Business.Services
{
    public class DatabaseExportService : IDatabaseExportService
    {
        #region ctor

        readonly IDatabaseReaderService databaseReaderService;

        readonly IDatabaseFactory databaseFactory;

        readonly ICsvService csvService;

        readonly IDatabaseWikiExportService databaseWikiExportService;

        public DatabaseExportService(
            IDatabaseReaderService databaseReaderService,
            IDatabaseFactory databaseFactory,
            ICsvService csvService,
            IDatabaseWikiExportService databaseWikiExportService
        )
        {
            this.databaseReaderService = databaseReaderService;
            this.databaseFactory = databaseFactory;
            this.csvService = csvService;
            this.databaseWikiExportService = databaseWikiExportService;
        }

        #endregion

        #region export databases as csv

        public CsvServiceResult ExportDatabasesAsCsv(int databaseGroupId, string databaseName)
        {
            var databases = FilterDatabasesForCsv(databaseGroupId, databaseName);
            var encodedBytes = csvService.ExportToCsv(databases, CsvColumnNames.DatabaseList);

            return CsvServiceResult.Success(encodedBytes);
        }

        ICollection<DatabaseExportListItem> FilterDatabasesForCsv(int databaseGroupId, string databaseName)
        {
            var filterDatabases = databaseReaderService.FilterDatabases(databaseGroupId, databaseName);

            return filterDatabases.Select(x =>
                new DatabaseExportListItem
                {
                    Name = x.Name,
                    DatabaseGroupName = x.DatabaseGroupName,
                    DatabaseTypeName = x.DatabaseTypeName
                }).ToList();
        }

        #endregion

        public string ExportDatabasesAsWikiText(int databaseGroupId, string databaseName)
        {
            var filterDatabases = FilterDatabasesForWiki(databaseGroupId, databaseName);

            return databaseWikiExportService.ExportDatabasesAsWikiText(filterDatabases);
        }

        ICollection<DatabaseWikiExportListItem> FilterDatabasesForWiki(int databaseGroupId, string databaseName)
        {
            var filterDatabases = databaseReaderService.FilterDatabases(databaseGroupId, databaseName);

            return databaseFactory.CreateDatabaseWikiExportListItems(filterDatabases);
        }

        public FileExportData ExportDatabasesAsWikiTextFile(int databaseGroupId, string databaseName)
        {
            string wikiText = ExportDatabasesAsWikiText(databaseGroupId, databaseName);

            return new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(wikiText),
                FileDownloadName = $"veritabani-listesi-wiki.txt",
                ContentType = ContentTypes.Txt
            };
        }
    }
}