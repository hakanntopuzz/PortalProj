using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseWikiExportService
    {
        string ExportDatabasesAsWikiText(IEnumerable<DatabaseWikiExportListItem> databases);
    }
}