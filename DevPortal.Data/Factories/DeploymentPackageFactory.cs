using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Data
{
    public class DeploymentPackageFactory : IDeploymentPackageFactory
    {
        readonly IFileSystem fileSystem;

        public DeploymentPackageFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public List<DeploymentPackageFolder> CreateDeploymentPackageFolders(string[] directory)
        {
            return directory.Select(item => new DeploymentPackageFolder
            {
                Path = item,
                DateModified = fileSystem.GetFileLastModified(item),
                CreationTime = fileSystem.GetFileCreationTime(item)
            }).ToList();
        }

        public List<DeploymentPackageFolder> CreateDeploymentPackageFolders()
        {
            return new List<DeploymentPackageFolder>();
        }

        public List<string> GetSubDirectoryFoldersName(string[] directory)
        {
            return directory.Select(item => fileSystem.GetNameFromPath(item)).ToList();
        }

        public List<string> CreateEmptyStringList()
        {
            return new List<string>();
        }
    }
}