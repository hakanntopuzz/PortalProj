using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class EnvironmentServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IEnvironmentRepository> environmentRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        EnvironmentService service;

        [SetUp]
        public void Initialize()
        {
            environmentRepository = new StrictMock<IEnvironmentRepository>();
            auditService = new StrictMock<IAuditService>();
            loggingService = new StrictMock<ILoggingService>();
            auditFactory = new StrictMock<IAuditFactory>();

            service = new EnvironmentService(
                environmentRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            environmentRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get environments

        [Test]
        public void GetEnvironments_NoCondition_ReturnEnvironmentList()
        {
            // Arrange
            ICollection<Environment> environmentList = null;

            environmentRepository.Setup(x => x.GetEnvironments()).Returns(environmentList);

            // Act
            var result = service.GetEnvironments();

            // Assert
            result.Should().BeSameAs(environmentList);
        }

        #endregion

        #region get environment by id

        [Test]
        public void GetEnvironmentById_EnvironmentExists_ReturnEnvironment()
        {
            // Arrange
            var environment = new Environment();
            var id = 1;

            environmentRepository.Setup(x => x.GetEnvironmentById(id)).Returns(environment);
            environmentRepository.Setup(x => x.GetEnvironmentUpdateInfo(id)).Returns(environment.RecordUpdateInfo);

            // Act
            var result = service.GetEnvironment(id);

            // Assert
            result.Should().BeSameAs(environment);
        }

        [Test]
        public void GetEnvironmentById_EnvironmentDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            Environment environment = null;

            environmentRepository.Setup(x => x.GetEnvironmentById(id)).Returns(environment);

            // Act

            var result = service.GetEnvironment(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region add environment

        [Test]
        public void AddEnvironment_EnvironmentExists_ReturnEnvironment()
        {
            // Arrange
            Environment environment = new Environment();
            Environment environmentModel = new Environment();
            var serviceResult = ServiceResult.Error(Messages.EnvironmentFound);

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environmentModel);

            // Act
            var result = service.AddEnvironment(environment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddEnvironment_EnvironmentIdZero_ReturnEnvironment()
        {
            // Arrange
            Environment environment = new Environment();
            Environment environmentModel = null;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);
            var environmentId = 0;

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environmentModel);
            environmentRepository.Setup(x => x.AddEnvironment(environment)).Returns(environmentId);

            // Act
            var result = service.AddEnvironment(environment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddEnvironment_Success_ReturnEnvironment()
        {
            // Arrange
            Environment environment = new Environment();
            Environment environmentModel = null;
            var serviceResult = ServiceResult.Success(Messages.AddingSucceeds);
            var environmentId = 1;

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environmentModel);
            environmentRepository.Setup(x => x.AddEnvironment(environment)).Returns(environmentId);

            // Act
            var result = service.AddEnvironment(environment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region update environment

        [Test]
        public void UpdateEnvironment_EnvironmentIsNull_ReturnServiceResultError()
        {
            // Arrange
            Environment environment = null;
            var serviceResult = ServiceResult.Error(Messages.NullParameterError);

            // Act
            var result = service.UpdateEnvironment(environment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateEnvironment_EnvironmentExists_ReturnServiceResultError()
        {
            // Arrange
            Environment environment = new Environment { Id = 5 };
            Environment environmentModel = new Environment { Id = 4 };
            var serviceResult = ServiceResult.Error(Messages.EnvironmentFound);

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environmentModel);

            // Act
            var result = service.UpdateEnvironment(environment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateEnvironment_EnvironmentHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            Environment environment = new Environment { Id = 3, Name = "Test" };
            var oldEnvironment = new Environment { Id = 3, Name = "Test" };

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environment);
            environmentRepository.Setup(x => x.GetEnvironmentById(environment.Id)).Returns(oldEnvironment);
            auditService.Setup(x => x.IsChanged(oldEnvironment, environment, nameof(Environment))).Returns(false);

            // Act
            var result = service.UpdateEnvironment(environment);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.EnvironmentUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateEnvironment_EnvironmentDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var environment = new Environment { Id = 3, Name = "Test" };
            var oldEnvironment = new Environment { Id = 3, Name = "Test" };

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environment);
            environmentRepository.Setup(x => x.GetEnvironmentById(environment.Id)).Returns(oldEnvironment);
            auditService.Setup(x => x.IsChanged(oldEnvironment, environment, nameof(Environment))).Returns(true);
            environmentRepository.Setup(x => x.UpdateEnvironment(environment)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<System.Exception>()));

            // Act
            var result = service.UpdateEnvironment(environment);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateEnvironment_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var environment = new Environment { Id = 2, Name = "Test" };
            var oldEnvironment = new Environment();
            var auditInfo = new AuditInfo();

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environment);
            environmentRepository.Setup(x => x.GetEnvironmentById(environment.Id)).Returns(oldEnvironment);
            auditService.Setup(x => x.IsChanged(oldEnvironment, environment, nameof(Environment))).Returns(true);
            environmentRepository.Setup(x => x.UpdateEnvironment(environment)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Environment), oldEnvironment.Id, oldEnvironment, oldEnvironment, environment.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<System.Exception>()));

            // Act
            var result = service.UpdateEnvironment(environment);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateEnvironment_UpdateEnvironmentSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var environment = new Environment { Id = 3, Name = "Test" };
            var oldEnvironment = new Environment { Id = 3, Name = "Test" };
            var serviceResult = ServiceResult.Success(Messages.EnvironmentUpdated);
            var auditInfo = new AuditInfo();

            environmentRepository.Setup(x => x.GetEnvironmentByName(environment.Name)).Returns(environment);
            environmentRepository.Setup(x => x.GetEnvironmentById(environment.Id)).Returns(oldEnvironment);
            auditService.Setup(x => x.IsChanged(oldEnvironment, environment, nameof(Environment))).Returns(true);
            environmentRepository.Setup(x => x.UpdateEnvironment(environment)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Environment), oldEnvironment.Id, oldEnvironment, oldEnvironment, environment.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateEnvironment(environment);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region DeleteEnvironment

        [Test]
        public void DeleteEnvironment_DeleteEnvironmentFails_ReturnServiceErrorResult()
        {
            // Arrange
            var environmentId = 1;
            var relatedAppEnvironmentCount = 0;
            var serviceResult = ServiceResult.Error(Messages.DeleteFails);
            var deleteResult = false;

            SetupGetApplicationEnvironmentCountByEnvironmentId(environmentId, relatedAppEnvironmentCount);
            environmentRepository.Setup(x => x.DeleteEnvironment(environmentId)).Returns(deleteResult);

            // Act
            var result = service.DeleteEnvironment(environmentId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteEnvironment_RelatedApplicationEnvironmentsExists_ReturnServiceErrorResult()
        {
            // Arrange
            var environmentId = 1;
            var relatedAppEnvironmentCount = 3;
            var serviceResult = ServiceResult.Error(Messages.RelatedApplicationEnvironmentsExists);

            SetupGetApplicationEnvironmentCountByEnvironmentId(environmentId, relatedAppEnvironmentCount);

            // Act
            var result = service.DeleteEnvironment(environmentId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        void SetupGetApplicationEnvironmentCountByEnvironmentId(int environmentId, int relatedAppEnvironmentCount)
        {
            environmentRepository.Setup(x => x.GetApplicationEnvironmentCountByEnvironmentId(environmentId)).Returns(relatedAppEnvironmentCount);
        }

        [Test]
        public void DeleteEnvironment_DeleteEnvironmentSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var environmentId = 1;
            var relatedAppEnvironmentCount = 0;
            var serviceResult = ServiceResult.Success(Messages.EnvironmentDeleted);
            var deleteResult = true;

            SetupGetApplicationEnvironmentCountByEnvironmentId(environmentId, relatedAppEnvironmentCount);
            environmentRepository.Setup(x => x.DeleteEnvironment(environmentId)).Returns(deleteResult);

            // Act
            var result = service.DeleteEnvironment(environmentId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}