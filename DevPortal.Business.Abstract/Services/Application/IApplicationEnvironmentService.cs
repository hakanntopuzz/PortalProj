using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationEnvironmentService
    {
        ICollection<ApplicationEnvironment> GetApplicationEnvironments(int applicationId);

        ICollection<Environment> GetEnvironmentsDoesNotExist(int applicationId);

        ServiceResult AddApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        ApplicationEnvironment GetApplicationEnvironment(int environmentId);

        ICollection<ApplicationEnvironment> GetApplicationEnvironmentsHasLog(int applicationId);

        ServiceResult UpdateApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        ServiceResult DeleteApplicationEnvironment(int environmentId);
    }
}