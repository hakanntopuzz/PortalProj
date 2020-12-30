using System.Collections.Generic;

namespace DevPortal.Framework.Abstract
{
    public interface IFileService
    {
        bool FilePathListContainsFile(string fileName, List<string> filePathList);

        int GetFileIndexInFilePathList(List<string> filePathList, string fileName);
    }
}