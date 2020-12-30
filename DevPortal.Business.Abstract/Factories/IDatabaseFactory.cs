using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseFactory
    {
        ICollection<DatabaseWikiExportListItem> CreateDatabaseWikiExportListItems(ICollection<Database> databases);
    }
}