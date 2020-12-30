using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FileSystemServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IFileSystem> fileSystemWrapper;

        StrictMock<ILoggingService> loggingService;

        StrictMock<IFileSystemFactory> fileSystemFactory;

        StrictMock<ICommandExecutorService> commandExecutorService;

        FileSystemService service;

        [SetUp]
        public void Initialize()
        {
            fileSystemWrapper = new StrictMock<IFileSystem>();
            loggingService = new StrictMock<ILoggingService>();
            fileSystemFactory = new StrictMock<IFileSystemFactory>();
            commandExecutorService = new StrictMock<ICommandExecutorService>();
            service = new FileSystemService(
                fileSystemWrapper.Object,
                loggingService.Object,
                fileSystemFactory.Object,
                commandExecutorService.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            fileSystemWrapper.VerifyAll();
            loggingService.VerifyAll();
            fileSystemFactory.VerifyAll();
            commandExecutorService.VerifyAll();
        }

        #endregion

        #region AddApplicationEnvironment

        [Test]
        public void GetDownloadFileContents_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            byte[] fileContent = new byte[0];
            LogFileModel fileModel = new LogFileModel();
            fileSystemWrapper.Setup(x => x.GetFileContents(path)).Returns(fileContent);
            fileSystemFactory.Setup(x => x.GetDownloadFileContents(path, fileContent)).Returns(fileModel);

            // Act
            var result = service.GetDownloadFileContents(path);

            // Assert
            result.Should().Be(fileModel);
        }

        [Test]
        public void GetDownloadFileContents_Throws_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            byte[] fileContent = new byte[0];
            LogFileModel fileModel = new LogFileModel();
            Exception exception = new Exception();
            fileSystemWrapper.Setup(x => x.GetFileContents(path)).Returns(fileContent);
            fileSystemFactory.Setup(x => x.GetDownloadFileContents(path, fileContent)).Throws(exception);
            loggingService.Setup(x => x.LogError("FileSystemService.GetDownloadFileContents", "Dosya içeriği okunamadı.", exception));

            // Act
            var result = service.GetDownloadFileContents(path);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetFileContent

        [Test]
        public void GetFileContent_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            byte[] fileContent = new byte[0];

            var dateTime = DateTime.Now;
            var text = "text";
            var size = 123;
            LogFileModel fileModel = new LogFileModel { };
            fileSystemWrapper.Setup(x => x.GetFileLastModified(path)).Returns(dateTime);
            fileSystemWrapper.Setup(x => x.GetDocumentText(path)).Returns(text);
            fileSystemWrapper.Setup(x => x.GetFileSizeInKiloByte(path)).Returns(size);
            fileSystemFactory.Setup(x => x.CreateLogFile(path, dateTime, size.ToString(), text)).Returns(fileModel);

            // Act
            var result = service.GetFileContent(path);

            // Assert
            result.Should().Be(fileModel);
        }

        [Test]
        public void GetFileContent_Throws_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            byte[] fileContent = new byte[0];
            LogFileModel fileModel = new LogFileModel();
            Exception exception = new Exception();
            var dateTime = DateTime.Now;
            var creationTime = DateTime.Now;
            fileSystemWrapper.Setup(x => x.GetFileLastModified(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("FileSystemService.GetFileContent", Resources.Resources.Messages.DosyaOkunamadi, exception));

            // Act
            var result = service.GetFileContent(path);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetFilePathGenericList

        [Test]
        public void GetFilePathGenericList_NoCondition_ReturnFileModellist()
        {
            // Arrange
            var path = "path";
            string[] pathlist = new string[1] { path };
            var dateTime = DateTime.Now;
            var size = 123;

            LogFileModel fileModel = new LogFileModel { Path = path };
            Collection<LogFileModel> filelist = new Collection<LogFileModel> { fileModel };
            fileSystemFactory.Setup(x => x.CreateLogFiles()).Returns(filelist);
            fileSystemWrapper.Setup(x => x.GetFilePathList(path)).Returns(pathlist);
            fileSystemWrapper.Setup(x => x.GetFileLastModified(path)).Returns(dateTime);
            fileSystemWrapper.Setup(x => x.GetFileSizeInKiloByte(path)).Returns(size);
            fileSystemFactory.Setup(x => x.CreateLogFile(path, dateTime, size.ToString())).Returns(fileModel);

            // Act
            var result = service.GetFilePathGenericList(path);

            // Assert
            result.Count.Should().Be(filelist.Count);
        }

        [Test]
        public void GetFilePathGenericList_Throws_ReturnFileModelList()
        {
            // Arrange
            var path = "path";
            byte[] fileContent = new byte[0];
            LogFileModel fileModel = new LogFileModel();
            Exception exception = new Exception();
            var collection = new Collection<LogFileModel>();
            var dateTime = DateTime.Now;
            var creationTime = DateTime.Now;
            fileSystemFactory.Setup(x => x.CreateLogFiles()).Returns(collection);
            fileSystemWrapper.Setup(x => x.GetFilePathList(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("FileSystemService.GetFilePathGenericList", Resources.Resources.Messages.DosyaOkunamadi, exception));

            // Act
            var result = service.GetFilePathGenericList(path);

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region CreateFolder

        [Test]
        public void CreateFolder_NoCondition_ReturnTrue()
        {
            // Arrange
            var path = "path";

            fileSystemWrapper.Setup(x => x.CreateFolder(path));

            // Act
            var result = service.CreateFolder(path);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CreateFolder_NoCondition_ReturnFalse()
        {
            // Arrange
            var path = "path";
            var exception = new Exception();

            fileSystemWrapper.Setup(x => x.CreateFolder(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("CreateFolder", $"{path} yolunda klasör oluşturulamadı.", exception));

            // Act
            var result = service.CreateFolder(path);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region CopyFile

        [Test]
        public void CopyFile_NoCondition_ReturnTrue()
        {
            // Arrange
            var sourceFile = "sourceFile";
            var destFile = "destFile";

            fileSystemWrapper.Setup(x => x.CopyFile(sourceFile, destFile));

            // Act
            var result = service.CopyFile(sourceFile, destFile);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CopyFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var sourceFile = "sourceFile";
            var destFile = "destFile";
            var exception = new Exception();

            fileSystemWrapper.Setup(x => x.CopyFile(sourceFile, destFile)).Throws(exception);
            loggingService.Setup(x => x.LogError("CopyFile", $"{sourceFile} dosyası kopyalanamadı.", exception));

            // Act
            var result = service.CopyFile(sourceFile, destFile);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DeleteFolder

        [Test]
        public void DeleteFolder_NoCondition_ReturnTrue()
        {
            // Arrange

            var path = "path";
            fileSystemWrapper.Setup(x => x.DeleteFolder(path));

            // Act
            var result = service.DeleteFolder(path);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void DeleteFolder_NoCondition_ReturnFalse()
        {
            // Arrange
            var path = "path";
            var exception = new Exception();
            fileSystemWrapper.Setup(x => x.DeleteFolder(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("DeleteFolder", $"{path} Klasör silinemedi.", exception));

            // Act
            var result = service.DeleteFolder(path);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DoesFolderExists

        [Test]
        public void DoesFolderExists_NoCondition_ReturnTrue()
        {
            // Arrange

            var file = "file";
            fileSystemWrapper.Setup(x => x.DoesFolderExists(file)).Returns(true);

            // Act
            var result = service.DoesFolderExists(file);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void DoesFolderExists_NoCondition_ReturnFalse()
        {
            // Arrange
            var file = "file";
            var exception = new Exception();
            fileSystemWrapper.Setup(x => x.DoesFolderExists(file)).Throws(exception);
            loggingService.Setup(x => x.LogError("DoesFolderExists", $"{file} Klasör arama işleminde beklenmeyen bir hata oluştu.", exception));

            // Act
            var result = service.DoesFolderExists(file);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region MoveFile

        [Test]
        public void MoveFile_NoCondition_ReturnTrue()
        {
            // Arrange
            var sourceFile = "sourceFile";
            var destFile = "destFile";

            fileSystemWrapper.Setup(x => x.CopyToFilesInFolder(sourceFile, destFile));

            // Act
            var result = service.MoveFile(sourceFile, destFile);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MoveFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var sourceFile = "sourceFile";
            var destFile = "destFile";
            var exception = new Exception();

            fileSystemWrapper.Setup(x => x.CopyToFilesInFolder(sourceFile, destFile)).Throws(exception);
            loggingService.Setup(x => x.LogError("MoveFile", $"{sourceFile} dosyası taşınamadı.", exception));

            // Act
            var result = service.MoveFile(sourceFile, destFile);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DoesFileExists

        [Test]
        public void DoesFileExists_NoCondition_ReturnTrue()
        {
            // Arrange
            var file = "file";

            fileSystemWrapper.Setup(x => x.DoesFileExists(file)).Returns(true);

            // Act
            var result = service.DoesFileExists(file);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void DoesFileExists_NoCondition_ReturnFalse()
        {
            // Arrange
            var file = "file";

            fileSystemWrapper.Setup(x => x.DoesFileExists(file)).Returns(false);

            // Act
            var result = service.DoesFileExists(file);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void DoesFileExists_NoCondition_ReturnFalseAndThrowErrorMessage()
        {
            // Arrange
            var file = "file";
            var exception = new Exception();

            fileSystemWrapper.Setup(x => x.DoesFileExists(file)).Throws(exception);
            loggingService.Setup(x => x.LogError("DoesFileExists", $"{file} dosyası arama işleminde beklenmeyen bir hata oluştu.", exception));

            // Act
            var result = service.DoesFileExists(file);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region ExecutePowerShellScript

        [Test]
        public void ExecutePowerShellScript_NoCondition_ReturnTrue()
        {
            // Arrange
            var path = "path";
            var destPath = "destPath";
            var fileName = "fileName";

            commandExecutorService.Setup(x => x.ExecutePowerShellScript(path, destPath, fileName));

            // Act
            var result = service.ExecutePowerShellScript(path, destPath, fileName);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ExecutePowerShellScript_NoCondition_ReturnFalse()
        {
            // Arrange
            var path = "path";
            var destPath = "destPath";
            var fileName = "fileName";
            Exception exception = new Exception();

            commandExecutorService.Setup(x => x.ExecutePowerShellScript(path, destPath, fileName)).Throws(exception);
            loggingService.Setup(x => x.LogError("ExecutePowerShellScript", $"PowerShell Scripti çalıştırılırken sorun oluştu.", exception));

            // Act
            var result = service.ExecutePowerShellScript(path, destPath, fileName);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region MultiCopyFile

        [Test]
        public void MultiCopyFile_DoesFileExists_ReturnTrue()
        {
            // Arrange
            var file = "file";
            var sourceFile = new List<string> {
                 "file"
            };
            var destFile = new List<string> {
            "file"
            };

            fileSystemWrapper.Setup(x => x.DoesFileExists(file)).Returns(true);

            // Act
            var result = service.MultiCopyFile(sourceFile, destFile);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MultiCopyFile_DoesNotFileExists_ReturnTrue()
        {
            // Arrange
            var file = "file";
            var sourceFile = new List<string> {
                 "file"
            };
            var destFile = new List<string> {
                "file"
            };
            Exception exception = new Exception();
            var i = 0;

            fileSystemWrapper.Setup(x => x.DoesFileExists(file)).Throws(exception);
            loggingService.Setup(x => x.LogError("DoesFileExists", $"{file} dosyası arama işleminde beklenmeyen bir hata oluştu.", exception));
            fileSystemWrapper.Setup(x => x.CopyFile(sourceFile[i], destFile[i]));

            // Act
            var result = service.MultiCopyFile(sourceFile, destFile);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MultiCopyFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var file = "file";
            var sourceFile = new List<string> {
                 "file"
            };
            var destFile = new List<string> {
                "file"
            };
            Exception exception = new Exception();
            var i = 0;

            fileSystemWrapper.Setup(x => x.DoesFileExists(file)).Throws(exception);
            loggingService.Setup(x => x.LogError("DoesFileExists", $"{file} dosyası arama işleminde beklenmeyen bir hata oluştu.", exception));
            fileSystemWrapper.Setup(x => x.CopyFile(sourceFile[i], destFile[i])).Throws(exception);
            loggingService.Setup(x => x.LogError("MultiCopyFile", $"Geçici PowerShell Script dosyaları kopyalanamadı.", exception));

            // Act
            var result = service.MultiCopyFile(sourceFile, destFile);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region MultiDeleteFile

        [Test]
        public void MultiDeleteFile_NoCondition_ReturnTrue()
        {
            // Arrange
            var destFile = new List<string> {
            "file"
            };

            fileSystemWrapper.Setup(x => x.DeleteFile(destFile[0]));

            // Act
            var result = service.MultiDeleteFile(destFile);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MultiDeleteFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var destFile = new List<string> {
            "file",
            "destFile"
            };
            Exception exception = new Exception();

            fileSystemWrapper.Setup(x => x.DeleteFile(destFile[0])).Throws(exception);
            loggingService.Setup(x => x.LogError("MultiDeleteFile", $"Geçici PowerShell Script dosyaları silinemedi.", exception));

            // Act
            var result = service.MultiDeleteFile(destFile);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DeleteFile

        [Test]
        public void DeleteFile_NoCondition_ReturnTrue()
        {
            // Arrange
            var path = "path";

            fileSystemWrapper.Setup(x => x.DeleteFile(path));

            // Act
            var result = service.DeleteFile(path);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void DeleteFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var path = "path";
            Exception exception = new Exception();

            fileSystemWrapper.Setup(x => x.DeleteFile(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("DeleteFile", $"{path} Dosya silinemedi.", exception));

            // Act
            var result = service.DeleteFile(path);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region GetFilePathList

        [Test]
        public void GetFilePathList_NoCondition_ReturnList()
        {
            // Arrange
            var path = "path";
            var fileType = "fileType";
            var model = new string[] { };

            fileSystemWrapper.Setup(x => x.GetFilePathList(path, fileType)).Returns(model);

            // Act
            var result = service.GetFilePathList(path, fileType);

            // Assert
            result.Should().BeEquivalentTo(model.ToList());
        }

        [Test]
        public void GetFilePathList_NoCondition_ReturnEmptyList()
        {
            // Arrange
            var path = "path";
            var fileType = "fileType";
            var model = new string[] { };
            var list = new List<string>();
            Exception exception = new Exception();

            fileSystemWrapper.Setup(x => x.GetFilePathList(path, fileType)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetFilePathList", $"{path} yolunda {fileType} dosya biçimi bulunamadı.", exception));
            fileSystemFactory.Setup(x => x.CreateEmptyStringList()).Returns(list);

            // Act
            var result = service.GetFilePathList(path, fileType);

            // Assert
            result.Should().BeEquivalentTo(list);
        }

        #endregion

        #region GetDirectories

        [Test]
        public void GetDirectories_NoCondition_ReturnString()
        {
            // Arrange
            var path = "path";
            var dizi = new string[] { };

            fileSystemWrapper.Setup(x => x.GetDirectories(path)).Returns(dizi);

            // Act
            var result = service.GetDirectories(path);

            // Assert
            result.Should().BeEquivalentTo(dizi.ToList());
        }

        [Test]
        public void GetDirectories_NoCondition_ReturnEmptyString()
        {
            // Arrange
            var path = "path";
            var dizi = new string[] { };
            Exception exception = new Exception();

            fileSystemWrapper.Setup(x => x.GetDirectories(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetDirectories", $"{path} yolunda klasörler getirilirken sorun oluştu.", exception));
            fileSystemFactory.Setup(x => x.CreateEmptyString()).Returns(dizi);

            // Act
            var result = service.GetDirectories(path);

            // Assert
            result.Should().BeEquivalentTo(dizi);
        }

        #endregion

        #region GetFilePathList

        [Test]
        public void GetFilePathList_NoCondition_ReturnString()
        {
            // Arrange
            var path = "path";
            var dizi = new string[] { };

            fileSystemWrapper.Setup(x => x.GetFilePathList(path)).Returns(dizi);

            // Act
            var result = service.GetFilePathList(path);

            // Assert
            result.Should().BeEquivalentTo(dizi.ToList());
        }

        [Test]
        public void GetFilePathList_NoCondition_ReturnEmptyString()
        {
            // Arrange
            var path = "path";
            var dizi = new string[] { };
            Exception exception = new Exception();

            fileSystemWrapper.Setup(x => x.GetFilePathList(path)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetFilePathList", $"{path} yolunda path listesi getirilirken sorun oluştu.", exception));
            fileSystemFactory.Setup(x => x.CreateEmptyString()).Returns(dizi);

            // Act
            var result = service.GetFilePathList(path);

            // Assert
            result.Should().BeEquivalentTo(dizi);
        }

        #endregion

        #region PathCombine

        [Test]
        public void PathCombine_NoCondition_ReturnString()
        {
            // Arrange
            var path1 = "path1";
            var path2 = "path2";
            var path = @"path1\path2";

            fileSystemWrapper.Setup(x => x.PathCombine(path1, path2)).Returns(path);

            // Act
            var result = service.PathCombine(path1, path2);

            // Assert
            result.Should().Be(path);
        }

        [Test]
        public void PathCombine_NoCondition_ReturnEmptyString()
        {
            // Arrange
            var path1 = "path1";
            var path2 = "path2";
            Exception exception = new Exception();

            fileSystemWrapper.Setup(x => x.PathCombine(path1, path2)).Throws(exception);
            loggingService.Setup(x => x.LogError("PathCombine", $"Verilen parametreleri combine işleminde sorun oluştu.", exception));

            // Act
            var result = service.PathCombine(path1, path2);

            // Assert
            result.Should().Be(string.Empty);
        }

        #endregion
    }
}