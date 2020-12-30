using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IEnvironmentRepository
    {
        ICollection<Environment> GetEnvironments();

        Environment GetEnvironmentById(int id);

        Environment GetEnvironmentByName(string name);

        int AddEnvironment(Environment environment);

        RecordUpdateInfo GetEnvironmentUpdateInfo(int id);

        bool UpdateEnvironment(Environment environment);

        bool DeleteEnvironment(int environmentId);

        int GetApplicationEnvironmentCountByEnvironmentId(int environmentId);
    }
}