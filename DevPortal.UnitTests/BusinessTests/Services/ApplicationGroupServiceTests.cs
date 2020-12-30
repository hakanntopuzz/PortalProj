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
    public class ApplicationGroupServiceTest : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationGroupRepository> applicationGroupRepository;

        StrictMock<IApplicationRepository> applicationRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationGroupService service;

        [SetUp]
        public void Initialize()
        {
            applicationGroupRepository = new StrictMock<IApplicationGroupRepository>();
            applicationRepository = new StrictMock<IApplicationRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationGroupService(
                applicationGroupRepository.Object,
                applicationRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationGroupRepository.VerifyAll();
            applicationRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region GetApplicationGroups

        [Test]
        public void GetApplicationGroups_NoCondition_ReturnApplicationGroupDetails()
        {
            // Arrange
            ICollection<ApplicationGroup> applicationGroups = new List<ApplicationGroup>();

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);

            var result = service.GetApplicationGroups();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationGroups_NoCondition_ReturnApplicationGroupsIsNull()
        {
            // Arrange
            ICollection<ApplicationGroup> applicationGroups = null;

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);

            var result = service.GetApplicationGroups();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddApplicationGroup

        [Test]
        public void AddApplicationGroup_AddApplicationGroupFails_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationGroupModel = new ApplicationGroup();
            ApplicationGroup applicationGroup = null;
            var applicationGroupId = 0;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroupModel.Name)).Returns(applicationGroup);
            applicationGroupRepository.Setup(x => x.AddApplicationGroup(applicationGroupModel)).Returns(applicationGroupId);

            // Act
            var result = service.AddApplicationGroup(applicationGroupModel);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationGroup_AddApplicationGroupIsExits_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationGroupModel = new ApplicationGroup();
            var applicationGroup = new ApplicationGroup();
            var serviceResult = ServiceResult.Error(Messages.ApplicationGroupNameExists);

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroupModel.Name)).Returns(applicationGroup);

            // Act
            var result = service.AddApplicationGroup(applicationGroupModel);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationGroup_AddApplicationGroupSucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var applicationGroupModel = new ApplicationGroup();
            ApplicationGroup applicationGroup = null;
            var applicationGroupId = 1;
            var serviceResult = ServiceResult.Success(Messages.AddingSucceeds);

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroupModel.Name)).Returns(applicationGroup);
            applicationGroupRepository.Setup(x => x.AddApplicationGroup(applicationGroupModel)).Returns(applicationGroupId);

            // Act
            var result = service.AddApplicationGroup(applicationGroupModel);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region GetApplicationGroupByName

        [Test]
        public void GetApplicationGroupByName_NoCondition_ReturnTrue()
        {
            // Arrange
            var name = "name";
            var applicationGroup = new ApplicationGroup();

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(name)).Returns(applicationGroup);

            var result = service.GetApplicationGroupByName(name);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationGroupByName_NoCondition_ReturnApplicationGroupIsNull()
        {
            // Arrange
            var name = "name";
            ApplicationGroup applicationGroup = null;

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(name)).Returns(applicationGroup);

            var result = service.GetApplicationGroupByName(name);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region get application group by id

        [Test]
        public void GetApplicationGroupById_ApplicationGroupExists_ReturnApplicationGroup()
        {
            // Arrange
            var id = 1;
            var applicationGroup = new ApplicationGroup();
            var updateInfo = new RecordUpdateInfo();

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroupById(id)).Returns(applicationGroup);
            applicationGroupRepository.Setup(x => x.GetApplicationGroupUpdateInfo(id)).Returns(updateInfo);

            var result = service.GetApplicationGroupById(id);

            // Assert
            result.Should().Be(applicationGroup);
        }

        [Test]
        public void GetApplicationGroupById_ApplicationGroupDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            ApplicationGroup applicationGroup = null;

            applicationGroupRepository.Setup(x => x.GetApplicationGroupById(id)).Returns(applicationGroup);

            // Act

            var result = service.GetApplicationGroupById(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region update application group

        [Test]
        public void UpdateApplicationGroup_ApplicationGroupNull_ReturnServiceErrorResult()
        {
            // Arrange
            ApplicationGroup applicationGroup = null;

            // Act
            var result = service.UpdateApplicationGroup(applicationGroup);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationGroup_ApplicationGroupExists_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup { Id = 1, Name = "aa" };
            var group = new ApplicationGroup { Id = 2, Name = "aa" };
            var serviceResult = ServiceResult.Error(Messages.ApplicationGroupNameExists);

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroup.Name)).Returns(group);

            // Act
            var result = service.UpdateApplicationGroup(applicationGroup);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
            applicationGroupRepository.Verify(x => x.UpdateApplicationGroup(applicationGroup), Times.Never);
        }

        [Test]
        public void UpdateApplicationGroup_ApplicationGroupHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup { Id = 3, Name = "Test", StatusId = 1905 };
            var oldApplicationGroup = new ApplicationGroup { Id = 3, Name = "Test", StatusId = 1905 };

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroup.Name)).Returns(applicationGroup);
            applicationGroupRepository.Setup(x => x.GetApplicationGroupById(applicationGroup.Id)).Returns(oldApplicationGroup);
            auditService.Setup(x => x.IsChanged(oldApplicationGroup, applicationGroup, nameof(BaseApplicationGroup))).Returns(false);

            // Act
            var result = service.UpdateApplicationGroup(applicationGroup);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationGroupUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationGroup_ApplicationGroupDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup { Id = 1, Name = "Test", StatusId = 1 };
            var oldApplicationGroup = new ApplicationGroup { Id = 1, Name = "Test", StatusId = 2 };

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroup.Name)).Returns(applicationGroup);
            applicationGroupRepository.Setup(x => x.GetApplicationGroupById(applicationGroup.Id)).Returns(oldApplicationGroup);
            auditService.Setup(x => x.IsChanged(oldApplicationGroup, applicationGroup, nameof(BaseApplicationGroup))).Returns(true);
            applicationGroupRepository.Setup(x => x.UpdateApplicationGroup(applicationGroup)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationGroup(applicationGroup);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationGroup_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup { Id = 1, Name = "Test" };
            var oldApplicationGroup = new ApplicationGroup();
            var auditInfo = new AuditInfo();

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroup.Name)).Returns(applicationGroup);
            applicationGroupRepository.Setup(x => x.GetApplicationGroupById(applicationGroup.Id)).Returns(oldApplicationGroup);
            auditService.Setup(x => x.IsChanged(oldApplicationGroup, applicationGroup, nameof(BaseApplicationGroup))).Returns(true);
            applicationGroupRepository.Setup(x => x.UpdateApplicationGroup(applicationGroup)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationGroup), oldApplicationGroup.Id, oldApplicationGroup, oldApplicationGroup, applicationGroup.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationGroup(applicationGroup);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationGroup_UpdateApplicationSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup { Id = 1, Name = "test" };
            var oldApplicationGroup = new ApplicationGroup { Id = 1, Name = "test123" };
            ApplicationGroup group = null;
            var updateApplicationResult = true;
            var auditInfo = new AuditInfo();

            applicationGroupRepository.Setup(x => x.GetApplicationGroupByName(applicationGroup.Name)).Returns(group);
            applicationGroupRepository.Setup(x => x.GetApplicationGroupById(applicationGroup.Id)).Returns(oldApplicationGroup);
            auditService.Setup(x => x.IsChanged(oldApplicationGroup, applicationGroup, nameof(BaseApplicationGroup))).Returns(true);
            applicationGroupRepository.Setup(x => x.UpdateApplicationGroup(applicationGroup)).Returns(updateApplicationResult);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationGroup), oldApplicationGroup.Id, oldApplicationGroup, oldApplicationGroup, applicationGroup.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationGroup(applicationGroup);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationGroupUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region GetApplicationGroupStatus

        [Test]
        public void GetApplicationGroupStatus_NoCondition_ReturnApplicationGroupStatus()
        {
            // Arrange
            ICollection<ApplicationGroupStatus> applicationGroupStatuses = new List<ApplicationGroupStatus>();

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroupStatusList()).Returns(applicationGroupStatuses);

            var result = service.GetApplicationGroupStatusList();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationGroupStatus_NoCondition_ReturnApplicationGroupStatusIsNull()
        {
            // Arrange
            ICollection<ApplicationGroupStatus> applicationGroupStatuses = null;

            // Act
            applicationGroupRepository.Setup(x => x.GetApplicationGroupStatusList()).Returns(applicationGroupStatuses);

            var result = service.GetApplicationGroupStatusList();

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region DeleteApplicationGroup

        [Test]
        public void DeleteApplicationGroup_DeleteApplicationGroupFails_ReturnServiceErrorResult()
        {
            // Arrange
            var groupId = 1;
            var applications = new List<ApplicationListItem>();
            var serviceResult = ServiceResult.Error(Messages.DeleteFails);
            var deleteResult = false;

            applicationRepository.Setup(x => x.GetApplicationsByGroupId(groupId)).Returns(applications);
            applicationGroupRepository.Setup(x => x.DeleteApplicationGroup(groupId)).Returns(deleteResult);

            // Act
            var result = service.DeleteApplicationGroup(groupId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationGroup_RelatedApplicationsExists_ReturnServiceErrorResult()
        {
            // Arrange
            var groupId = 1;
            var applications = new List<ApplicationListItem>
            {
                new ApplicationListItem {
                    Id =1,
                    Name = "name",
                    ApplicationGroupName="group-name",
                    ModifiedDate = DateTime.Now,
                    Status = "active"
                }
            };
            var serviceResult = ServiceResult.Error(Messages.RelatedApplicationsExists);

            applicationRepository.Setup(x => x.GetApplicationsByGroupId(groupId)).Returns(applications);

            // Act
            var result = service.DeleteApplicationGroup(groupId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationGroup_DeleteApplicationGroupSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var groupId = 1;
            var applications = new List<ApplicationListItem>();
            var serviceResult = ServiceResult.Success(Messages.ApplicationGroupDeleted);
            var deleteResult = true;

            applicationRepository.Setup(x => x.GetApplicationsByGroupId(groupId)).Returns(applications);
            applicationGroupRepository.Setup(x => x.DeleteApplicationGroup(groupId)).Returns(deleteResult);

            // Act
            var result = service.DeleteApplicationGroup(groupId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}