using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationEnvironmentRepository
    {
        ICollection<ApplicationEnvironment> GetApplicationEnvironments(int applicationId);

        ICollection<Environment> GetEnvironmentsDoesNotExistByApplicationId(int applicationId);

        bool AddApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        bool DeleteApplicationEnvironment(int environmentId);

        ApplicationEnvironment GetApplicationEnvironmentById(int applicationEnvironmentId);

        bool UpdateApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        RecordUpdateInfo GetApplicationEnvironmentUpdateInfo(int environmentId);

        ApplicationEnvironment GetApplicationEnvironmentByEnvironmentId(int applicationId, int environmentId);
    }
}