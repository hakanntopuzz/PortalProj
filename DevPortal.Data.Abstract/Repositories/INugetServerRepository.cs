using DevPortal.Model;
using DevPortal.NugetManager.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface INugetServerRepository
    {
        string GetPackages(int skip);

        ICollection<string> GetSubDirectoryFoldersName(string filePath);

        ICollection<NugetPackageFolder> GetNugetPackagesFolder(string filePath);

        ICollection<string> GetFileList(string filePath);

        LogFileModel GetDownloadFileContents(string path);

        LogFileModel GetNuspecTemplate(string path);
    }
}