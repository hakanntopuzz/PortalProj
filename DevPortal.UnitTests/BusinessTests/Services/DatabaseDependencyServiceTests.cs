using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
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
    public class DatabaseDependencyServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDatabaseDependencyRepository> databaseDependencyRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        DatabaseDependencyService service;

        [SetUp]
        public void Initialize()
        {
            databaseDependencyRepository = new StrictMock<IDatabaseDependencyRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new DatabaseDependencyService(
                databaseDependencyRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseDependencyRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get database dependency by id

        [Test]
        public void GetDatabaseDependencyById_DatabaseDependencyExists_ReturnDatabaseDependency()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency();
            var id = 1;

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(id)).Returns(databaseDependency);
            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyUpdateInfo(id)).Returns(databaseDependency.RecordUpdateInfo);

            // Act
            var result = service.GetDatabaseDependency(id);

            // Assert
            result.Should().BeSameAs(databaseDependency);
        }

        [Test]
        public void GetDatabaseDependencyById_DatabaseDependencyDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            DatabaseDependency databaseDependency = null;

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(id)).Returns(databaseDependency);

            // Act

            var result = service.GetDatabaseDependency(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region add database dependency

        [Test]
        public void AddDatabaseDependency_Error_ReturnServiceResult()
        {
            // Arrange
            DatabaseDependency databaseDependency = new DatabaseDependency();
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            databaseDependencyRepository.Setup(x => x.AddDatabaseDependency(databaseDependency)).Returns(false);

            // Act
            var result = service.AddDatabaseDependency(databaseDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabaseDependency_Success_ReturnServiceResult()
        {
            // Arrange
            DatabaseDependency databaseDependency = new DatabaseDependency();
            var serviceResult = ServiceResult.Success(Messages.DatabaseDependencyCreated);

            databaseDependencyRepository.Setup(x => x.AddDatabaseDependency(databaseDependency)).Returns(true);

            // Act
            var result = service.AddDatabaseDependency(databaseDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region update database dependency

        [Test]
        public void UpdateDatabaseDependency_ParameterIsNull_ReturnServiceResult()
        {
            // Arrange
            DatabaseDependency databaseDependency = null;
            var serviceResult = ServiceResult.Error(Messages.NullParameterError);

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseDependency_DatabaseDependencyHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var oldDatabaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(databaseDependency.Id)).Returns(oldDatabaseDependency);
            auditService.Setup(x => x.IsChanged(oldDatabaseDependency, databaseDependency, nameof(DatabaseDependency))).Returns(false);

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseDependencyUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseDependency_DatabaseDependencyUpdateFails_ReturnServerError()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var oldDatabaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(databaseDependency.Id)).Returns(oldDatabaseDependency);
            auditService.Setup(x => x.IsChanged(oldDatabaseDependency, databaseDependency, nameof(DatabaseDependency))).Returns(true);
            databaseDependencyRepository.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseDependency_DatabaseDependencyAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var oldDatabaseDependency = new DatabaseDependency();

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(databaseDependency.Id)).Returns(oldDatabaseDependency);
            auditService.Setup(x => x.IsChanged(oldDatabaseDependency, databaseDependency, nameof(DatabaseDependency))).Returns(true);
            databaseDependencyRepository.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(true);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseDependency_UpdateDatabaseDependencyError_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var oldDatabaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };

            var serviceResult = ServiceResult.Error(Messages.UpdateFails);

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(databaseDependency.Id)).Returns(oldDatabaseDependency);
            auditService.Setup(x => x.IsChanged(oldDatabaseDependency, databaseDependency, nameof(DatabaseDependency))).Returns(true);
            databaseDependencyRepository.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(true);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseDependency_UpdateDatabaseDependencyNotSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var oldDatabaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            var serviceResult = ServiceResult.Error(Messages.UpdateFails);

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(databaseDependency.Id)).Returns(oldDatabaseDependency);
            auditService.Setup(x => x.IsChanged(oldDatabaseDependency, databaseDependency, nameof(DatabaseDependency))).Returns(true);
            databaseDependencyRepository.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(DatabaseDependency), databaseDependency.Id, oldDatabaseDependency, oldDatabaseDependency, databaseDependency.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseDependency_UpdateDatabaseDependencySucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var databaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var oldDatabaseDependency = new DatabaseDependency { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            var serviceResult = ServiceResult.Success(Messages.DatabaseDependencyUpdated);

            databaseDependencyRepository.Setup(x => x.GetDatabaseDependencyById(databaseDependency.Id)).Returns(oldDatabaseDependency);
            auditService.Setup(x => x.IsChanged(oldDatabaseDependency, databaseDependency, nameof(DatabaseDependency))).Returns(true);
            databaseDependencyRepository.Setup(x => x.UpdateDatabaseDependency(databaseDependency)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(DatabaseDependency), databaseDependency.Id, oldDatabaseDependency, oldDatabaseDependency, databaseDependency.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateDatabaseDependency(databaseDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region  delete database dependency

        [Test]
        public void DeleteDatabaseDependency_NoCondition_ReturnTrue()
        {
            // Arrange
            var id = 5;

            // Act
            databaseDependencyRepository.Setup(x => x.DeleteDatabaseDependency(id)).Returns(true);

            var serviceResult = ServiceResult.Success(Messages.DatabaseDependencyDeleted);

            var result = service.DeleteDatabaseDependency(id);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteDatabaseDependency_NoCondition_ReturnFalse()
        {
            // Arrange
            var id = 5;

            // Act
            databaseDependencyRepository.Setup(x => x.DeleteDatabaseDependency(id)).Returns(false);

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            var result = service.DeleteDatabaseDependency(id);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region get database dependencies by application id

        [Test]
        public void GetDatabaseDependenciesByApplicationId_NotNull_ReturnDatabase()
        {
            // Arrange
            var applicationId = 5;
            ICollection<DatabaseDependency> databases = new List<DatabaseDependency>();

            // Act
            databaseDependencyRepository.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(databases);

            var result = service.GetDatabaseDependenciesByApplicationId(applicationId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetDatabaseDependenciesByApplicationId_Null_ReturnDatabaseIsNull()
        {
            // Arrange
            var applicationId = 5;
            ICollection<DatabaseDependency> databases = null;

            // Act
            databaseDependencyRepository.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(databases);

            var result = service.GetDatabaseDependenciesByApplicationId(applicationId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetFullDatabaseDependenciesByApplicationId

        [Test]
        public void GetFullDatabaseDependenciesByApplicationId_NoCondition_ReturnDatabaseExportListItems()
        {
            // Arrange
            var applicationId = 5;
            var databaseExportListItems = new List<DatabaseDependenciesExportListItem>();

            databaseDependencyRepository.Setup(x => x.GetFullDatabaseDependenciesByApplicationId(applicationId)).Returns(databaseExportListItems);

            // Act
            var result = service.GetFullDatabaseDependenciesByApplicationId(applicationId);

            // Assert
            result.Should().BeEquivalentTo(databaseExportListItems);
        }

        #endregion
    }
}