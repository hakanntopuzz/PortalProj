using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DevPortal.Business
{
    public class FileSystemService : BaseService, IFileSystemService
    {
        #region ctor

        readonly IFileSystem fileSystemWrapper;

        readonly ILoggingService loggingService;

        readonly IFileSystemFactory fileSystemFactory;

        readonly ICommandExecutorService commandExecutorService;

        public FileSystemService(
            IFileSystem fileSystemWrapper,
            ILoggingService loggingService,
            IFileSystemFactory fileSystemFactory,
             ICommandExecutorService commandExecutorService)
        {
            this.fileSystemWrapper = fileSystemWrapper;
            this.loggingService = loggingService;
            this.fileSystemFactory = fileSystemFactory;
            this.commandExecutorService = commandExecutorService;
        }

        #endregion

        public LogFileModel GetDownloadFileContents(string path)
        {
            try
            {
                var fileContent = fileSystemWrapper.GetFileContents(path);

                return fileSystemFactory.GetDownloadFileContents(path, fileContent);
            }
            catch (Exception ex)
            {
                loggingService.LogError(SetMethodNameForLogMessage(nameof(GetDownloadFileContents)), Resources.Resources.Messages.DosyaOkunamadi, ex);

                return null;
            }
        }

        public LogFileModel GetFileContent(string path)
        {
            try
            {
                var datetime = fileSystemWrapper.GetFileLastModified(path);
                var text = fileSystemWrapper.GetDocumentText(path);
                var size = fileSystemWrapper.GetFileSizeInKiloByte(path).ToString();

                return fileSystemFactory.CreateLogFile(path, datetime, size, text);
            }
            catch (Exception ex)
            {
                loggingService.LogError(SetMethodNameForLogMessage(nameof(GetFileContent)), Resources.Resources.Messages.DosyaOkunamadi, ex);

                return null;
            }
        }

        //Not:ismi belki değişebilir.
        public Collection<LogFileModel> GetFilePathGenericList(string path)
        {
            try
            {
                var logFiles = fileSystemFactory.CreateLogFiles();
                foreach (var item in fileSystemWrapper.GetFilePathList(path))
                {
                    var datetime = fileSystemWrapper.GetFileLastModified(item);
                    var size = fileSystemWrapper.GetFileSizeInKiloByte(item).ToString();
                    logFiles.Add(fileSystemFactory.CreateLogFile(item, datetime, size));
                }

                return logFiles;
            }
            catch (Exception ex)
            {
                loggingService.LogError(SetMethodNameForLogMessage(nameof(GetFilePathGenericList)), Resources.Resources.Messages.DosyaOkunamadi, ex);

                return fileSystemFactory.CreateLogFiles();
            }
        }

        public bool ExecutePowerShellScript(string path, string destPath, string fileName)
        {
            try
            {
                commandExecutorService.ExecutePowerShellScript(path, destPath, fileName);

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(ExecutePowerShellScript), $"PowerShell Scripti çalıştırılırken sorun oluştu.", ex);

                return false;
            }
        }

        public bool MultiCopyFile(List<string> sourceFile, List<string> destFile)
        {
            try
            {
                for (int i = 0; i < destFile.Count; i++)
                {
                    if (!DoesFileExists(destFile[i]))
                    {
                        fileSystemWrapper.CopyFile(sourceFile[i], destFile[i]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(MultiCopyFile), $"Geçici PowerShell Script dosyaları kopyalanamadı.", ex);

                return false;
            }
        }

        public bool MultiDeleteFile(List<string> destFile)
        {
            try
            {
                for (int i = 0; i < destFile.Count; i++)
                {
                    fileSystemWrapper.DeleteFile(destFile[i]);
                }

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(MultiDeleteFile), $"Geçici PowerShell Script dosyaları silinemedi.", ex);

                return false;
            }
        }

        public bool DoesFileExists(string file)
        {
            try
            {
                return fileSystemWrapper.DoesFileExists(file);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(DoesFileExists), $"{file} dosyası arama işleminde beklenmeyen bir hata oluştu.", ex);

                return false;
            }
        }

        public bool DoesFolderExists(string file)
        {
            try
            {
                return fileSystemWrapper.DoesFolderExists(file);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(DoesFolderExists), $"{file} Klasör arama işleminde beklenmeyen bir hata oluştu.", ex);

                return false;
            }
        }

        public bool DeleteFolder(string path)
        {
            try
            {
                fileSystemWrapper.DeleteFolder(path);

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(DeleteFolder), $"{path} Klasör silinemedi.", ex);

                return false;
            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                fileSystemWrapper.DeleteFile(path);

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(DeleteFile), $"{path} Dosya silinemedi.", ex);

                return false;
            }
        }

        public bool CreateFolder(string path)
        {
            try
            {
                fileSystemWrapper.CreateFolder(path);

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(CreateFolder), $"{path} yolunda klasör oluşturulamadı.", ex);

                return false;
            }
        }

        public bool CopyFile(string sourceFile, string destFile)
        {
            try
            {
                fileSystemWrapper.CopyFile(sourceFile, destFile);

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(CopyFile), $"{sourceFile} dosyası kopyalanamadı.", ex);

                return false;
            }
        }

        public bool MoveFile(string sourceFile, string destFile)
        {
            try
            {
                fileSystemWrapper.CopyToFilesInFolder(sourceFile, destFile);

                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(MoveFile), $"{sourceFile} dosyası taşınamadı.", ex);

                return false;
            }
        }

        public ICollection<string> GetFilePathList(string path, string fileType)
        {
            try
            {
                return fileSystemWrapper.GetFilePathList(path, fileType).ToList();
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetFilePathList), $"{path} yolunda {fileType} dosya biçimi bulunamadı.", ex);

                return fileSystemFactory.CreateEmptyStringList();
            }
        }

        public ICollection<string> GetDirectories(string path)
        {
            try
            {
                return fileSystemWrapper.GetDirectories(path);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetDirectories), $"{path} yolunda klasörler getirilirken sorun oluştu.", ex);

                return fileSystemFactory.CreateEmptyString();
            }
        }

        public ICollection<string> GetFilePathList(string path)
        {
            try
            {
                return fileSystemWrapper.GetFilePathList(path);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetFilePathList), $"{path} yolunda path listesi getirilirken sorun oluştu.", ex);

                return fileSystemFactory.CreateEmptyString();
            }
        }

        public string PathCombine(string path1, string path2)
        {
            try
            {
                return fileSystemWrapper.PathCombine(path1, path2);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(PathCombine), $"Verilen parametreleri combine işleminde sorun oluştu.", ex);

                return string.Empty;
            }
        }
    }
}