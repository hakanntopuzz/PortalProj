using AB.Framework.UnitTests;
using DevPortal.Model;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Business.Abstract.Service;
using DevPortal.NugetManager.Model;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetService> nugetService;

        StrictMock<INugetArchiveService> nugetArchiveService;

        StrictMock<INuspecService> nuspecService;

        StrictMock<IHostEnvironment> hostEnvironment;

        NugetController controller;

        [SetUp]
        public void Initialize()
        {
            nugetService = new StrictMock<INugetService>();
            nugetArchiveService = new StrictMock<INugetArchiveService>();
            nuspecService = new StrictMock<INuspecService>();
            hostEnvironment = new StrictMock<IHostEnvironment>();

            controller = new NugetController(
                nugetService.Object,
                nugetArchiveService.Object,
                nuspecService.Object,
                hostEnvironment.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetService.VerifyAll();
            nugetArchiveService.VerifyAll();
            nuspecService.VerifyAll();
            hostEnvironment.VerifyAll();
        }

        #endregion

        #region get filtered nuget packages

        [Test]
        public void GetFilteredNugetPackages_NoCondition_ReturnNugetPackages()
        {
            // Arrange
            var skip = 10;
            var take = 10;
            const string searchString = "";
            var orderBy = 1;
            var filteredNugetPackages = new FilteredNugetPackages();

            nugetService.Setup(x => x.GetFilteredNugetPackages(skip, take, searchString, orderBy)).Returns(filteredNugetPackages);

            // Act
            var result = controller.GetFilteredNugetPackages(skip, take, searchString, orderBy);

            // Assert
            result.Should().Be(filteredNugetPackages);
        }

        #endregion

        #region get nuget packages by title

        [Test]
        public void GetNugetPackagesByTitle_NoCondition_ReturnNugetPackageList()
        {
            // Arrange
            var packageTitle = "packageTitle";
            var id = "1";
            var nugetPackage = new NugetPackage { Id = id };
            var nugetPackageList = new List<NugetPackage> { nugetPackage };
            var nugets = new NugetPackageViewModel { NugetPackages = nugetPackageList };

            nugetService.Setup(x => x.GetNugetPackagesByTitle(packageTitle)).Returns(nugets);

            // Act
            var result = controller.GetNugetPackagesByTitle(packageTitle);

            // Assert
            result.Should().NotBeNull();
            nugets.NugetPackages[0].Id.Should().Be(id);
        }

        #endregion

        #region get filtered from archived nuget packages

        [Test]
        public void GetFilteredFromArchivedNugetPackages_NoCondition_ReturnNugetPackageFolderList()
        {
            // Arrange
            var nugetPackageFolder = new List<NugetPackageFolder>();
            var searchString = "searchString";

            nugetArchiveService.Setup(x => x.GetFilteredFromArchivedNugetPackages(searchString)).Returns(nugetPackageFolder);

            // Act
            var result = controller.GetFilteredFromArchivedNugetPackages(searchString);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(nugetPackageFolder.Count);
        }

        #endregion

        #region get filtered from old nuget packages

        [Test]
        public void GetFilteredFromOldNugetPackages_NoCondition_ReturnNugetPackageFolderViewModel()
        {
            // Arrange

            var nugetPackageFolder = new NugetPackageFolderViewModel
            {
                NugetPackageFolders = new List<NugetPackageFolder> { new NugetPackageFolder { } }
            };
            var searchString = "searchString";

            nugetService.Setup(x => x.GetFilteredOldNugetPackages(searchString)).Returns(nugetPackageFolder);

            // Act
            var result = controller.GetFilteredFromOldNugetPackages(searchString);

            // Assert
            result.Should().NotBeNull();
            result.NugetPackageFolders.Count.Should().Be(nugetPackageFolder.NugetPackageFolders.Count);
        }

        #endregion

        #region file download

        [Test]
        public void NupkgDownload_NoCondition_ReturnFile()
        {
            // Arrange
            var path = "path";
            var fileName = "fileName";
            var nugetPackageLists = new LogFileModel();

            nugetArchiveService.Setup(x => x.GetNugetPackagesNupkgFileContents(path, fileName)).Returns(nugetPackageLists);

            // Act
            var result = controller.NupkgDownload(path, fileName);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(nugetPackageLists);
        }

        [Test]
        public void NuspecDownload_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            var fileName = "fileName";
            var rootPath = "rootPath";
            var fileModel = new LogFileModel();

            hostEnvironment.Setup(x => x.ContentRootPath).Returns(rootPath);
            nuspecService.Setup(x => x.GetNuspecFileContents(rootPath + "/wwwroot" + path, fileName)).Returns(fileModel);

            // Act
            var result = controller.NuspecDownload(path, fileName);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(fileModel);
        }

        #endregion

        #region get nuspec template

        [Test]
        public void GetNuspecTemplate_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "path";
            var rootPath = "rootPath";
            var fileModel = new LogFileModel();

            hostEnvironment.Setup(x => x.ContentRootPath).Returns(rootPath);
            nugetService.Setup(x => x.GetNuspecTemplate(rootPath + "/wwwroot" + path)).Returns(fileModel);

            // Act
            var result = controller.GetNuspecTemplate(path);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(fileModel);
        }

        #endregion

        #region archive package

        [Test]
        public void ArchivePackage_NoCondition_ReturnTrue()
        {
            // Arrange
            var title = "title";
            var versionItem = "versionItem";
            nugetArchiveService.Setup(x => x.CreatePackageFolder(title, versionItem)).Returns(true);

            // Act
            var result = controller.ArchivePackage(title, versionItem);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ArchivePackage_NoCondition_ReturnFalse()
        {
            // Arrange
            var title = "title";
            var versionItem = "versionItem";
            nugetArchiveService.Setup(x => x.CreatePackageFolder(title, versionItem)).Returns(false);

            // Act
            var result = controller.ArchivePackage(title, versionItem);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region move nuget packages to archive

        //[Test]
        //public void MoveNugetPackageToOldPackagesFile_NoCondition_ReturnTrue()
        //{
        //    // Arrange
        //    var title = "title";
        //    nugetService.Setup(x => x.MoveNugetPackageToOldPackagesFile(title)).Returns(true);

        //    // Act
        //    var result = controller.MoveNugetPackageToOldPackagesFile(title);

        //    // Assert
        //    result.Should().BeTrue();
        //}

        //[Test]
        //public void MoveNugetPackageToOldPackagesFile_NoCondition_ReturnFalse()
        //{
        //    // Arrange
        //    var title = "title";
        //    nugetService.Setup(x => x.MoveNugetPackageToOldPackagesFile(title)).Returns(false);

        //    // Act
        //    var result = controller.MoveNugetPackageToOldPackagesFile(title);

        //    // Assert
        //    result.Should().BeFalse();
        //}

        #endregion

        #region export nuget packages

        [Test]
        public void ExportNugetPackages_NoCondition_ReturnByteArray()
        {
            // Arrange
            var skip = 0;
            var totalCount = 49;
            var orderBy = 0;
            var filteredNugetPackages = new FilteredNugetPackages();
            var searchString = "searchString";
            byte[] csvData = { 1, 2, 3 };
            var csvServiceResult = CsvServiceResult.Success(csvData);

            nugetService.Setup(x => x.GetFilteredNugetPackages(skip, totalCount, searchString, orderBy)).Returns(filteredNugetPackages);
            nugetService.Setup(x => x.ExportNugetPackagesAsCsv(filteredNugetPackages.NugetPackages)).Returns(csvServiceResult);

            // Act
            var result = controller.ExportNugetPackages(searchString, totalCount);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeOfType(csvServiceResult.Value.GetType());
        }

        #endregion

        #region move nuget package to old packages file

        [Test]
        public void MoveNugetPackageToOldPackagesFile_NoCondition_ReturnTrue()
        {
            // Arrange
            var title = "title";
            nugetService.Setup(x => x.MoveNugetPackageToOldPackagesFile(title)).Returns(true);

            // Act
            var result = controller.MoveNugetPackageToOldPackagesFile(title);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void MoveNugetPackageToOldPackagesFile_NoCondition_ReturnFalse()
        {
            // Arrange
            var title = "title";
            nugetService.Setup(x => x.MoveNugetPackageToOldPackagesFile(title)).Returns(false);

            // Act
            var result = controller.MoveNugetPackageToOldPackagesFile(title);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region move old package to nuget packages file

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void MoveOldPackageToNugetPackagesFile_TestCase_ReturnTestCaseResult(bool value)
        {
            // Arrange
            var title = "title";
            nugetService.Setup(x => x.MoveOldPackageToNugetPackagesFile(title)).Returns(value);

            // Act
            var result = controller.MoveOldPackageToNugetPackagesFile(title);

            // Assert
            result.Should().Be(value);
        }

        #endregion

        #region upload new nuget package

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void UploadNewNugetPackage_TestCase_ReturnTestCaseResult(bool value)
        {
            // Arrange
            var fileName = "title";
            nugetService.Setup(x => x.UploadNewNugetPackage(fileName)).Returns(value);

            // Act
            var result = controller.UploadNewNugetPackage(fileName);

            // Assert
            result.Should().Be(value);
        }

        #endregion
    }
}