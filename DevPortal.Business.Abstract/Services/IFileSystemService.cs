using DevPortal.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevPortal.Business.Abstract
{
    public interface IFileSystemService
    {
        LogFileModel GetDownloadFileContents(string path);

        LogFileModel GetFileContent(string path);

        Collection<LogFileModel> GetFilePathGenericList(string path);

        bool MultiCopyFile(List<string> sourceFile, List<string> destFile);

        bool ExecutePowerShellScript(string path, string destPath, string fileName);

        bool MultiDeleteFile(List<string> destFile);

        bool DoesFileExists(string file);

        bool DoesFolderExists(string file);

        bool DeleteFolder(string path);

        bool DeleteFile(string path);

        bool CreateFolder(string path);

        bool CopyFile(string sourceFile, string destFile);

        bool MoveFile(string sourceFile, string destFile);

        ICollection<string> GetFilePathList(string path, string fileType);

        ICollection<string> GetDirectories(string path);

        ICollection<string> GetFilePathList(string path);

        string PathCombine(string path1, string path2);
    }
}