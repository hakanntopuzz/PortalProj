using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.Web.Library.Abstract;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CachedNugetServiceTests : BaseTestFixture
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

        CachedNugetService service;

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

            service = new CachedNugetService(
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
            nugetQueryService.VerifyAll();
            nugetArchiveReaderService.VerifyAll();
            fileSystemService.VerifyAll();
        }

        #endregion

        [Test]
        public void GetAllNugetPackages_PackagesIsNotNull_ReturnNugetPackageList()
        {
            // Arrange
            var packages = new List<NugetPackage>();

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

            cacheWrapper.Setup(x => x.Get<List<NugetPackage>>(nameof(CacheKeyNames.NugetPackageList))).Returns(packages);
            nugetQueryService.Setup(x => x.GetGroupedNugetPackages(allPackages)).Returns(groupedPackages);
            nugetQueryService.Setup(x => x.OrderPackages(groupedPackages, orderBy)).Returns(orderedPackages);
            nugetQueryService.Setup(x => x.SearchByTagOrTitle(orderedPackages, searchString)).Returns(searchedPackages);
            nugetFactory.Setup(x => x.CreateFilteredNugetPackagesModel(0, filteredNugetPackages)).Returns(filteredNugetPackagesModel);

            //Act
            var result = service.GetFilteredNugetPackages(skip, take, searchString, orderBy);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetAllNugetPackages_PackagesIsNull_ReturnNugetPackageList()
        {
            // Arrange
            List<NugetPackage> packages = null;
            var packageList = new List<NugetPackage>();
            var minute = 5;
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

            cacheWrapper.Setup(x => x.Get<List<NugetPackage>>(nameof(CacheKeyNames.NugetPackageList))).Returns(packages);
            nugetPackageSerializationService.Setup(x => x.GetAllNugetPackages()).Returns(allPackages);
            settings.Setup(x => x.NugetPackageListCacheTimeInMinutes).Returns(minute);
            cacheWrapper.Setup(x => x.AddWithAbsoluteExpiration(nameof(CacheKeyNames.NugetPackageList), packageList, minute));
            nugetQueryService.Setup(x => x.GetGroupedNugetPackages(allPackages)).Returns(groupedPackages);
            nugetQueryService.Setup(x => x.OrderPackages(groupedPackages, orderBy)).Returns(orderedPackages);
            nugetQueryService.Setup(x => x.SearchByTagOrTitle(orderedPackages, searchString)).Returns(searchedPackages);
            nugetFactory.Setup(x => x.CreateFilteredNugetPackagesModel(0, filteredNugetPackages)).Returns(filteredNugetPackagesModel);

            //Act
            var result = service.GetFilteredNugetPackages(skip, take, searchString, orderBy);

            //Assert
            result.Should().NotBeNull();
        }
    }
}