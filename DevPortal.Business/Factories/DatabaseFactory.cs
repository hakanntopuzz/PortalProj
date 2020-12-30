using DevPortal.Business.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Factories
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public ICollection<DatabaseWikiExportListItem> CreateDatabaseWikiExportListItems(ICollection<Database> databases)
        {
            return databases.Select(x =>
                new DatabaseWikiExportListItem
                {
                    Name = x.Name,
                    DatabaseGroupName = x.DatabaseGroupName,
                    DatabaseTypeName = x.DatabaseTypeName,
                    RedmineProjectName = x.RedmineProjectName
                }).ToList();
        }
    }
}