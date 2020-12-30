using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationNugetPackageServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationNugetPackageRepository> repository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        StrictMock<INugetPackageService> nugetPackageService;

        ApplicationNugetPackageService service;

        [SetUp]
        public void Initialize()
        {
            repository = new StrictMock<IApplicationNugetPackageRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();
            nugetPackageService = new StrictMock<INugetPackageService>();

            service = new ApplicationNugetPackageService(
                repository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object,
                nugetPackageService.Object

                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            repository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
            nugetPackageService.VerifyAll();
        }

        #endregion

        #region Get Nuget Packages

        [Test]
        public void GetNugetPackages_NoCondition_ReturnPackageList()
        {
            // Arrange
            var applicationId = 1;
            var package1 = new ApplicationNugetPackage { NugetPackageName = "packageName-1" };
            var package2 = new ApplicationNugetPackage { NugetPackageName = "packageName-2" };
            var nugetPackageList = new List<ApplicationNugetPackage> {
                package1,
                package2
            };
            var nugetPackage1Url = new Uri("http://www.example.com/packageUrl/packageName-1");
            var nugetPackage2Url = new Uri("http://www.example.com/packageUrl/packageName-2");

            nugetPackageService.Setup(x => x.GetNugetPackageUrl(package1.NugetPackageName)).Returns(nugetPackage1Url);
            nugetPackageService.Setup(x => x.GetNugetPackageUrl(package2.NugetPackageName)).Returns(nugetPackage2Url);
            repository.Setup(x => x.GetNugetPackages(applicationId)).Returns(nugetPackageList);

            // Act
            var result = service.GetNugetPackages(applicationId);

            // Assert
            result.Should().BeSameAs(nugetPackageList);
        }

        #endregion

        #region GetApplicationNugetPackageById

        [Test]
        public void GetApplicationNugetPackageById_NoCondition_ReturnApplicationNugetPackage()
        {
            // Arrange
            var packageId = 1;
            var package = new ApplicationNugetPackage();
            var recordUpdateInfo = new RecordUpdateInfo();

            repository.Setup(x => x.GetApplicationNugetPackageById(packageId)).Returns(package);
            repository.Setup(x => x.GetPackageUpdateInfo(packageId)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetApplicationNugetPackage(packageId);

            // Assert
            result.Should().Be(package);
        }

        #endregion

        #region Add Application Nuget Package

        [Test]
        public void AddApplicationNugetPackage_AddApplicationNugetPackageIsExits_ReturnServiceErrorResult()
        {
            // Arrange
            var package = new ApplicationNugetPackage();
            var serviceResult = ServiceResult.Error(Messages.ApplicationNugetPackageNameExists);

            repository.Setup(x => x.GetApplicationNugetPackageByName(package.NugetPackageName)).Returns(package);

            // Act
            var result = service.AddApplicationNugetPackage(package);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationNugetPackage_AddingFails_ReturnServiceErrorResult()
        {
            // Arrange
            var package = new ApplicationNugetPackage();
            ApplicationNugetPackage resultPackage = null;
            var addedResult = false;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            repository.Setup(x => x.GetApplicationNugetPackageByName(package.NugetPackageName)).Returns(resultPackage);
            repository.Setup(x => x.AddApplicationNugetPackage(package)).Returns(addedResult);

            // Act
            var result = service.AddApplicationNugetPackage(package);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationNugetPackage_AddingSucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var package = new ApplicationNugetPackage();
            ApplicationNugetPackage resultPackage = null;
            var addedResult = true;
            var serviceResult = ServiceResult.Success(Messages.ApplicationNugetPackageCreated);

            repository.Setup(x => x.GetApplicationNugetPackageByName(package.NugetPackageName)).Returns(resultPackage);
            repository.Setup(x => x.AddApplicationNugetPackage(package)).Returns(addedResult);

            // Act
            var result = service.AddApplicationNugetPackage(package);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region UpdateApplicationNugetPackage

        [Test]
        public void UpdateApplicationNugetPackage_ApplicationNugetPackageIsNull_ReturnServiceErrorResult()
        {
            // Arrange
            ApplicationNugetPackage appNugetPackage = null;

            // Act
            var result = service.UpdateApplicationNugetPackage(appNugetPackage);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationNugetPackage_ApplicationNugetPackageHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var appNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };
            var oldAppNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };

            repository.Setup(x => x.GetApplicationNugetPackageById(appNugetPackage.NugetPackageId)).Returns(oldAppNugetPackage);
            auditService.Setup(x => x.IsChanged(oldAppNugetPackage, appNugetPackage, nameof(ApplicationNugetPackage))).Returns(false);

            // Act
            var result = service.UpdateApplicationNugetPackage(appNugetPackage);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationNugetPackageUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationNugetPackage_ApplicationNugetPackageUpdateFails_ReturnServerError()
        {
            // Arrange
            var appNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };
            var oldAppNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };

            repository.Setup(x => x.GetApplicationNugetPackageById(appNugetPackage.NugetPackageId)).Returns(oldAppNugetPackage);
            auditService.Setup(x => x.IsChanged(oldAppNugetPackage, appNugetPackage, nameof(ApplicationNugetPackage))).Returns(true);
            repository.Setup(x => x.UpdateApplicationNugetPackage(appNugetPackage)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationNugetPackage(appNugetPackage);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationNugetPackage_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var appNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };
            var oldAppNugetPackage = new ApplicationNugetPackage();
            var auditInfo = new AuditInfo();

            repository.Setup(x => x.GetApplicationNugetPackageById(appNugetPackage.NugetPackageId)).Returns(oldAppNugetPackage);
            auditService.Setup(x => x.IsChanged(oldAppNugetPackage, appNugetPackage, nameof(ApplicationNugetPackage))).Returns(true);
            repository.Setup(x => x.UpdateApplicationNugetPackage(appNugetPackage)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationNugetPackage), oldAppNugetPackage.NugetPackageId, oldAppNugetPackage, oldAppNugetPackage, appNugetPackage.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationNugetPackage(appNugetPackage);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationNugetPackage_UpdateApplicationSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var appNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };
            var oldAppNugetPackage = new ApplicationNugetPackage { NugetPackageId = 3, NugetPackageName = "Test" };

            var auditInfo = new AuditInfo();

            repository.Setup(x => x.GetApplicationNugetPackageById(appNugetPackage.NugetPackageId)).Returns(oldAppNugetPackage);
            auditService.Setup(x => x.IsChanged(oldAppNugetPackage, appNugetPackage, nameof(ApplicationNugetPackage))).Returns(true);
            repository.Setup(x => x.UpdateApplicationNugetPackage(appNugetPackage)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationNugetPackage), oldAppNugetPackage.NugetPackageId, oldAppNugetPackage, oldAppNugetPackage, appNugetPackage.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationNugetPackage(appNugetPackage);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationNugetPackageUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region DeleteApplicationNugetPackage

        [Test]
        public void DeleteApplicationNugetPackage_DeleteFails_ReturnServiceErrorResult()
        {
            // Arrange
            var packageId = 1;
            var updateResult = false;

            repository.Setup(x => x.DeleteApplicationNugetPackage(packageId)).Returns(updateResult);

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            // Act
            var result = service.DeleteApplicationNugetPackage(packageId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationNugetPackage_DeleteSucceeds_ReturnServiceSuccessResult()
        {
            var packageId = 1;
            var updateResult = true;

            repository.Setup(x => x.DeleteApplicationNugetPackage(packageId)).Returns(updateResult);

            var serviceResult = ServiceResult.Success(Messages.ApplicationNugetPackageDeleted);

            // Act
            var result = service.DeleteApplicationNugetPackage(packageId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}