using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IDeploymentPackageFactory
    {
        List<DeploymentPackageFolder> CreateDeploymentPackageFolders(string[] directory);

        List<string> GetSubDirectoryFoldersName(string[] directory);

        List<DeploymentPackageFolder> CreateDeploymentPackageFolders();

        List<string> CreateEmptyStringList();
    }
}