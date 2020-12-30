using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseDependencyService
    {
        DatabaseDependency GetDatabaseDependency(int id);

        ServiceResult AddDatabaseDependency(DatabaseDependency databaseDependency);

        ServiceResult UpdateDatabaseDependency(DatabaseDependency databaseDependency);

        ServiceResult DeleteDatabaseDependency(int databaseDependencyId);

        ICollection<DatabaseDependency> GetDatabaseDependenciesByApplicationId(int applicationId);

        ICollection<DatabaseDependenciesExportListItem> GetFullDatabaseDependenciesByApplicationId(int applicationId);
    }
}