using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetArchiveServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetServerRepository> nugetServerRepository;

        StrictMock<IFileService> fileService;

        StrictMock<ISettings> settings;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<INugetArchiveReaderService> nugetArchiveReaderService;

        StrictMock<IFileSystemService> fileSystemService;

        NugetArchiveService service;

        [SetUp]
        public void Initialize()
        {
            nugetServerRepository = new StrictMock<INugetServerRepository>();
            fileService = new StrictMock<IFileService>();
            settings = new StrictMock<ISettings>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            nugetArchiveReaderService = new StrictMock<INugetArchiveReaderService>();
            fileSystemService = new StrictMock<IFileSystemService>();

            service = new NugetArchiveService(
                nugetServerRepository.Object,
                fileService.Object,
                settings.Object,
                generalSettingsService.Object,
                nugetArchiveReaderService.Object,
                fileSystemService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetServerRepository.VerifyAll();
            fileService.VerifyAll();
            settings.VerifyAll();
            generalSettingsService.VerifyAll();
            nugetArchiveReaderService.VerifyAll();
            fileSystemService.VerifyAll();
        }

        #endregion

        #region get filtered from archived nuget packages

        [Test]
        public void GetFilteredFromArchivedNugetPackages_searchStringIsNullOrEmpty_ReturnNugetPAckageFolderList()
        {
            // Arrange
            var path = "\\venus\\desktop";
            string searchString = null;
            var nugetFolderList = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path } };
            var subDirectoryList = new List<string> { "1.0.0" };

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetFolderList);

            //Act
            var result = service.GetFilteredFromArchivedNugetPackages(searchString);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetFilteredFromArchivedNugetPackages_searchStringIsNotNullOrNotEmpty_ReturnNugetPAckageFolderList()
        {
            // Arrange
            var path = "\\venus\\desktop";
            string searchString = "searchString";
            var nugetFolderList = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path } };
            var subDirectoryList = new List<string> { "1.0.0" };

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetFolderList);

            //Act
            var result = service.GetFilteredFromArchivedNugetPackages(searchString);

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region get nuget packages nupkg file contents

        [Test]
        public void GetNugetPackagesNupkgFileContents_DoesntExistNupkgFileInList_ReturnNull()
        {
            // Arrange
            var filePath = "AB.Data.ApiClient";
            var fileName = "fileName";
            var list = new List<string> { };

            nugetServerRepository.Setup(x => x.GetFileList(filePath)).Returns(list);
            fileService.Setup(x => x.FilePathListContainsFile(fileName, list)).Returns(false);

            //Act
            var result = service.GetNugetPackagesNupkgFileContents(filePath, fileName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetNugetPackagesNupkgFileContents_DoesExistNupkgFileInList_ReturnFileModel()
        {
            // Arrange
            var filePath = "path";
            var fileName = "fileName";
            var list = new List<string> { fileName };
            var fileModel = new LogFileModel();
            var itemNumber = 0;

            nugetServerRepository.Setup(x => x.GetFileList(filePath)).Returns(list);
            fileService.Setup(x => x.FilePathListContainsFile(fileName, list)).Returns(true);
            fileService.Setup(x => x.GetFileIndexInFilePathList(list, fileName)).Returns(itemNumber);
            nugetServerRepository.Setup(x => x.GetDownloadFileContents(filePath + list[itemNumber])).Returns(fileModel);

            //Act
            var result = service.GetNugetPackagesNupkgFileContents(filePath, fileName);

            // Assert
            result.Should().Be(fileModel);
        }

        #endregion

        #region create package folder

        [Test]
        public void CreatePackageFolder_ArchivePackageIsNullAndCreatePackageFolderIsNotSuccess_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "physicalPath";
            var nugetPackageFolders = new List<NugetPackageFolder>();

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);
            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(physicalPath);
            fileSystemService.Setup(x => x.CreateFolder(physicalPath + "/" + title)).Returns(false);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CreatePackageFolder_CreatePackageFolderIsSuccessAndCreatePackageVersionFolderIsNotSuccess_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "physicalPath";
            var list = new List<string>();
            var nugetPackageFolders = new List<NugetPackageFolder> {
                 new NugetPackageFolder {
                     Path = "Path"
                 }
            };

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);
            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(physicalPath);
            fileSystemService.Setup(x => x.CreateFolder(physicalPath + "/" + title)).Returns(true);

            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(physicalPath);
            fileSystemService.Setup(x => x.CreateFolder(physicalPath + "/" + title + "/" + versionItem)).Returns(false);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CreatePackageFolder_ArchivePackageIsNotNullAndCreatePackageVersionFolderIsNotSuccess_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "PhysicalPath";
            var list = new List<string> { versionItem };
            var nugetPackageFolders = new List<NugetPackageFolder> {
                 new NugetPackageFolder {
                     Path = "title",
                     SubDirectory = new List<string>()
                 }
        };

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);

            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(physicalPath);
            fileSystemService.Setup(x => x.CreateFolder(physicalPath + "/" + title + "/" + versionItem)).Returns(false);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CreatePackageFolder_CreatePackageVersionFolderIsSuccessAndIsPackageArchived_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "physicalPath";
            var archivePhysicalPath = "archivePhysicalPath";
            var list = new List<string>();
            var nugetPackageFolders = new List<NugetPackageFolder> {
                 new NugetPackageFolder {
                     Path = "title",
                      SubDirectory = new List<string>()
                 }
            };

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);

            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(archivePhysicalPath);
            fileSystemService.Setup(x => x.CreateFolder(archivePhysicalPath + "/" + title + "/" + versionItem)).Returns(true);

            var SourceFile = NugetServerNupkgFilePath(title, versionItem, physicalPath);
            string destFile = ArchiveNugetServerNupkgFilePath(title, versionItem, archivePhysicalPath);

            nugetArchiveReaderService.Setup(x => x.IsPackageArchived(destFile)).Returns(true);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeTrue();
        }

        private string ArchiveNugetServerNupkgFilePath(string title, string versionItem, string archivePhysicalPath)
        {
            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(archivePhysicalPath);
            return archivePhysicalPath + "/" + title + "/" + versionItem + "/" + title + "." + versionItem + ".nupkg";
        }

        private string NugetServerNupkgFilePath(string title, string versionItem, string physicalPath)
        {
            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(physicalPath);
            return (physicalPath + "/" + title + "/" + versionItem + "/" + title + "." + versionItem + ".nupkg").Replace("/", @"\");
        }

        [Test]
        public void CreatePackageFolder_HasVersionFolderAndIsPackageArchived_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "physicalPath";
            var archivePhysicalPath = "archivePhysicalPath";
            var list = new List<string> { versionItem };
            var nugetPackageFolders = new List<NugetPackageFolder> {
                 new NugetPackageFolder {
                     Path = "title",
                     SubDirectory = new List<string>{versionItem}
                 }
            };

            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);

            var sourceFile = NugetServerNupkgFilePath(title, versionItem, physicalPath);
            string destFile = ArchiveNugetServerNupkgFilePath(title, versionItem, archivePhysicalPath);

            nugetArchiveReaderService.Setup(x => x.IsPackageArchived(destFile)).Returns(true);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CreatePackageFolder_CopyFileIsSuccess_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "physicalPath";
            var archivePhysicalPath = "archivePhysicalPath";
            var list = new List<string>();
            var nugetPackageFolders = new List<NugetPackageFolder> {
                 new NugetPackageFolder {
                     Path = "title",
                     SubDirectory = new List<string>{versionItem}
                 }
            };
            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);

            var sourceFile = NugetServerNupkgFilePath(title, versionItem, physicalPath);
            string destFile = ArchiveNugetServerNupkgFilePath(title, versionItem, archivePhysicalPath);

            nugetArchiveReaderService.Setup(x => x.IsPackageArchived(destFile)).Returns(false);

            fileSystemService.Setup(x => x.CopyFile(sourceFile, destFile)).Returns(true);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CreatePackageFolder_CopyFileIsNotSuccess_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var versionItem = "1.0.1";
            var physicalPath = "physicalPath";
            var archivePhysicalPath = "archivePhysicalPath";
            var list = new List<string>();
            var nugetPackageFolders = new List<NugetPackageFolder> {
                 new NugetPackageFolder {
                     Path = "title",
                     SubDirectory = new List<string>{versionItem}
                 }
            };
            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(nugetPackageFolders);

            var sourceFile = NugetServerNupkgFilePath(title, versionItem, physicalPath);
            string destFile = ArchiveNugetServerNupkgFilePath(title, versionItem, archivePhysicalPath);

            nugetArchiveReaderService.Setup(x => x.IsPackageArchived(destFile)).Returns(false);

            fileSystemService.Setup(x => x.CopyFile(sourceFile, destFile)).Returns(false);

            //Act
            var result = service.CreatePackageFolder(title, versionItem);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}