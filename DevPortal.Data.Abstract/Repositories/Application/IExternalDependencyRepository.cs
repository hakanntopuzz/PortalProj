using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IExternalDependencyRepository
    {
        ExternalDependency GetExternalDependencyById(int id);

        RecordUpdateInfo GetExternalDependencyUpdateInfo(int id);

        int AddExternalDependency(ExternalDependency externalDependency);

        bool UpdateExternalDependency(ExternalDependency externalDependency);

        bool DeleteExternalDependency(ExternalDependency externalDependency);

        ICollection<ExternalDependenciesExportListItem> GetExternalDependencies(int applicationId);
    }
}