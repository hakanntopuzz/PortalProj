using DevPortal.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.Business.Services
{
    public class DatabaseWikiExportService : IDatabaseWikiExportService
    {
        #region ctor

        readonly IWikiTextService wikiTextService;

        public DatabaseWikiExportService(IWikiTextService wikiTextService)
        {
            this.wikiTextService = wikiTextService;
        }

        #endregion

        public string ExportDatabasesAsWikiText(IEnumerable<DatabaseWikiExportListItem> databases)
        {
            if (databases == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            AppendHeader(builder, ExportFileContentNames.DatabaseListHeaderText);

            var headerColumns = new[] {
                ExportFileContentNames.DatabaseName,
                ExportFileContentNames.DatabaseGroup,
                ExportFileContentNames.DatabaseType };
            AppendTableHeader(builder, headerColumns);

            foreach (var database in databases)
            {
                if (HasRedmineProject(database))
                {
                    AppendDatabaseWikiExportListItemRowWithRedmineProjectLink(builder, database);
                }
                else
                {
                    var rowData = new[] { database.Name, database.DatabaseGroupName, database.DatabaseTypeName };
                    AppendTableRow(builder, rowData);
                }
            }

            return builder.ToString();
        }

        static bool HasRedmineProject(DatabaseWikiExportListItem database)
        {
            return !string.IsNullOrEmpty(database.RedmineProjectName);
        }

        void AppendDatabaseWikiExportListItemRowWithRedmineProjectLink(StringBuilder builder, DatabaseWikiExportListItem database)
        {
            var databaseNameWithProjectLink = wikiTextService.GenerateWikiLink(database.RedmineProjectName, database.Name);
            var rowData = new[] { databaseNameWithProjectLink, database.DatabaseGroupName, database.DatabaseTypeName };
            AppendTableRow(builder, rowData);
        }

        void AppendHeader(StringBuilder builder, string headerText)
        {
            var header = wikiTextService.GenerateH2(headerText);
            builder.Append(header);
        }

        void AppendTableHeader(StringBuilder builder, string[] columns)
        {
            var headerRow = wikiTextService.GenerateTableHeader(columns);
            builder.Append(headerRow);
        }

        void AppendTableRow(StringBuilder builder, string[] rowData)
        {
            var row = wikiTextService.GenerateTableRow(rowData);
            builder.Append(row);
        }
    }
}