using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseTypeServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDatabaseTypeRepository> databaseTypeRepository;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        DatabaseTypeService service;

        [SetUp]
        public void Initialize()
        {
            databaseTypeRepository = new StrictMock<IDatabaseTypeRepository>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new DatabaseTypeService(
                databaseTypeRepository.Object,
                urlHelper.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseTypeRepository.VerifyAll();
            urlHelper.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get database types

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturntDatabaseTypeList()
        {
            // Arrange
            ICollection<DatabaseType> databaseTypes = null;

            databaseTypeRepository.Setup(x => x.GetDatabaseTypes()).Returns(databaseTypes);

            // Act
            var result = service.GetDatabaseTypes();

            // Assert
            result.Should().BeSameAs(databaseTypes);
        }

        #endregion

        #region add database type

        [Test]
        public void AddDatabaseType_DatabaseTypeExists_ReturnServiceResult()
        {
            // Arrange
            DatabaseType databaseType = new DatabaseType();
            DatabaseType databaseTypeModel = new DatabaseType();
            var serviceResult = ServiceResult.Error(Messages.DatabaseTypeFound);

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseTypeModel);

            // Act
            var result = service.AddDatabaseType(databaseType);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabaseType_DatabaseTypeIdZero_ReturnServiceResult()
        {
            // Arrange
            DatabaseType databaseType = new DatabaseType();
            DatabaseType databaseTypeModel = null;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseTypeModel);
            databaseTypeRepository.Setup(x => x.AddDatabaseType(databaseType)).Returns(false);

            // Act
            var result = service.AddDatabaseType(databaseType);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabaseType_Success_ReturnServiceResult()
        {
            // Arrange
            DatabaseType databaseType = new DatabaseType();
            DatabaseType databaseTypeModel = null;
            var serviceResult = ServiceResult.Success(Messages.AddingSucceeds);

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseTypeModel);
            databaseTypeRepository.Setup(x => x.AddDatabaseType(databaseType)).Returns(true);

            // Act
            var result = service.AddDatabaseType(databaseType);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region update database type

        [Test]
        public void UpdateDatabaseType_DatabaseTypeIsNull_ReturnServiceResultError()
        {
            // Arrange
            DatabaseType databaseType = null;
            var serviceResult = ServiceResult.Error(Messages.NullParameterError);

            // Act
            var result = service.UpdateDatabaseType(databaseType);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseType_DatabaseTypeExists_ReturnServiceResultError()
        {
            // Arrange
            DatabaseType databaseType = new DatabaseType { Id = 5 };
            DatabaseType databaseTypeModel = new DatabaseType { Id = 4 };
            var serviceResult = ServiceResult.Error(Messages.DatabaseTypeFound);

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseTypeModel);

            // Act
            var result = service.UpdateDatabaseType(databaseType);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseType_DatabaseTypeHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            DatabaseType databaseType = new DatabaseType { Id = 5, Name = "type" };
            var oldDatabaseType = new DatabaseType { Id = 5 };

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseType);
            databaseTypeRepository.Setup(x => x.GetDatabaseTypeById(databaseType.Id)).Returns(oldDatabaseType);
            auditService.Setup(x => x.IsChanged(oldDatabaseType, databaseType, nameof(DatabaseType))).Returns(false);

            // Act
            var result = service.UpdateDatabaseType(databaseType);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseTypeUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseType_DatabaseTypeDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var databaseType = new DatabaseType { Id = 2, Name = "type" };
            var oldDatabaseType = new DatabaseType { Id = 5 };

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseType);
            databaseTypeRepository.Setup(x => x.GetDatabaseTypeById(databaseType.Id)).Returns(oldDatabaseType);
            auditService.Setup(x => x.IsChanged(oldDatabaseType, databaseType, nameof(DatabaseType))).Returns(true);
            databaseTypeRepository.Setup(x => x.UpdateDatabaseType(databaseType)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseType(databaseType);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseType_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var databaseType = new DatabaseType { Id = 2, Name = "type" };
            var oldDatabaseType = new DatabaseType();
            var auditInfo = new AuditInfo();

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseType);
            databaseTypeRepository.Setup(x => x.GetDatabaseTypeById(databaseType.Id)).Returns(oldDatabaseType);
            auditService.Setup(x => x.IsChanged(oldDatabaseType, databaseType, nameof(DatabaseType))).Returns(true);
            databaseTypeRepository.Setup(x => x.UpdateDatabaseType(databaseType)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(DatabaseType), oldDatabaseType.Id, oldDatabaseType, oldDatabaseType, databaseType.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseType(databaseType);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseType_UpdateDatabaseTypeSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseType = new DatabaseType { Id = 2, Name = "type" };
            var oldDatabaseType = new DatabaseType { Id = 5 };
            var serviceResult = ServiceResult.Success(Messages.DatabaseTypeUpdated);
            var auditInfo = new AuditInfo();

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeByName(databaseType.Name)).Returns(databaseType);
            databaseTypeRepository.Setup(x => x.GetDatabaseTypeById(databaseType.Id)).Returns(oldDatabaseType);
            auditService.Setup(x => x.IsChanged(oldDatabaseType, databaseType, nameof(DatabaseType))).Returns(true);
            databaseTypeRepository.Setup(x => x.UpdateDatabaseType(databaseType)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(DatabaseType), oldDatabaseType.Id, oldDatabaseType, oldDatabaseType, databaseType.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateDatabaseType(databaseType);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region delete database type

        [Test]
        public void DeleteDatabaseType_DeleteDatabaseTypeFails_ReturnRedirectableClientActionResult()
        {
            // Arrange
            var datatabletypeId = 1;
            var relatedDatabaseCount = 0;
            var redirectUrl = "redirectUrl";
            var serviceResult = RedirectableClientActionResult.Error(redirectUrl, Messages.DeleteFails);
            var deleteResult = false;

            SetupGetDatabaseCountByDatabaseTypeId(datatabletypeId, relatedDatabaseCount);
            SetupGenerateUrl(redirectUrl);
            databaseTypeRepository.Setup(x => x.DeleteDatabaseType(datatabletypeId)).Returns(deleteResult);

            // Act
            var result = service.DeleteDatabaseType(datatabletypeId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteDatabaseType_RelatedApplicationEnvironmentsExists_ReturnRedirectableClientActionResult()
        {
            // Arrange
            var datatabletypeId = 1;
            var relatedDatabaseCount = 1;
            var redirectUrl = "redirectUrl";
            var serviceResult = RedirectableClientActionResult.Error(redirectUrl, Messages.RelatedDatabaseTypeDatabaseExists);

            SetupGetDatabaseCountByDatabaseTypeId(datatabletypeId, relatedDatabaseCount);
            SetupGenerateUrl(redirectUrl);

            // Act
            var result = service.DeleteDatabaseType(datatabletypeId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteDatabaseType_DeleteDatabaseTypeSucceeds_ReturnRedirectableClientActionResult()
        {
            // Arrange
            var datatabletypeId = 1;
            var relatedDatabaseCount = 0;
            var redirectUrl = "redirectUrl";
            var serviceResult = RedirectableClientActionResult.Success(redirectUrl, Messages.DatabaseTypeDeleted);
            var deleteResult = true;

            SetupGetDatabaseCountByDatabaseTypeId(datatabletypeId, relatedDatabaseCount);
            SetupGenerateUrl(redirectUrl);
            databaseTypeRepository.Setup(x => x.DeleteDatabaseType(datatabletypeId)).Returns(deleteResult);

            // Act
            var result = service.DeleteDatabaseType(datatabletypeId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        void SetupGetDatabaseCountByDatabaseTypeId(int datatabletypeId, int relatedDatabaseCount)
        {
            databaseTypeRepository.Setup(x => x.GetDatabaseCountByDatabaseTypeId(datatabletypeId)).Returns(relatedDatabaseCount);
        }

        void SetupGenerateUrl(string redirectUrl)
        {
            urlHelper.Setup(x => x.GenerateUrl(DatabaseTypeControllerActionNames.Index, ControllerNames.DatabaseType)).Returns(redirectUrl);
        }

        #endregion

        #region get database type by id

        [Test]
        public void GetDatabaseTypeById_DatabaseTypeExists_ReturnDatabaseType()
        {
            // Arrange
            var databaseType = new DatabaseType();
            var id = 1;

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeById(id)).Returns(databaseType);
            databaseTypeRepository.Setup(x => x.GetDatabaseTypeUpdateInfo(id)).Returns(databaseType.RecordUpdateInfo);

            // Act
            var result = service.GetDatabaseType(id);

            // Assert
            result.Should().BeSameAs(databaseType);
        }

        [Test]
        public void GetDatabaseTypeById_DatabaseTypeDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            DatabaseType databaseType = null;

            databaseTypeRepository.Setup(x => x.GetDatabaseTypeById(id)).Returns(databaseType);

            // Act

            var result = service.GetDatabaseType(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion
    }
}