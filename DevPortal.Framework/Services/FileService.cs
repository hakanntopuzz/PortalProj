using DevPortal.Framework.Abstract;
using System.Collections.Generic;

namespace DevPortal.Framework.Services
{
    public class FileService : IFileService
    {
        public bool FilePathListContainsFile(string fileName, List<string> filePathList)
        {
            if (filePathList == null)
            {
                return false;
            }

            return filePathList.Count > 0 && filePathList.Contains(fileName);
        }

        public int GetFileIndexInFilePathList(List<string> filePathList, string fileName)
        {
            if (filePathList == null)
            {
                return 0;
            }

            for (int i = 0; i < filePathList.Count; i++)
            {
                if (filePathList[i].Contains(fileName))
                {
                    return i;
                }
            }

            return 0;
        }
    }
}