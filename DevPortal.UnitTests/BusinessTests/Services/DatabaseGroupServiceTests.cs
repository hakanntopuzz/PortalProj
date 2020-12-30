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
    public class DatabaseGroupServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDatabaseGroupRepository> databaseGroupRepository;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        DatabaseGroupService service;

        [SetUp]
        public void Initialize()
        {
            databaseGroupRepository = new StrictMock<IDatabaseGroupRepository>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new DatabaseGroupService(
                databaseGroupRepository.Object,
                urlHelper.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseGroupRepository.VerifyAll();
            urlHelper.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get database groups

        [Test]
        public void GetDatabaseGroups_NoCondition_ReturntDatabaseGroupList()
        {
            // Arrange
            ICollection<DatabaseGroup> databaseGroups = null;

            databaseGroupRepository.Setup(x => x.GetDatabaseGroups()).Returns(databaseGroups);

            // Act
            var result = service.GetDatabaseGroups();

            // Assert
            result.Should().BeSameAs(databaseGroups);
        }

        #endregion

        #region add database group

        [Test]
        public void AddDatabaseGroup_DatabaseGroupExists_ReturnDatabaseGroup()
        {
            // Arrange
            DatabaseGroup databaseGroup = new DatabaseGroup();
            DatabaseGroup databaseGroupModel = new DatabaseGroup();
            var serviceResult = ServiceResult.Error(Messages.DatabaseGroupFound);

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroupModel);

            // Act
            var result = service.AddDatabaseGroup(databaseGroup);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabaseGroup_DatabaseGroupIdZero_ReturnDatabaseGroup()
        {
            // Arrange
            DatabaseGroup databaseGroup = new DatabaseGroup();
            DatabaseGroup databaseGroupModel = null;
            var serviceResult = ServiceResult.Error(Messages.DatabaseGroupAdded);
            var databaseGroupId = 0;

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroupModel);
            databaseGroupRepository.Setup(x => x.AddDatabaseGroup(databaseGroup)).Returns(databaseGroupId);

            // Act
            var result = service.AddDatabaseGroup(databaseGroup);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddDatabaseGroup_Success_ReturnDatabaseGroup()
        {
            // Arrange
            DatabaseGroup databaseGroup = new DatabaseGroup();
            DatabaseGroup databaseGroupModel = null;
            var serviceResult = ServiceResult.Success(Messages.AddingSucceeds);
            var databaseGroupId = 1;

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroupModel);
            databaseGroupRepository.Setup(x => x.AddDatabaseGroup(databaseGroup)).Returns(databaseGroupId);

            // Act
            var result = service.AddDatabaseGroup(databaseGroup);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region get database group by id

        [Test]
        public void GetDatabaseGroupById_DatabaseGroupExists_ReturnDatabaseGroup()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup();
            var id = 1;

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupById(id)).Returns(databaseGroup);
            databaseGroupRepository.Setup(x => x.GetDatabaseGroupUpdateInfo(id)).Returns(databaseGroup.RecordUpdateInfo);

            // Act
            var result = service.GetDatabaseGroup(id);

            // Assert
            result.Should().BeSameAs(databaseGroup);
        }

        [Test]
        public void GetDatabaseGroupById_DatabaseGroupDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            DatabaseGroup databaseGroup = null;

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupById(id)).Returns(databaseGroup);

            // Act

            var result = service.GetDatabaseGroup(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region update database group

        [Test]
        public void UpdateDatabaseGroup_DatabaseGroupIsNull_ReturnServiceResultError()
        {
            // Arrange
            DatabaseGroup databaseGroup = null;
            var serviceResult = ServiceResult.Error(Messages.NullParameterError);

            // Act
            var result = service.UpdateDatabaseGroup(databaseGroup);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseGroup_DatabaseGroupExists_ReturnServiceResultError()
        {
            // Arrange
            DatabaseGroup databaseGroup = new DatabaseGroup { Id = 5 };
            DatabaseGroup databaseGroupModel = new DatabaseGroup { Id = 4 };
            var serviceResult = ServiceResult.Error(Messages.DatabaseGroupFound);

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroupModel);

            // Act
            var result = service.UpdateDatabaseGroup(databaseGroup);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateDatabaseGroup_DatabaseGroupHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup { Id = 5, Name = "group" };
            var oldDatabaseGroup = new DatabaseGroup { Id = 5 };

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroup);
            databaseGroupRepository.Setup(x => x.GetDatabaseGroupById(databaseGroup.Id)).Returns(oldDatabaseGroup);
            auditService.Setup(x => x.IsChanged(oldDatabaseGroup, databaseGroup, nameof(DatabaseGroup))).Returns(false);

            // Act
            var result = service.UpdateDatabaseGroup(databaseGroup);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseGroupUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseGroup_DatabaseGroupDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup { Id = 2, Name = "group" };
            var oldDatabaseGroup = new DatabaseGroup { Id = 5 };

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroup);
            databaseGroupRepository.Setup(x => x.GetDatabaseGroupById(databaseGroup.Id)).Returns(oldDatabaseGroup);
            auditService.Setup(x => x.IsChanged(oldDatabaseGroup, databaseGroup, nameof(DatabaseGroup))).Returns(true);
            databaseGroupRepository.Setup(x => x.UpdateDatabaseGroup(databaseGroup)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseGroup(databaseGroup);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseGroup_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup { Id = 2, Name = "group" };
            var oldDatabaseGroup = new DatabaseGroup();
            var auditInfo = new AuditInfo();

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroup);
            databaseGroupRepository.Setup(x => x.GetDatabaseGroupById(databaseGroup.Id)).Returns(oldDatabaseGroup);
            auditService.Setup(x => x.IsChanged(oldDatabaseGroup, databaseGroup, nameof(DatabaseGroup))).Returns(true);
            databaseGroupRepository.Setup(x => x.UpdateDatabaseGroup(databaseGroup)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(DatabaseGroup), oldDatabaseGroup.Id, oldDatabaseGroup, oldDatabaseGroup, databaseGroup.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateDatabaseGroup(databaseGroup);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateDatabaseGroup_UpdateDatabaseGroupSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseGroup = new DatabaseGroup { Id = 2, Name = "group" };
            var oldDatabaseGroup = new DatabaseGroup { Id = 5, Name = "group" };
            var auditInfo = new AuditInfo();

            databaseGroupRepository.Setup(x => x.GetDatabaseGroupByName(databaseGroup.Name)).Returns(databaseGroup);
            databaseGroupRepository.Setup(x => x.GetDatabaseGroupById(databaseGroup.Id)).Returns(oldDatabaseGroup);
            auditService.Setup(x => x.IsChanged(oldDatabaseGroup, databaseGroup, nameof(DatabaseGroup))).Returns(true);
            databaseGroupRepository.Setup(x => x.UpdateDatabaseGroup(databaseGroup)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(DatabaseGroup), oldDatabaseGroup.Id, oldDatabaseGroup, oldDatabaseGroup, databaseGroup.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateDatabaseGroup(databaseGroup);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.DatabaseGroupUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region delete database group

        [Test]
        public void DeleteDatabaseGroup_DeleteDatabaseGroupFails_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseGroupId = 1;
            var relatedDatabaseCount = 0;
            var redirectUrl = "redirectUrl";
            var serviceResult = RedirectableClientActionResult.Error(redirectUrl, Messages.DeleteFails);
            var deleteResult = false;

            SetupGetDatabaseCountByDatabaseGroupId(databaseGroupId, relatedDatabaseCount);
            SetupGenerateUrl(redirectUrl);
            databaseGroupRepository.Setup(x => x.DeleteDatabaseGroup(databaseGroupId)).Returns(deleteResult);

            // Act
            var result = service.DeleteDatabaseGroup(databaseGroupId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteDatabaseGroup_RelatedDatabaseGroupDatabaseExists_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseGroupId = 1;
            var relatedDatabaseCount = 1;
            var redirectUrl = "redirectUrl";
            var serviceResult = RedirectableClientActionResult.Error(redirectUrl, Messages.RelatedDatabaseGroupDatabaseExists);

            SetupGetDatabaseCountByDatabaseGroupId(databaseGroupId, relatedDatabaseCount);
            SetupGenerateUrl(redirectUrl);

            // Act
            var result = service.DeleteDatabaseGroup(databaseGroupId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteDatabaseGroup_DeleteDatabaseGroupSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var databaseGroupId = 1;
            var relatedDatabaseCount = 0;
            var redirectUrl = "redirectUrl";
            var serviceResult = RedirectableClientActionResult.Success(redirectUrl, Messages.DatabaseGroupDeleted);
            var deleteResult = true;

            SetupGetDatabaseCountByDatabaseGroupId(databaseGroupId, relatedDatabaseCount);
            SetupGenerateUrl(redirectUrl);
            databaseGroupRepository.Setup(x => x.DeleteDatabaseGroup(databaseGroupId)).Returns(deleteResult);

            // Act
            var result = service.DeleteDatabaseGroup(databaseGroupId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        void SetupGetDatabaseCountByDatabaseGroupId(int databaseGroupId, int relatedDatabaseCount)
        {
            databaseGroupRepository.Setup(x => x.GetDatabaseCountByDatabaseGroupId(databaseGroupId)).Returns(relatedDatabaseCount);
        }

        void SetupGenerateUrl(string redirectUrl)
        {
            urlHelper.Setup(x => x.GenerateUrl(DatabaseGroupControllerActionNames.Index, ControllerNames.DatabaseGroup)).Returns(redirectUrl);
        }

        #endregion
    }
}