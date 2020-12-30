using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IDeploymentPackageService
    {
        ICollection<DeploymentPackageFolder> GetApplicationsByEnvironmentId(int environmentId);

        ICollection<DeploymentPackageFolder> GetDatabaseDeploymentPackages(int environmentId);
    }
}