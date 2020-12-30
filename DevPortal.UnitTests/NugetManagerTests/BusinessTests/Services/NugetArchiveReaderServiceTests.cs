using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetArchiveReaderServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetServerRepository> nugetServerRepository;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IFileSystemService> fileSystemService;

        NugetArchiveReaderService service;

        [SetUp]
        public void Initialize()
        {
            nugetServerRepository = new StrictMock<INugetServerRepository>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            fileSystemService = new StrictMock<IFileSystemService>();

            service = new NugetArchiveReaderService(
                nugetServerRepository.Object,
                generalSettingsService.Object,
                fileSystemService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetServerRepository.VerifyAll();
            generalSettingsService.VerifyAll();
            fileSystemService.VerifyAll();
        }

        #endregion

        [Test]
        public void GetArchivedNugetPackages_NoCondition_ReturnNugetPackageFolderList()
        {
            // Arrange
            var path = "//path";
            var list = new List<string>();
            var nugetPackageFolders = new List<NugetPackageFolder>
            {
                new NugetPackageFolder { Path = path,SubDirectory=list }
            };

            generalSettingsService.Setup(x => x.GetNugetPackageArchiveFolderPath()).Returns(path);
            nugetServerRepository.Setup(x => x.GetNugetPackagesFolder(path)).Returns(nugetPackageFolders);
            nugetServerRepository.Setup(x => x.GetSubDirectoryFoldersName(nugetPackageFolders[0].FilePath)).Returns(list);

            //Act
            var result = service.GetArchivedNugetPackages();

            // Assert
            result.Should().NotBeNull();
            result[0].SubDirectory.GetType().Should().Be(list.GetType());
        }

        [Test]
        public void SetPackageArchiveStatus_NugetPackagesIsNull_ReturnVoid()
        {
            // Arrange
            List<NugetPackage> nugetPackages = null;
            var archivePackages = new NugetPackageFolder();

            //Act
            service.SetPackageArchiveStatus(nugetPackages, archivePackages);

            // Assert
        }

        [Test]
        public void SetPackageArchiveStatus_ArchivePackagesIsNull_ReturnVoid()
        {
            // Arrange
            var nugetPackages = new List<NugetPackage>();
            NugetPackageFolder archivePackages = null;

            //Act
            service.SetPackageArchiveStatus(nugetPackages, archivePackages);

            // Assert
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetPackageArchiveStatus_VersionFolderExists_ReturnVoid(bool returnValue)
        {
            // Arrange
            var nugetPackages = new List<NugetPackage>
            {
                new NugetPackage
                {
                    Properties= new NugetProperties
                    {
                        Archive = true,
                        Version = "1.0.0",
                        Title ="title"
                    }
                }
            };
            var archivePackages = new NugetPackageFolder
            {
                SubDirectory = new List<string>
                {
                    "1.0.0"
                },
                Path = "path"
            };
            var versionItem = nugetPackages[0].Properties.Version;
            var path = $"{archivePackages.FilePath}/{versionItem}";
            var fileName = $"{nugetPackages[0].Title}.{versionItem}.nupkg";
            var filePath = Path.Combine(path, fileName);

            fileSystemService.Setup(x => x.DoesFileExists(filePath)).Returns(returnValue);

            //Act
            service.SetPackageArchiveStatus(nugetPackages, archivePackages);

            // Assert
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsPackageArchived_NoCondition_ReturnBool(bool returnValue)
        {
            // Arrange
            var filePath = "filePath";

            fileSystemService.Setup(x => x.DoesFileExists(filePath)).Returns(returnValue);

            //Act
            var result = service.IsPackageArchived(filePath);

            // Assert
            result.Should().Be(returnValue);
        }
    }
}