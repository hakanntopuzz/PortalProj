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
using System.Linq;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationEnvironmentServiceTest : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationEnvironmentRepository> repository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationEnvironmentService service;

        [SetUp]
        public void Initialize()
        {
            repository = new StrictMock<IApplicationEnvironmentRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationEnvironmentService(
                repository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            repository.VerifyAll();
        }

        #endregion

        #region AddApplicationEnvironment

        [Test]
        public void AddApplicationEnvironment_AddApplicationEnvironmentFails_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment();
            var addApplicationResult = false;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            repository.Setup(x => x.AddApplicationEnvironment(applicationEnvironment)).Returns(addApplicationResult);

            // Act
            var result = service.AddApplicationEnvironment(applicationEnvironment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationEnvironment_AddApplicationEnvironmentSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment();
            var updateApplicationResult = true;
            var serviceResult = ServiceResult.Success(Messages.ApplicationEnvironmentCreated);

            repository.Setup(x => x.AddApplicationEnvironment(applicationEnvironment)).Returns(updateApplicationResult);

            // Act
            var result = service.AddApplicationEnvironment(applicationEnvironment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region DeleteApplicationEnvironment

        [Test]
        public void DeleteApplicationEnvironment_NoCondition_ReturnTrue()
        {
            // Arrange
            var id = 5;

            var serviceResult = ServiceResult.Success(Messages.ApplicationEnvironmentDeleted);

            repository.Setup(x => x.DeleteApplicationEnvironment(id)).Returns(true);

            // Act
            var result = service.DeleteApplicationEnvironment(id);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationEnvironment_NoCondition_ReturnFalse()
        {
            // Arrange
            var id = 5;

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            repository.Setup(x => x.DeleteApplicationEnvironment(id)).Returns(false);

            // Act
            var result = service.DeleteApplicationEnvironment(id);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region GetApplicationEnvironmentDetailById

        [Test]
        public void GetApplicationEnvironmentDetailById_EnvironmentExists_ReturnApplicationEnvironment()
        {
            // Arrange
            var id = 5;
            var applicationEnvironment = new ApplicationEnvironment();
            var recordUpdateInfo = new RecordUpdateInfo();

            repository.Setup(x => x.GetApplicationEnvironmentById(id)).Returns(applicationEnvironment);
            repository.Setup(x => x.GetApplicationEnvironmentUpdateInfo(id)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetApplicationEnvironment(id);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationEnvironmentDetailById_EnvironmentDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 5;
            ApplicationEnvironment applicationEnvironment = null;

            repository.Setup(x => x.GetApplicationEnvironmentById(id)).Returns(applicationEnvironment);

            // Act
            var result = service.GetApplicationEnvironment(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationEnvironments

        [Test]
        public void GetApplicationEnvironments_NoCondition_ReturnApplicationEnvironment()
        {
            // Arrange
            int applicationId = 1;
            ICollection<ApplicationEnvironment> applicationEnvironment = new List<ApplicationEnvironment>();

            repository.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(applicationEnvironment);

            // Act
            var result = service.GetApplicationEnvironments(applicationId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetApplicationEnvironments_NoCondition_ReturnApplicationEnvironmentIsNull()
        {
            // Arrange
            int applicationId = 1;
            ICollection<ApplicationEnvironment> applicationEnvironment = null;

            repository.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(applicationEnvironment);

            // Act
            var result = service.GetApplicationEnvironments(applicationId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetApplicationEnvironmentsInHasLog

        [Test]
        public void GetApplicationEnvironmentsInHasLog_NoCondition_ReturnApplicationEnvironment()
        {
            // Arrange
            int applicationId = 1;
            ICollection<ApplicationEnvironment> applicationEnvironment = new List<ApplicationEnvironment> { new ApplicationEnvironment { Id = 1, HasLog = true } };
            var expected = applicationEnvironment.Where(q => q.HasLog).ToList();

            repository.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(applicationEnvironment);

            // Act
            var result = service.GetApplicationEnvironmentsHasLog(applicationId);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(expected.Count);
        }

        [Test]
        public void GetApplicationEnvironmentsInHasLog_NoCondition_ReturnApplicationEnvironmentIsEmpty()
        {
            // Arrange
            int applicationId = 1;
            List<ApplicationEnvironment> applicationEnvironment = new List<ApplicationEnvironment> { new ApplicationEnvironment { } };

            repository.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(applicationEnvironment);

            // Act
            var result = service.GetApplicationEnvironmentsHasLog(applicationId);

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region GetEnvironmentsDoesNotExistByApplicationId

        [Test]
        public void GetEnvironmentsDoesNotExistByApplicationId_NoCondition_ReturnEnvironment()
        {
            // Arrange
            int applicationId = 1;
            ICollection<Model.Environment> environment = new List<Model.Environment>();

            repository.Setup(x => x.GetEnvironmentsDoesNotExistByApplicationId(applicationId)).Returns(environment);

            // Act
            var result = service.GetEnvironmentsDoesNotExist(applicationId);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetEnvironmentsDoesNotExistByApplicationId_NoCondition_ReturnEnvironmentIsNull()
        {
            // Arrange
            int applicationId = 1;
            ICollection<Model.Environment> environment = null;

            repository.Setup(x => x.GetEnvironmentsDoesNotExistByApplicationId(applicationId)).Returns(environment);

            // Act
            var result = service.GetEnvironmentsDoesNotExist(applicationId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region UpdateApplicationEnvironment

        [Test]
        public void UpdateApplicationEnvironment_ApplicationEnvironmentIsNull_ReturnServiceErrorResult()
        {
            // Arrange
            ApplicationEnvironment applicationEnvironment = null;

            // Act
            var result = service.UpdateApplicationEnvironment(applicationEnvironment);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationEnvironment_UpdateApplicationEnvironmentExists_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment { Id = 1, ApplicationId = 1, EnvironmentId = 2 };
            var environment = new ApplicationEnvironment { Id = 2, ApplicationId = 3, EnvironmentId = 4 };
            var serviceResult = ServiceResult.Error(Messages.ApplicationEnvironmentFound);

            repository.Setup(x => x.GetApplicationEnvironmentByEnvironmentId(applicationEnvironment.ApplicationId, applicationEnvironment.EnvironmentId)).Returns(environment);

            // Act
            var result = service.UpdateApplicationEnvironment(applicationEnvironment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateApplicationEnvironment_ApplicationEnvironmentHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 2 };
            var oldApplicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 2 };

            repository.Setup(x => x.GetApplicationEnvironmentByEnvironmentId(applicationEnvironment.ApplicationId, applicationEnvironment.EnvironmentId)).Returns(applicationEnvironment);
            repository.Setup(x => x.GetApplicationEnvironmentById(applicationEnvironment.Id)).Returns(oldApplicationEnvironment);
            auditService.Setup(x => x.IsChanged(oldApplicationEnvironment, applicationEnvironment, nameof(ApplicationEnvironment))).Returns(false);

            // Act
            var result = service.UpdateApplicationEnvironment(applicationEnvironment);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationEnvironmentUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationEnvironment_ApplicationEnvironmentDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 2 };
            var oldApplicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 2 };

            repository.Setup(x => x.GetApplicationEnvironmentByEnvironmentId(applicationEnvironment.ApplicationId, applicationEnvironment.EnvironmentId)).Returns(applicationEnvironment);
            repository.Setup(x => x.GetApplicationEnvironmentById(applicationEnvironment.Id)).Returns(oldApplicationEnvironment);
            auditService.Setup(x => x.IsChanged(oldApplicationEnvironment, applicationEnvironment, nameof(ApplicationEnvironment))).Returns(true);
            repository.Setup(x => x.UpdateApplicationEnvironment(applicationEnvironment)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationEnvironment(applicationEnvironment);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationEnvironment_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 2 };
            var oldApplicationEnvironment = new ApplicationEnvironment();
            var auditInfo = new AuditInfo();

            repository.Setup(x => x.GetApplicationEnvironmentByEnvironmentId(applicationEnvironment.ApplicationId, applicationEnvironment.EnvironmentId)).Returns(applicationEnvironment);
            repository.Setup(x => x.GetApplicationEnvironmentById(applicationEnvironment.Id)).Returns(oldApplicationEnvironment);
            auditService.Setup(x => x.IsChanged(oldApplicationEnvironment, applicationEnvironment, nameof(ApplicationEnvironment))).Returns(true);
            repository.Setup(x => x.UpdateApplicationEnvironment(applicationEnvironment)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationEnvironment), oldApplicationEnvironment.Id, oldApplicationEnvironment, oldApplicationEnvironment, applicationEnvironment.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationEnvironment(applicationEnvironment);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationEnvironment_UpdateApplicationEnvironmentSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 2 };
            var oldApplicationEnvironment = new ApplicationEnvironment { ApplicationId = 1, EnvironmentId = 4 };
            var auditInfo = new AuditInfo();

            repository.Setup(x => x.GetApplicationEnvironmentByEnvironmentId(applicationEnvironment.ApplicationId, applicationEnvironment.EnvironmentId)).Returns(applicationEnvironment);
            repository.Setup(x => x.GetApplicationEnvironmentById(applicationEnvironment.Id)).Returns(oldApplicationEnvironment);
            auditService.Setup(x => x.IsChanged(oldApplicationEnvironment, applicationEnvironment, nameof(ApplicationEnvironment))).Returns(true);
            repository.Setup(x => x.UpdateApplicationEnvironment(applicationEnvironment)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationEnvironment), oldApplicationEnvironment.Id, oldApplicationEnvironment, oldApplicationEnvironment, applicationEnvironment.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationEnvironment(applicationEnvironment);

            // Assert
            var serviceResult = ServiceResult.Success(Messages.ApplicationEnvironmentUpdated);
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}