using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.NugetManager.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Data
{
    public class NugetServerFactory : INugetServerFactory
    {
        readonly IFileSystem fileSystem;

        public NugetServerFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public List<NugetPackageFolder> CreateNugetPackageFolders(List<string> directory)
        {
            return directory.Select(item => new NugetPackageFolder
            {
                Path = item,
                DateModified = fileSystem.GetFileLastModified(item),
                CreationTime = fileSystem.GetFileCreationTime(item)
            }).ToList();
        }

        public List<NugetPackageFolder> CreateNugetPackageFolders()
        {
            return new List<NugetPackageFolder>();
        }

        public List<string> GetSubDirectoryFoldersName(List<string> directory)
        {
            return directory.Select(item => fileSystem.GetNameFromPath(item)).ToList();
        }

        public List<string> CreateEmptyStringList()
        {
            return new List<string>();
        }
    }
}