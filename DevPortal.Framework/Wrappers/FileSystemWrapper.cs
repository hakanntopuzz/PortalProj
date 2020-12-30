using DevPortal.Framework.Abstract;
using System;
using System.IO;

namespace DevPortal.Framework.Wrappers
{
    public class FileSystemWrapper : IFileSystem
    {
        public string GetDocumentText(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        public Stream GetDocumentStream(string path)
        {
            return new FileStream(path, FileMode.Open);
        }

        public byte[] GetFileContents(string path)
        {
            return File.ReadAllBytes(path);
        }

        public string[] GetFilePathList(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        public string[] GetFilePathList(string path, string fileType)
        {
            try
            {
                return Directory.GetFiles(path, fileType, SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public DateTime GetFileLastModified(string path)
        {
            var fileInfo = new FileInfo(path);

            return fileInfo.LastWriteTime;
        }

        public long GetFileSizeInByte(string path)
        {
            var fileInfo = new FileInfo(path);

            return fileInfo.Length;
        }

        public decimal GetFileSizeInKiloByte(string path)
        {
            decimal kb = ByteHesaplama(GetFileSizeInByte(path));

            return Math.Ceiling(kb);
        }

        #region format size

        private static decimal ByteHesaplama(decimal bytes)
        {
            return bytes / 1024;
        }

        #endregion

        public DateTime GetFileCreationTime(string path)
        {
            var fileInfo = new FileInfo(path);

            return fileInfo.CreationTime;
        }

        public string GetNameFromPath(string path)
        {
            var splits = path.Replace(@"\", "/").Split('/');

            return splits[splits.Length - 1];
        }

        public bool DoesFileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DoesFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void CopyFile(string sourceFile, string destFile)
        {
            File.Copy(sourceFile, destFile);
        }

        /// <summary>
        /// Verilen klasördeki tüm dosyaları geri dönüşümsüz siler
        /// </summary>
        /// <param name="path">Klasör yolu</param>
        public void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Verilen dosyayı geri dönüşümsüz siler
        /// </summary>
        /// <param name="path">Dosyanın yolu</param>
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void CopyToFilesInFolder(string source, string dest)
        {
            var filesInFolder = new DirectoryInfo(source).GetFiles("*.*");
            foreach (FileInfo dosya in filesInFolder)
            {
                dosya.CopyTo(dest + dosya.Name);
            }
        }

        public string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}