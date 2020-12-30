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
    public class ApplicationBuildSettingsServiceTest : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationBuildSettingsRepository> repository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationBuildSettingsService service;

        [SetUp]
        public void Initialize()
        {
            repository = new StrictMock<IApplicationBuildSettingsRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationBuildSettingsService(
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
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region Get Application Build Settings

        [Test]
        public void GetApplicationBuildSettings_NoConditions_ReturnBuildSettings()
        {
            // Arrange
            var id = 5;
            var buildSettings = new ApplicationBuildSettings();
            var recordUpdateInfo = new RecordUpdateInfo();
            repository.Setup(x => x.GetApplicationBuildSettings(id)).Returns(buildSettings);
            repository.Setup(x => x.GetApplicationBuildSettingsUpdateInfo(id)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetApplicationBuildSettings(id);

            // Assert
            result.Should().Be(buildSettings);
        }

        #endregion

        #region Add/Update Application Build Settings

        [Test]
        public void AddOrUpdateApplicationBuildSettings_ApplicationBuildSettingsIsNullAndAddBuildSettingsFails_ReturnServiceErrorResult()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { ApplicationId = 1 };
            ApplicationBuildSettings settings = null;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(settings);
            repository.Setup(x => x.AddApplicationBuildSettings(buildSettings)).Returns(false);

            // Act
            var result = service.AddOrUpdateApplicationBuildSettings(buildSettings);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddOrUpdateApplicationBuildSettings_ApplicationBuildSettingsIsNullAndAddBuildSettingsSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { ApplicationId = 1 };
            ApplicationBuildSettings settings = null;
            var serviceResult = ServiceResult.Success(Messages.ApplicationBuildSettingsUpdated);

            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(settings);
            repository.Setup(x => x.AddApplicationBuildSettings(buildSettings)).Returns(true);

            // Act
            var result = service.AddOrUpdateApplicationBuildSettings(buildSettings);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddOrUpdateApplicationBuildSettings_ApplicationBuildSettingsHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var applicationId = 1;
            var buildSettings = new ApplicationBuildSettings { ApplicationId = 1, ProjectName = "name" };
            var oldBuildSettings = new ApplicationBuildSettings { ApplicationId = 1, ProjectName = "name" };
            var recordUpdateInfo = new RecordUpdateInfo();
            var serviceResult = ServiceResult.Success(Messages.UpdateSucceeds);

            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(buildSettings);
            repository.Setup(x => x.GetApplicationBuildSettingsUpdateInfo(applicationId)).Returns(recordUpdateInfo);
            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(oldBuildSettings);
            auditService.Setup(x => x.IsChanged(oldBuildSettings, buildSettings, nameof(ApplicationBuildSettings))).Returns(false);

            // Act
            var result = service.AddOrUpdateApplicationBuildSettings(buildSettings);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddOrUpdateApplicationBuildSettings_UpdateFails_ReturnServerError()
        {
            // Arrange
            var applicationId = 1;
            var buildSettings = new ApplicationBuildSettings { ApplicationId = 1, ProjectName = "name" };
            var oldBuildSettings = new ApplicationBuildSettings { ApplicationId = 1, ProjectName = "name" };
            var recordUpdateInfo = new RecordUpdateInfo();
            var serviceResult = ServiceResult.Error(Messages.UpdateFails);

            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(buildSettings);
            repository.Setup(x => x.GetApplicationBuildSettingsUpdateInfo(applicationId)).Returns(recordUpdateInfo);
            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(oldBuildSettings);
            auditService.Setup(x => x.IsChanged(oldBuildSettings, buildSettings, nameof(ApplicationBuildSettings))).Returns(true);
            repository.Setup(x => x.UpdateApplicationBuildSettings(buildSettings)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.AddOrUpdateApplicationBuildSettings(buildSettings);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddOrUpdateApplicationBuildSettings_UpdateSucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var applicationId = 1;
            var buildSettings = new ApplicationBuildSettings { Id = 1, ApplicationId = 1, ProjectName = "name" };
            var oldBuildSettings = new ApplicationBuildSettings { Id = 1, ApplicationId = 1, ProjectName = "name" };
            var auditInfo = new AuditInfo();
            var recordUpdateInfo = new RecordUpdateInfo();
            var serviceResult = ServiceResult.Success(Messages.ApplicationBuildSettingsUpdated);

            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(oldBuildSettings);
            repository.Setup(x => x.GetApplicationBuildSettingsUpdateInfo(applicationId)).Returns(recordUpdateInfo);
            auditService.Setup(x => x.IsChanged(oldBuildSettings, buildSettings, nameof(ApplicationBuildSettings))).Returns(true);
            repository.Setup(x => x.UpdateApplicationBuildSettings(buildSettings)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationBuildSettings), buildSettings.Id, oldBuildSettings, oldBuildSettings, buildSettings.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.AddOrUpdateApplicationBuildSettings(buildSettings);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddOrUpdateApplicationBuildSettings_AddAuditFails_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationId = 1;
            var buildSettings = new ApplicationBuildSettings { Id = 1, ApplicationId = 1, ProjectName = "name" };
            var oldBuildSettings = new ApplicationBuildSettings { Id = 1, ApplicationId = 1, ProjectName = "name" };
            var auditInfo = new AuditInfo();
            var recordUpdateInfo = new RecordUpdateInfo();
            var serviceResult = ServiceResult.Error(Messages.UpdateFails);

            repository.Setup(x => x.GetApplicationBuildSettings(buildSettings.ApplicationId)).Returns(oldBuildSettings);
            repository.Setup(x => x.GetApplicationBuildSettingsUpdateInfo(applicationId)).Returns(recordUpdateInfo);
            auditService.Setup(x => x.IsChanged(oldBuildSettings, buildSettings, nameof(ApplicationBuildSettings))).Returns(true);
            repository.Setup(x => x.UpdateApplicationBuildSettings(buildSettings)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ApplicationBuildSettings), buildSettings.Id, oldBuildSettings, oldBuildSettings, buildSettings.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.AddOrUpdateApplicationBuildSettings(buildSettings);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}