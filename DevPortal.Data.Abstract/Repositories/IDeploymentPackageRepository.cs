using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IDeploymentPackageRepository
    {
        ICollection<string> GetSubDirectoryFoldersName(string filePath);

        ICollection<DeploymentPackageFolder> GetDeploymentPackagesFolder(string filePath);
    }
}