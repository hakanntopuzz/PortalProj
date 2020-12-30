using System;
using System.IO;

namespace DevPortal.Framework.Abstract
{
    public interface IFileSystem
    {
        string[] GetFilePathList(string path);

        string[] GetFilePathList(string path, string fileType);

        string GetDocumentText(string path);

        long GetFileSizeInByte(string path);

        decimal GetFileSizeInKiloByte(string path);

        DateTime GetFileLastModified(string path);

        Stream GetDocumentStream(string path);

        byte[] GetFileContents(string path);

        DateTime GetFileCreationTime(string path);

        string[] GetDirectories(string path);

        string GetNameFromPath(string path);

        bool DoesFileExists(string path);

        void CreateFolder(string path);

        void CopyFile(string sourceFile, string destFile);

        void DeleteFolder(string path);

        bool DoesFolderExists(string path);

        void CopyToFilesInFolder(string source, string dest);

        void DeleteFile(string path);

        string PathCombine(string path1, string path2);
    }
}