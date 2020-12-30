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

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseWriterServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDatabaseRepository> databaseRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        DatabaseWriterService service;

        [SetUp]
        public void Initialize()
        {
            databaseRepository = new StrictMock<IDatabaseRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new DatabaseWriterService(
                databaseRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region edit database

        [Test]
        public void EditDatabase_DatabaseNull_ReturnServiceErrorResult()
        {
            // Arrange
            Database database = null;

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditDatabase_DatabaseExists_ReturnDatabaseNameExistsError()
        {
            // Arrange
            var database = new Database { Id = 1, Name = "Test" };
            var data = new Database { Id = 2, Name = "Test" };

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(data);

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.DatabaseNameExists);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditDatabase_DatabaseHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var database = new Database
            {
                Id = 3,
                Name = "Test",
                DatabaseGroupId = 1,
                DatabaseTypeId = 2,
                RedmineProjectName = "redmine",
                Description = null
            };
            var data = new Database
            {
                Id = 3,
                Name = "Test"
            };
            var oldDatabase = new Database
            {
                Id = 3,
                Name = "Test",
                DatabaseGroupId = 1,
                DatabaseTypeId = 2,
                RedmineProjectName = "redmine",
                Description = null
            };

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(data);
            databaseRepository.Setup(x => x.GetDatabase(database.Id)).Returns(oldDatabase);
            auditService.Setup(x => x.IsChanged(oldDatabase, database, nameof(Database))).Returns(false);

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditDatabase_DatabaseDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var database = new Database
            {
                Id = 1,
                Name = "Test",
                Description = null
            };
            var data = new Database
            {
                Id = 1,
                Name = "Test"
            };
            var oldDatabase = new Database
            {
                Id = 1,
                Name = "Test",
                Description = "asd"
            };

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(data);
            databaseRepository.Setup(x => x.GetDatabase(database.Id)).Returns(oldDatabase);
            auditService.Setup(x => x.IsChanged(oldDatabase, database, nameof(Database))).Returns(true);
            databaseRepository.Setup(x => x.UpdateDatabase(database)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditDatabase_DatabaseDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var database = new Database
            {
                Id = 1,
                Name = "Test"
            };
            var data = new Database
            {
                Id = 1,
                Name = "Testt"
            };
            var oldDatabase = new Database();
            var auditInfo = new AuditInfo();

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(data);
            databaseRepository.Setup(x => x.GetDatabase(database.Id)).Returns(oldDatabase);
            auditService.Setup(x => x.IsChanged(oldDatabase, database, nameof(Database))).Returns(true);
            databaseRepository.Setup(x => x.UpdateDatabase(database)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Database), oldDatabase.Id, oldDatabase, oldDatabase, database.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditDatabase_DatabaseUpdateSuccess_ReturnDatabaseUpdated()
        {
            // Arrange
            var database = new Database
            {
                Id = 1,
                Name = "Test"
            };
            var data = new Database
            {
                Id = 1,
                Name = "Testt"
            };
            var oldDatabase = new Database();
            var auditInfo = new AuditInfo();

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(data);
            databaseRepository.Setup(x => x.GetDatabase(database.Id)).Returns(oldDatabase);
            auditService.Setup(x => x.IsChanged(oldDatabase, database, nameof(Database))).Returns(true);
            databaseRepository.Setup(x => x.UpdateDatabase(database)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Database), oldDatabase.Id, oldDatabase, oldDatabase, database.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditDatabase_DatabaseWithNewNameDoesNotExistAndDatabaseUpdateSuccess_ReturnDatabaseUpdated()
        {
            // Arrange
            var database = new Database
            {
                Id = 1,
                Name = "Test"
            };
            var data = new Database
            {
                Id = 1,
                Name = "Testt"
            };
            Database existingDatabase = null;
            Database oldDatabase = new Database();
            var auditInfo = new AuditInfo();

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(existingDatabase);
            databaseRepository.Setup(x => x.GetDatabase(database.Id)).Returns(oldDatabase);
            auditService.Setup(x => x.IsChanged(oldDatabase, database, nameof(Database))).Returns(true);
            databaseRepository.Setup(x => x.UpdateDatabase(database)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Database), oldDatabase.Id, oldDatabase, oldDatabase, database.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateDatabase(database);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region add database

        [Test]
        public void AddDatabase_DatabaseExists_ReturnDatabase()
        {
            // Arrange
            var database = new Database();
            Database databaseModel = new Database();
            var serviceResult = ServiceResult.Error(Messages.DatabaseFound);

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(databaseModel);

            // Act
            var result = service.AddDatabase(database);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabase_DatabaseIdZero_ReturnDatabase()
        {
            // Arrange
            var database = new Database();
            Database databaseModel = null;
            var serviceResult = ServiceResult.Error(Messages.DatabaseAdded);
            var databaseId = 0;

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(databaseModel);
            databaseRepository.Setup(x => x.AddDatabase(database)).Returns(databaseId);

            // Act
            var result = service.AddDatabase(database);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabase_Success_ReturnDatabase()
        {
            // Arrange
            var database = new Database();
            Database databaseModel = null;
            var serviceResult = ServiceResult.Success(Messages.AddingSucceeds);
            var databaseId = 1;

            databaseRepository.Setup(x => x.GetDatabaseByDatabaseName(database.Name)).Returns(databaseModel);
            databaseRepository.Setup(x => x.AddDatabase(database)).Returns(databaseId);

            // Act
            var result = service.AddDatabase(database);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabase_DatabaseIsNull_ReturnGeneralError()
        {
            // Arrange
            Database database = null;
            var serviceResult = ServiceResult.Error(Messages.GeneralError);

            // Act
            var result = service.AddDatabase(database);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region delete database

        [Test]
        public void DeleteDatabase_Fails_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseId = 1;
            var deleteResult = false;

            databaseRepository.Setup(x => x.DeleteDatabase(databaseId)).Returns(deleteResult);

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            // Act
            var result = service.DeleteDatabase(databaseId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteDatabase_Succeeds_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseId = 1;
            var deleteResult = true;

            databaseRepository.Setup(x => x.DeleteDatabase(databaseId)).Returns(deleteResult);

            var serviceResult = ServiceResult.Success(Messages.DatabaseDeleted);

            // Act
            var result = service.DeleteDatabase(databaseId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}