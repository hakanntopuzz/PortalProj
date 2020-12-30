using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDependencyReaderServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationRepository> applicationRepository;

        StrictMock<IApplicationDependencyRepository> applicationDependencyRepository;

        StrictMock<INugetPackageService> nugetPackageService;

        ApplicationDependencyReaderService service;

        [SetUp]
        public void Initialize()
        {
            applicationRepository = new StrictMock<IApplicationRepository>();
            applicationDependencyRepository = new StrictMock<IApplicationDependencyRepository>();
            nugetPackageService = new StrictMock<INugetPackageService>();

            service = new ApplicationDependencyReaderService(
                applicationRepository.Object,
                applicationDependencyRepository.Object,
                nugetPackageService.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationRepository.VerifyAll();
            applicationDependencyRepository.VerifyAll();
            nugetPackageService.VerifyAll();
        }

        #endregion

        #region  get application dependency by id

        [Test]
        public void GetApplicationDependencyById_ApplicationDependencyExists_ReturnApplicationDependency()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency();
            var id = 1;

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(id)).Returns(applicationDependency);
            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyUpdateInfo(id)).Returns(applicationDependency.RecordUpdateInfo);

            // Act
            var result = service.GetApplicationDependency(id);

            // Assert
            result.Should().BeSameAs(applicationDependency);
        }

        [Test]
        public void GetApplicationDependencyById_ApplicationDependencyDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            ApplicationDependency applicationDependency = null;

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(id)).Returns(applicationDependency);

            // Act

            var result = service.GetApplicationDependency(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get application dependencies by application id

        [Test]
        public void GetApplicationDependenciesByApplicationId_NoCondition_ReturnApplication()
        {
            // Arrange
            var applicationId = 1;
            var application = new List<ApplicationDependency>();

            applicationRepository.Setup(x => x.GetApplicationDependenciesByApplicationId(applicationId)).Returns(application);

            // Act
            var result = service.GetApplicationDependencies(applicationId);

            // Assert
            result.Should().BeEquivalentTo(application);
        }

        #endregion

        #region get external dependencies by application id

        [Test]
        public void GetExternalDependenciesByApplicationId_NoCondition_ReturnExternalDependencies()
        {
            // Arrange
            var applicationId = 5;
            var externalDependencies = new List<ExternalDependency>();

            applicationRepository.Setup(x => x.GetExternalDependenciesByApplicationId(applicationId)).Returns(externalDependencies);

            // Act
            var result = service.GetExternalDependencies(applicationId);

            // Assert
            result.Should().BeEquivalentTo(externalDependencies);
        }

        [Test]
        public void GetExternalDependenciesByApplicationId_NoCondition_ReturnExternalDependenciesIsNull()
        {
            // Arrange
            var applicationId = 5;
            ICollection<ExternalDependency> externalDependencies = null;

            applicationRepository.Setup(x => x.GetExternalDependenciesByApplicationId(applicationId)).Returns(externalDependencies);

            // Act
            var result = service.GetExternalDependencies(applicationId);

            // Assert
            result.Should().BeEquivalentTo(externalDependencies);
        }

        #endregion

        #region GetApplicationDependencies

        [Test]
        public void GetApplicationDependencies_NoCondition_ReturnApplicationExportListItems()
        {
            // Arrange
            var applicationId = 5;
            var applicationExportListItems = new List<ApplicationDependenciesExportListItem>();

            applicationRepository.Setup(x => x.GetApplicationDependencies(applicationId)).Returns(applicationExportListItems);

            // Act
            var result = service.GetApplicationDependenciesExportList(applicationId);

            // Assert
            result.Should().BeEquivalentTo(applicationExportListItems);
        }

        #endregion

        #region GetFullExternalDependenciesByApplicationId

        [Test]
        public void GetFullExternalDependenciesByApplicationId_NoCondition_ReturnExternalExportListItems()
        {
            // Arrange
            var applicationId = 5;
            var externalExportListItems = new List<ExternalDependenciesExportListItem>();

            applicationRepository.Setup(x => x.GetFullExternalDependenciesByApplicationId(applicationId)).Returns(externalExportListItems);

            // Act
            var result = service.GetExternalDependenciesExportList(applicationId);

            // Assert
            result.Should().BeEquivalentTo(externalExportListItems);
        }

        #endregion

        #region GetDatabaseDependencies

        [Test]
        public void GetDatabaseDependencies_NoCondition_ReturnApplicationExportListItems()
        {
            // Arrange
            var applicationId = 5;
            var applicationExportListItems = new List<DatabaseDependenciesExportListItem>();

            applicationRepository.Setup(x => x.GetDatabaseDependencies(applicationId)).Returns(applicationExportListItems);

            // Act
            var result = service.GetDatabaseDependenciesExportList(applicationId);

            // Assert
            result.Should().BeEquivalentTo(applicationExportListItems);
        }

        #endregion

        #region get nuget package dependencies by application id

        [Test]
        public void GetNugetPackageDependenciesByApplicationId_NoCondition_ReturnApplication()
        {
            // Arrange
            var applicationId = 1;
            var packageName = "package";

            var nugetPackageDependencies = new List<NugetPackageDependency> {
            new NugetPackageDependency{
                NugetPackageName = "package"
                }
            };
            var nugetPackageUrl = new Uri("http://www.example.com/packageUrl/");

            nugetPackageService.Setup(x => x.GetNugetPackageUrl(packageName)).Returns(nugetPackageUrl);
            applicationRepository.Setup(x => x.GetNugetPackageDependenciesByApplicationId(applicationId)).Returns(nugetPackageDependencies);

            // Act
            var result = service.GetNugetPackageDependencies(applicationId);

            // Assert
            result.Should().BeEquivalentTo(nugetPackageDependencies);
        }

        #endregion

        #region get nuget package dependencies

        [Test]
        public void GetNugetPackageDependencies_NoCondition_ReturnApplication()
        {
            // Arrange
            var applicationId = 1;
            var packageName = "package";

            var nugetPackageDependencies = new List<NugetPackageDependenciesExportListItem>
            {
                new NugetPackageDependenciesExportListItem
                {
                    Name = "package"
                }
            };
            var nugetPackageUrl = new Uri("http://www.example.com/packageUrl/");

            nugetPackageService.Setup(x => x.GetNugetPackageUrl(packageName)).Returns(nugetPackageUrl);
            applicationRepository.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetPackageDependencies);

            // Act
            var result = service.GetNugetPackageDependenciesExportList(applicationId);

            // Assert
            result.Should().BeEquivalentTo(nugetPackageDependencies);
        }

        #endregion
    }
}