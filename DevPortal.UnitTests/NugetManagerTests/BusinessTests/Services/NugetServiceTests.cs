using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.NugetManager.Model.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetServerRepository> nugetServerRepository;

        StrictMock<INugetPackageSerializationService> nugetPackageSerializationService;

        StrictMock<INugetFactory> nugetFactory;

        StrictMock<ICacheWrapper> cacheWrapper;

        StrictMock<ISettings> settings;

        StrictMock<ICsvService> csvService;

        StrictMock<INugetPackageViewModelFactory> nugetPackageViewModelFactory;

        StrictMock<INugetPackageService> nugetPackageService;

        StrictMock<INugetQueryService> nugetQueryService;

        StrictMock<INugetArchiveReaderService> nugetArchiveReaderService;

        StrictMock<IFileSystemService> fileSystemService;

        NugetService service;

        [SetUp]
        public void Initialize()
        {
            nugetServerRepository = new StrictMock<INugetServerRepository>();
            nugetPackageSerializationService = new StrictMock<INugetPackageSerializationService>(); ;
            nugetFactory = new StrictMock<INugetFactory>();
            cacheWrapper = new StrictMock<ICacheWrapper>();
            settings = new StrictMock<ISettings>();
            csvService = new StrictMock<ICsvService>();
            nugetPackageViewModelFactory = new StrictMock<INugetPackageViewModelFactory>();
            nugetPackageService = new StrictMock<INugetPackageService>();
            nugetQueryService = new StrictMock<INugetQueryService>();
            nugetArchiveReaderService = new StrictMock<INugetArchiveReaderService>();
            fileSystemService = new StrictMock<IFileSystemService>();

            service = new NugetService(
                nugetServerRepository.Object,
                nugetPackageSerializationService.Object,
                nugetFactory.Object,
                cacheWrapper.Object,
                settings.Object,
                csvService.Object,
                nugetPackageViewModelFactory.Object,
                nugetPackageService.Object,
                nugetQueryService.Object,
                nugetArchiveReaderService.Object,
                fileSystemService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetServerRepository.VerifyAll();
            nugetPackageSerializationService.VerifyAll();
            nugetFactory.VerifyAll();
            cacheWrapper.VerifyAll();
            settings.VerifyAll();
            csvService.VerifyAll();
            nugetPackageViewModelFactory.VerifyAll();
            nugetPackageService.VerifyAll();
            fileSystemService.VerifyAll();
        }

        #endregion

        #region get filtered from old nuget packages

        [Test]
        public void GetFilteredFromOldNugetPackages_searchStringIsNullOrEmpty_ReturnNugetPAckageFolderList()
        {
            // Arrange
            var path = "//ABNUGETSRV/emeklipaketler";
            string searchString = null;
            var nugetFolderList = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path } };
            var subDirectoryList = new List<string> { "1.0.0" };
            var nugets = new NugetPackageFolderViewModel { NugetPackageFolders = nugetFolderList };

            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(path);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(path)).Returns(nugetFolderList);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetFolderList[0].FilePath)).Returns(subDirectoryList);
            nugetPackageViewModelFactory.Setup(x => x.CreateNugetPackageFolderViewModel(nugetFolderList)).Returns(nugets);

            //Act
            var result = service.GetFilteredOldNugetPackages(searchString);

            // Assert
            result.NugetPackageFolders.Should().NotBeNull();
        }

        [Test]
        public void GetFilteredFromOldNugetPackages_searchStringIsNotNullOrNotEmpty_ReturnNugetPAckageFolderList()
        {
            // Arrange
            var path = "//ABNUGETSRV/emeklipaketler";
            string searchString = "emeklipaketler";
            var nugetFolderList = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path } };
            var subDirectoryList = new List<string> { "1.0.0" };
            var nugets = new NugetPackageFolderViewModel { NugetPackageFolders = nugetFolderList };

            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(path);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(path)).Returns(nugetFolderList);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetFolderList[0].FilePath)).Returns(subDirectoryList);
            nugetPackageViewModelFactory.Setup(x => x.CreateNugetPackageFolderViewModel(nugetFolderList)).Returns(nugets);
            nugetPackageViewModelFactory.Setup(x => x.CreateNugetPackageFolderViewModel(nugetFolderList)).Returns(nugets);

            //Act
            var result = service.GetFilteredOldNugetPackages(searchString);

            // Assert
            result.NugetPackageFolders.Should().NotBeNull();
        }

        #endregion

        #region get filtered nuget packages

        [Test]
        public void GetFilteredNugetPackages_NoCondition_ReturnFilteredNugetPackageList()
        {
            // Arrange
            var skip = 0;
            var take = 0;
            string searchString = null;
            var orderBy = (int)NugetOrderType.None;

            var allPackages = new List<NugetPackage>();
            var groupedPackages = new List<NugetPackage>();
            var orderedPackages = new List<NugetPackage>();
            var searchedPackages = new List<NugetPackage>();
            var filteredNugetPackages = searchedPackages
                .Skip(skip)
                .Take(take);
            var filteredNugetPackagesModel = new FilteredNugetPackages();

            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(allPackages);
            nugetQueryService.Setup(x => x.GetGroupedNugetPackages(allPackages)).Returns(groupedPackages);
            nugetQueryService.Setup(x => x.OrderPackages(groupedPackages, orderBy)).Returns(orderedPackages);
            nugetQueryService.Setup(x => x.SearchByTagOrTitle(orderedPackages, searchString)).Returns(searchedPackages);
            nugetFactory.Setup(x => x.CreateFilteredNugetPackagesModel(0, filteredNugetPackages)).Returns(filteredNugetPackagesModel);

            // Act
            var result = service.GetFilteredNugetPackages(skip, take, searchString, orderBy);

            // Assert
            result.Should().Be(filteredNugetPackagesModel);
        }

        #endregion

        #region get grouped nuget packages

        [Test]
        public void GetGroupedNugetPackages_NoCondition_ReturnPackages()
        {
            // Arrange
            var allPackages = new List<NugetPackage>();
            var groupedPackages = new List<NugetPackage>();

            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(allPackages);
            nugetQueryService.Setup(x => x.GetGroupedNugetPackages(allPackages)).Returns(groupedPackages);

            // Act
            var result = service.GetGroupedNugetPackages();

            // Assert
            result.Should().BeSameAs(groupedPackages);
        }

        [Test]
        public void GetGroupedNugetPackages_PackagesSupplied_ReturnPackages()
        {
            // Arrange
            var allPackages = new List<NugetPackage>();

            var groupedPackages = new List<NugetPackage>();

            nugetQueryService.Setup(x => x.GetGroupedNugetPackages(allPackages)).Returns(groupedPackages);

            // Act
            var result = service.GetGroupedNugetPackages(allPackages);

            // Assert
            result.Should().BeSameAs(groupedPackages);
        }

        #endregion

        #region GetNugetPackagesByTitle

        [Test]
        public void GetNugetPackagesByTitle_NotFoundInArchive_ReturnPackagesViewModel()
        {
            // Arrange
            string title = "title";

            var allPackages = new List<NugetPackage>();
            var physicalPath = "PhysicalPath";
            var archivedPackageFolder = new NugetPackageFolder { Path = physicalPath };
            var archivedPackageFolders = new List<NugetPackageFolder> { archivedPackageFolder };
            var viewModel = new NugetPackageViewModel();

            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(allPackages);
            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(archivedPackageFolders);
            nugetArchiveReaderService.Setup(x => x.SetPackageArchiveStatus(allPackages, archivedPackageFolder));
            nugetPackageViewModelFactory.Setup(x => x.CreateNugetPackageViewModel(allPackages)).Returns(viewModel);

            // Act
            var result = service.GetNugetPackagesByTitle(title);

            // Assert
            result.Should().BeSameAs(viewModel);
        }

        [Test]
        public void GetNugetPackagesByTitle_FoundInArchive_SetArchiveStatusAndReturnPackages()
        {
            // Arrange
            string title = "title";

            var allPackages = new List<NugetPackage>();
            var physicalPath = "title";
            var archivedPackageFolder = new NugetPackageFolder { Path = physicalPath };
            var archivedPackageFolders = new List<NugetPackageFolder> { archivedPackageFolder };
            var viewModel = new NugetPackageViewModel();

            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(allPackages);
            nugetArchiveReaderService.Setup(x => x.GetArchivedNugetPackages()).Returns(archivedPackageFolders);
            nugetArchiveReaderService.Setup(x => x.SetPackageArchiveStatus(allPackages, archivedPackageFolder));
            nugetPackageViewModelFactory.Setup(x => x.CreateNugetPackageViewModel(allPackages)).Returns(viewModel);

            // Act
            var result = service.GetNugetPackagesByTitle(title);

            // Assert
            result.Should().BeSameAs(viewModel);
        }

        #endregion

        #region get last updated nuget packages

        [Test]
        public void GetLastUpdatedNugetPackages_OrderPublishedDateDesc_ReturnLastUpdatedNugetPackageListItems()
        {
            // Arrange
            var time = DateTime.Now;
            var properties = new NugetProperties { Published = time };
            var nugetPackage = new NugetPackage
            {
                Id = "id",
                Title = "ApiClient",
                Properties = properties
            };
            var packages = new List<NugetPackage> { nugetPackage };
            var packageUrl = new Uri($"http://nuget/packages/{nugetPackage.Title}");
            var packageListItem = new LastUpdatedNugetPackageListItem
            {
                PackageName = nugetPackage.Title,
                PackageUrl = packageUrl.ToString(),
                CurrentVersion = nugetPackage.Properties.Version,
                LastUpdateDate = nugetPackage.Properties.Published
            };
            var packageList = new List<LastUpdatedNugetPackageListItem> { packageListItem };

            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(packages);
            nugetPackageService.Setup(x => x.GetNugetPackageUrl(nugetPackage.Title)).Returns(packageUrl);

            // Act
            var result = service.GetLastUpdatedNugetPackages();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(packageList);
            result[0].Should().BeEquivalentTo(packageListItem);
        }

        #endregion

        #region get nuget packages stats

        [Test]
        public void GetNugetPackagesStats_NoCondition_ReturnStats()
        {
            // Arrange
            var nugetPackage1 = new NugetPackage { Title = "ApiClient" };
            var nugetPackage2 = new NugetPackage { Title = "ApiClient" };
            var packages = new List<NugetPackage> {
                nugetPackage1,
                nugetPackage2
            };

            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(packages);

            // Act
            var result = service.GetNugetPackagesStats();

            // Assert
            result.AllNugetPackagesCount.Should().Be(packages.Count);
            result.ActiveNugetPackagesCount.Should().Be(1);
        }

        #endregion

        #region get nuspec template

        [Test]
        public void GetNuspecTemplate_ArchivePackagesListFullAndArchiveCheck_ReturnNugetPackageList()
        {
            // Arrange
            var path = "path";
            var fileModel = new LogFileModel();

            nugetServerRepository.Setup(x => x.GetNuspecTemplate(path)).Returns(fileModel);

            //Act
            var result = service.GetNuspecTemplate(path);

            // Assert
            result.Should().Be(fileModel);
        }

        #endregion

        #region export packages as csv

        [Test]
        public void ExportNugetPackagesAsCsv_NoCondition_ReturnCsvServiceResultSuccess()
        {
            // Arrange
            var nugetPackageList = new List<NugetPackage> {
                new NugetPackage{
                     Id = "1",
                     Properties = new NugetProperties(),
                     Title ="Title"
                     }
            };

            byte[] encodedBytes = { 1, 2, 3 };

            csvService.Setup(x => x.ExportToCsv(SetupAny<ICollection<NugetExportListItem>>(), CsvColumnNames.NugetList)).Returns(encodedBytes);

            // Act
            var result = service.ExportNugetPackagesAsCsv(nugetPackageList);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Value.Should().BeSameAs(encodedBytes);
        }

        #endregion

        #region emekli paket

        [Test]
        public void MoveNugetPackageToOldPackagesFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var path = "path/" + title;
            var sourcePath = "sourcePath";
            var destPath = "destPath";
            var list = new List<string> { "1.0.0", "2.0.0", "3.0.0" };
            var nugetlist = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path, SubDirectory = list } };

            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(sourcePath);
            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(destPath);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(destPath)).Returns(nugetlist);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetlist[0].FilePath)).Returns(nugetlist[0].SubDirectory);

            // Act
            var result = service.MoveNugetPackageToOldPackagesFile(title);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void MoveNugetPackageToOldPackagesFile_CreateFolder_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var path = "path/";
            var sourcePath = "sourcePath";
            var destPath = "destPath";
            var dest = destPath + @"\" + title;
            var list = new List<string> { "1.0.0", "2.0.0", "3.0.0" };
            var nugetlist = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path, SubDirectory = list } };

            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(sourcePath);
            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(destPath);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(destPath)).Returns(nugetlist);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetlist[0].FilePath)).Returns(nugetlist[0].SubDirectory);
            fileSystemService.Setup(x => x.CreateFolder(dest)).Returns(false);

            // Act
            var result = service.MoveNugetPackageToOldPackagesFile(title);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void MoveNugetPackageToOldPackagesFile_CreateFolder_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var path = "path/";
            var sourcePath = "sourcePath";
            var destPath = "destPath";
            var dest = destPath + @"\" + title;
            var source = sourcePath + @"\" + title;
            var list = new List<string> { "1.0.0" };
            var destItem = dest + @"\" + list[0] + @"\";
            var sourceItem = source + @"\" + list[0] + @"\";
            var nugetlist = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path, SubDirectory = list } };

            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(sourcePath);
            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(destPath);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(destPath)).Returns(nugetlist);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetlist[0].FilePath)).Returns(nugetlist[0].SubDirectory);
            fileSystemService.Setup(x => x.CreateFolder(dest)).Returns(true);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(source)).Returns(list);
            fileSystemService.Setup(x => x.CreateFolder(destItem)).Returns(true);
            fileSystemService.Setup(x => x.MoveFile(sourceItem, destItem)).Returns(true);
            fileSystemService.Setup(x => x.DoesFolderExists($"{dest}".Replace("/", @"\"))).Returns(true);
            fileSystemService.Setup(x => x.DeleteFolder($"{source}".Replace("/", @"\"))).Returns(true);
            cacheWrapper.Setup(x => x.Remove(CacheKeyNames.NugetPackageList));

            // Act
            var result = service.MoveNugetPackageToOldPackagesFile(title);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MoveOldPackageToNugetPackagesFile_CreateFolder_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var path = "path/";
            var sourcePath = "sourcePath";
            var destPath = "destPath";
            var dest = destPath + @"\" + title;
            var source = sourcePath + @"\" + title;
            var list = new List<string> { "1.0.0" };
            var destItem = dest + @"\" + list[0] + @"\";
            var sourceItem = source + @"\" + list[0] + @"\";
            var nugetlist = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path, SubDirectory = list } };

            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(sourcePath);
            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(destPath);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(sourcePath)).Returns(nugetlist);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetlist[0].FilePath)).Returns(nugetlist[0].SubDirectory);
            fileSystemService.Setup(x => x.CreateFolder(source)).Returns(true);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(dest)).Returns(list);
            fileSystemService.Setup(x => x.CreateFolder(sourceItem)).Returns(true);
            fileSystemService.Setup(x => x.MoveFile(destItem, sourceItem)).Returns(true);
            fileSystemService.Setup(x => x.DoesFolderExists($"{source}".Replace("/", @"\"))).Returns(true);
            fileSystemService.Setup(x => x.DeleteFolder($"{dest}".Replace("/", @"\"))).Returns(true);
            cacheWrapper.Setup(x => x.Remove(CacheKeyNames.NugetPackageList));

            // Act
            var result = service.MoveOldPackageToNugetPackagesFile(title);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MoveOldPackageToNugetPackagesFile_FolderExists_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var path = "path/";
            var sourcePath = "sourcePath";
            var destPath = "destPath";
            var dest = destPath + @"\" + title;
            var source = sourcePath + @"\" + title;
            var list = new List<string> { "1.0.0" };
            var destItem = dest + @"\" + list[0] + @"\";
            var sourceItem = source + @"\" + list[0] + @"\";
            var nugetlist = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path, SubDirectory = list } };

            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(sourcePath);
            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(destPath);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(sourcePath)).Returns(nugetlist);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetlist[0].FilePath)).Returns(nugetlist[0].SubDirectory);
            fileSystemService.Setup(x => x.CreateFolder(source)).Returns(true);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(dest)).Returns(list);
            fileSystemService.Setup(x => x.CreateFolder(sourceItem)).Returns(true);
            fileSystemService.Setup(x => x.MoveFile(destItem, sourceItem)).Returns(true);
            fileSystemService.Setup(x => x.DoesFolderExists($"{source}".Replace("/", @"\"))).Returns(false);

            // Act
            var result = service.MoveOldPackageToNugetPackagesFile(title);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void MoveOldPackageToNugetPackagesFile_GetNugetPackagesIsNull_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var path = "path/" + title;
            var sourcePath = "sourcePath";
            var destPath = "destPath";
            var dest = destPath + @"\" + title;
            var source = sourcePath + @"\" + title;
            var list = new List<string> { "1.0.0" };
            var destItem = dest + @"\" + list[0] + @"\";
            var sourceItem = source + @"\" + list[0] + @"\";
            var nugetlist = new List<NugetPackageFolder> { new NugetPackageFolder { Path = path, SubDirectory = list } };

            settings.Setup(x => x.NugetPackagesPhysicalPath).Returns(sourcePath);
            settings.Setup(x => x.OldNugetPackagesPhysicalPath).Returns(destPath);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(sourcePath)).Returns(nugetlist);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetlist[0].FilePath)).Returns(nugetlist[0].SubDirectory);

            // Act
            var result = service.MoveOldPackageToNugetPackagesFile(title);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}