using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IDatabaseDependencyRepository
    {
        DatabaseDependency GetDatabaseDependencyById(int id);

        RecordUpdateInfo GetDatabaseDependencyUpdateInfo(int id);

        bool AddDatabaseDependency(DatabaseDependency databaseDependency);

        bool UpdateDatabaseDependency(DatabaseDependency databaseDependency);

        bool DeleteDatabaseDependency(int databaseDependencyId);

        ICollection<DatabaseDependency> GetDatabaseDependenciesByApplicationId(int applicationId);

        ICollection<DatabaseDependenciesExportListItem> GetFullDatabaseDependenciesByApplicationId(int applicationId);
    }
}