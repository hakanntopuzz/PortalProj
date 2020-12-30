using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IExternalDependencyService
    {
        ExternalDependency GetExternalDependencyById(int id);

        Int32ServiceResult AddExternalDependency(ExternalDependency externalDependency);

        ServiceResult UpdateExternalDependency(ExternalDependency externalDependency);

        StringServiceResult DeleteExternalDependency(ExternalDependency externalDependency);

        ICollection<ExternalDependenciesExportListItem> GetExternalDependencies(int applicationId);
    }
}