using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IEnvironmentService
    {
        ICollection<Environment> GetEnvironments();

        Environment GetEnvironment(int id);

        ServiceResult AddEnvironment(Environment environment);

        ServiceResult UpdateEnvironment(Environment environment);

        ServiceResult DeleteEnvironment(int environmentId);
    }
}