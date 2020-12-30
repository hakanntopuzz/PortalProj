using DevPortal.NugetManager.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface INugetServerFactory
    {
        List<NugetPackageFolder> CreateNugetPackageFolders(List<string> directory);

        List<string> GetSubDirectoryFoldersName(List<string> directory);

        List<NugetPackageFolder> CreateNugetPackageFolders();

        List<string> CreateEmptyStringList();
    }
}