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

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDependencyWriterServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationDependencyRepository> applicationDependencyRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationDependencyWriterService service;

        [SetUp]
        public void Initialize()
        {
            applicationDependencyRepository = new StrictMock<IApplicationDependencyRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationDependencyWriterService(
                applicationDependencyRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationDependencyRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region add application dependency

        [Test]
        public void AddApplicationDependency_Error_ReturnServiceResult()
        {
            // Arrange
            ApplicationDependency applicationDependency = new ApplicationDependency();
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            applicationDependencyRepository.Setup(x => x.AddApplicationDependency(applicationDependency)).Returns(false);

            // Act
            var result = service.AddApplicationDependency(applicationDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationDependency_Success_ReturnServiceResult()
        {
            // Arrange
            ApplicationDependency applicationDependency = new ApplicationDependency();
            var serviceResult = ServiceResult.Success(Messages.ApplicationDependencyCreated);

            applicationDependencyRepository.Setup(x => x.AddApplicationDependency(applicationDependency)).Returns(true);

            // Act
            var result = service.AddApplicationDependency(applicationDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region update application dependency

        [Test]
        public void UpdateApplicationDependency_ParameterIsNull_ReturnServiceResult()
        {
            // Arrange
            ApplicationDependency applicationDependency = null;

            // Act
            var result = service.UpdateApplicationDependency(applicationDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationDependency_ApplicationDependencyHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var oldApplicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(applicationDependency.Id)).Returns(oldApplicationDependency);
            auditService.Setup(x => x.IsChanged(oldApplicationDependency, applicationDependency, nameof(ApplicationDependency))).Returns(false);

            // Act
            var result = service.UpdateApplicationDependency(applicationDependency);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationDependencyUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationDependency_ApplicationDependencyUpdateFails_ReturnServerError()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var oldApplicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(applicationDependency.Id)).Returns(oldApplicationDependency);
            auditService.Setup(x => x.IsChanged(oldApplicationDependency, applicationDependency, nameof(ApplicationDependency))).Returns(true);
            applicationDependencyRepository.Setup(x => x.UpdateApplicationDependency(applicationDependency)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationDependency(applicationDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationDependency_ApplicationDependencyAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var oldApplicationDependency = new ApplicationDependency();
            var auditInfo = new AuditInfo();

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(applicationDependency.Id)).Returns(oldApplicationDependency);
            auditService.Setup(x => x.IsChanged(oldApplicationDependency, applicationDependency, nameof(ApplicationDependency))).Returns(true);
            applicationDependencyRepository.Setup(x => x.UpdateApplicationDependency(applicationDependency)).Returns(true);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationDependency(applicationDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationDependency_UpdateApplicationDependencyNotSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var oldApplicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(applicationDependency.Id)).Returns(oldApplicationDependency);
            auditService.Setup(x => x.IsChanged(oldApplicationDependency, applicationDependency, nameof(ApplicationDependency))).Returns(true);
            applicationDependencyRepository.Setup(x => x.UpdateApplicationDependency(applicationDependency)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationDependency), applicationDependency.Id, oldApplicationDependency, oldApplicationDependency, applicationDependency.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationDependency(applicationDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationDependency_UpdateApplicationDependencySucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var applicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var oldApplicationDependency = new ApplicationDependency { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            applicationDependencyRepository.Setup(x => x.GetApplicationDependencyById(applicationDependency.Id)).Returns(oldApplicationDependency);
            auditService.Setup(x => x.IsChanged(oldApplicationDependency, applicationDependency, nameof(ApplicationDependency))).Returns(true);
            applicationDependencyRepository.Setup(x => x.UpdateApplicationDependency(applicationDependency)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationDependency), applicationDependency.Id, oldApplicationDependency, oldApplicationDependency, applicationDependency.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationDependency(applicationDependency);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationDependencyUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  delete application dependency

        [Test]
        public void DeleteApplicationDependency_NoCondition_ReturnTrue()
        {
            // Arrange
            var applicationDependencyId = 5;
            var serviceResult = ServiceResult.Success(Messages.ApplicationDependencyDeleted);

            applicationDependencyRepository.Setup(x => x.DeleteApplicationDependency(applicationDependencyId)).Returns(true);

            // Act
            var result = service.DeleteApplicationDependency(applicationDependencyId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationDependency_NoCondition_ReturnFalse()
        {
            // Arrange
            var applicationDependencyId = 5;
            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            applicationDependencyRepository.Setup(x => x.DeleteApplicationDependency(applicationDependencyId)).Returns(false);

            // Act
            var result = service.DeleteApplicationDependency(applicationDependencyId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}