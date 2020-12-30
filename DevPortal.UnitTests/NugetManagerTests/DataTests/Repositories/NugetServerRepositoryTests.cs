using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Repositories;
using DevPortal.NugetManager.Model;
using DevPortal.Resources.Resources;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.UnitTests.NugetManagerTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetServerRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetServerApiClientWrapper> apiClient;

        StrictMock<ILoggingService> loggingService;

        StrictMock<IFileSystemService> fileSystemService;

        StrictMock<INugetServerFactory> nugetServerFactory;

        StrictMock<IApplicationDataRequestFactory> applicationDataRequestFactory;

        NugetServerRepository repository;

        [SetUp]
        public void Initialize()
        {
            apiClient = new StrictMock<INugetServerApiClientWrapper>();
            loggingService = new StrictMock<ILoggingService>();
            fileSystemService = new StrictMock<IFileSystemService>();
            nugetServerFactory = new StrictMock<INugetServerFactory>();
            applicationDataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();

            repository = new NugetServerRepository(
                apiClient.Object,
                loggingService.Object,
                fileSystemService.Object,
                nugetServerFactory.Object,
                applicationDataRequestFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            apiClient.VerifyAll();
            fileSystemService.VerifyAll();
            loggingService.VerifyAll();
            nugetServerFactory.VerifyAll();
            applicationDataRequestFactory.VerifyAll();
        }

        #endregion

        #region GetFiles

        [Test]
        public void GetFileList_NoCondition_ReturnPackages()
        {
            // Arrange
            var filePath = "filePath";
            List<string> directory = new List<string> { "path" };
            List<string> list = new List<string>();
            fileSystemService.Setup(x => x.GetFilePathList(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.GetSubDirectoryFoldersName(directory)).Returns(list);

            var result = repository.GetFileList(filePath);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetFileList_FilesCountGreatherThanTwo_ReturnPackages()
        {
            // Arrange
            var filePath = "filePath";
            List<string> directory = new List<string> { "path", "path2" };
            List<string> list = new List<string> { "1", "2" };
            fileSystemService.Setup(x => x.GetFilePathList(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.GetSubDirectoryFoldersName(directory)).Returns(list);

            var result = repository.GetFileList(filePath);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetFileList_Trows_ReturnPackages()
        {
            // Arrange
            var filePath = "filePath";
            List<string> directory = new List<string> { "path" };
            Exception exception = new Exception();
            var emptyList = new List<string>();
            fileSystemService.Setup(x => x.GetFilePathList(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.GetSubDirectoryFoldersName(directory)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetFileList", "Dosya içeriği okunamadı.", exception));
            nugetServerFactory.Setup(x => x.CreateEmptyStringList()).Returns(emptyList);

            var result = repository.GetFileList(filePath);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetPackages

        [Test]
        public void GetPackages_NoCondition_ReturnPackages()
        {
            // Arrange
            var skip = 100;
            var packages = "packages";
            apiClient.Setup(x => x.Get<string>($"Packages?$skip={skip}", null, null)).Returns(packages);

            var result = repository.GetPackages(skip);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(packages);
        }

        [Test]
        public void GetPackages_Trows_ReturnPackages()
        {
            // Arrange
            var skip = 100;
            Exception exception = new Exception();
            apiClient.Setup(x => x.Get<string>($"Packages?$skip={skip}", null, null)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetPackages", "Nuget sunucusundan paketlerin çekilmesi sırasında beklenmeyen bir hata oluştu.", exception));
            var result = repository.GetPackages(skip);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetNugetPackagesFolder

        [Test]
        public void GetNugetPackagesFolder_NoCondition_ReturnPackageFolderList()
        {
            // Arrange
            var filePath = "filePath/a";
            var directory = new List<string> { filePath };
            var nugetArchive = new List<NugetPackageFolder> { new NugetPackageFolder { Path = filePath } };
            fileSystemService.Setup(x => x.GetDirectories(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.CreateNugetPackageFolders(directory)).Returns(nugetArchive);

            var result = repository.GetNugetPackagesFolder(filePath).ToList();

            // Assert
            result[0].Name.Should().Be("a");
        }

        [Test]
        public void GetNugetPackagesFolder_Trows_ReturnThrowsAndErrorLog()
        {
            // Arrange
            var filePath = "filePath";
            Exception exception = new Exception();
            var directory = new List<string> { filePath };
            var emptyList = new List<NugetPackageFolder>();
            fileSystemService.Setup(x => x.GetDirectories(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.CreateNugetPackageFolders(directory)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetNugetPackagesFolder", Messages.DosyaOkunamadi, exception));
            nugetServerFactory.Setup(x => x.CreateNugetPackageFolders()).Returns(emptyList);
            var result = repository.GetNugetPackagesFolder(filePath);

            // Assert
            result.Count.Should().Be(emptyList.Count);
        }

        #endregion

        #region GetSubDirectoryFoldersName

        [Test]
        public void GetSubDirectoryFoldersName_NoCondition_ReturnPackageFileList()
        {
            // Arrange
            var filePath = "filePath/a";
            var directory = new List<string> { filePath };
            var nugetNupkgFile = new List<string> { "1.0.0", "2.0.0" };
            fileSystemService.Setup(x => x.GetDirectories(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.GetSubDirectoryFoldersName(directory)).Returns(nugetNupkgFile);

            var result = repository.GetSubDirectoryFoldersName(filePath).ToList();

            // Assert
            result[0].Should().Be(nugetNupkgFile[0]);
        }

        [Test]
        public void GetSubDirectoryFoldersName_Trows_ReturnThrowsAndErrorLog()
        {
            // Arrange
            var filePath = "filePath";
            Exception exception = new Exception();
            var directory = new List<string> { filePath };
            var emptyList = new List<string>();
            fileSystemService.Setup(x => x.GetDirectories(filePath)).Returns(directory);
            nugetServerFactory.Setup(x => x.GetSubDirectoryFoldersName(directory)).Throws(exception);
            loggingService.Setup(x => x.LogError("GetSubDirectoryFoldersName", Messages.DosyaOkunamadi, exception));
            nugetServerFactory.Setup(x => x.CreateEmptyStringList()).Returns(emptyList);
            var result = repository.GetSubDirectoryFoldersName(filePath);

            // Assert
            result.Count.Should().Be(emptyList.Count);
        }

        #endregion

        #region get download file contents

        [Test]
        public void GetDownloadFileContents_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            var fileModel = new Model.LogFileModel();

            fileSystemService.Setup(x => x.GetDownloadFileContents(path)).Returns(fileModel);

            // Act
            var result = repository.GetDownloadFileContents(path);

            // Assert
            result.Should().Be(fileModel);
        }

        #endregion

        #region get nuspec template

        [Test]
        public void GetNuspecTemplate_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            var fileModel = new Model.LogFileModel();

            fileSystemService.Setup(x => x.GetFileContent(path)).Returns(fileModel);

            // Act
            var result = repository.GetNuspecTemplate(path);

            // Assert
            result.Should().Be(fileModel);
        }

        [Test]
        public void GetNuspecTemplate_NoCondition_ReturnNull()
        {
            // Arrange
            var path = "path";
            var fileModel = new Model.LogFileModel();
            var exception = new Exception();
            var methodName = $"GetNuspecTemplate";
            var message = "Dosya okunamadı.";

            fileSystemService.Setup(x => x.GetFileContent(path)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, message, exception));

            // Act
            var result = repository.GetNuspecTemplate(path);

            // Assert
            result.Should().BeNull();
        }

        #endregion
    }
}