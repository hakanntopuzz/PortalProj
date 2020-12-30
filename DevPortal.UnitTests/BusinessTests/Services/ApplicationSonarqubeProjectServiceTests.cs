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
    public class ApplicationSonarqubeProjectServiceTest : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationSonarqubeProjectRepository> repository;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationSonarqubeProjectService service;

        [SetUp]
        public void Initialize()
        {
            repository = new StrictMock<IApplicationSonarqubeProjectRepository>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationSonarqubeProjectService(
                repository.Object,
                generalSettingsService.Object,
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
            generalSettingsService.VerifyAll();
        }

        #endregion

        #region Get Sonarqube Projects

        [Test]
        public void GetSonarqubeProjects_NoCondition_ReturnProjectList()
        {
            // Arrange
            var applicationId = 1;
            var sonarqubeProjectList = new List<SonarqubeProject>();
            var sonarqubeProjectUrl = "http://wwww.example.com/sonarqube-project-url";
            var sonarqubeProjectUri = new Uri(sonarqubeProjectUrl);

            // Act
            generalSettingsService.Setup(x => x.GetSonarqubeProjectUrl()).Returns(sonarqubeProjectUri);
            repository.Setup(x => x.GetSonarqubeProjects(applicationId)).Returns(sonarqubeProjectList);

            var result = service.GetSonarqubeProjects(applicationId);

            // Assert
            result.Should().BeSameAs(sonarqubeProjectList);
        }

        #endregion

        #region GetSonarQubeProjectById

        [Test]
        public void GetSonarQubeProjectById_ProjectDoesNotExist_ReturnNull()
        {
            // Arrange
            var projectId = 1;
            SonarqubeProject project = null;

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(projectId)).Returns(project);

            // Act
            var result = service.GetSonarQubeProject(projectId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetSonarQubeProjectById_NoCondition_ReturnSonarQubeProject()
        {
            // Arrange
            var projectId = 1;
            var project = new SonarqubeProject();
            var recordUpdateInfo = new RecordUpdateInfo();

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(projectId)).Returns(project);
            repository.Setup(x => x.GetApplicationSonarQubeProjectUpdateInfo(projectId)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetSonarQubeProject(projectId);

            // Assert
            result.Should().Be(project);
        }

        #endregion

        #region GetSonarQubeProjectTypes

        [Test]
        public void GetSonarQubeProjectTypes_NoCondition_ReturnSonarQubeProjectTypeList()
        {
            // Arrange
            var projectTypeList = new List<SonarQubeProjectType>();

            repository.Setup(x => x.GetSonarQubeProjectTypes()).Returns(projectTypeList);

            // Act
            var result = service.GetSonarQubeProjectTypes();

            // Assert
            result.Should().BeSameAs(projectTypeList);
        }

        #endregion

        #region AddApplicationSonarQubeProject

        [Test]
        public void AddApplicationSonarQubeProject_Fails_ReturnServiceErrorResult()
        {
            // Arrange
            var project = new SonarqubeProject();
            var addResult = false;

            repository.Setup(x => x.AddApplicationSonarQubeProject(project)).Returns(addResult);

            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            // Act
            var result = service.AddApplicationSonarQubeProject(project);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationSonarQubeProject_Succeeds_ReturnServiceErrorResult()
        {
            var project = new SonarqubeProject();
            var addResult = true;

            repository.Setup(x => x.AddApplicationSonarQubeProject(project)).Returns(addResult);

            var serviceResult = ServiceResult.Success(Messages.ApplicationSonarQubeProjectCreated);

            // Act
            var result = service.AddApplicationSonarQubeProject(project);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region UpdateApplicationSonarQubeProject

        [Test]
        public void UpdateApplicationSonarQubeProject_ApplicationSonarqubeProjectIsNull_ReturnServiceErrorResult()
        {
            // Arrange
            SonarqubeProject appSonarqubeProject = null;

            // Act
            var result = service.UpdateApplicationSonarQubeProject(appSonarqubeProject);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSonarQubeProject_ApplicationSonarqubeProjectHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var appSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var oldAppSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(appSonarqubeProject.SonarqubeProjectId)).Returns(oldAppSonarqubeProject);
            auditService.Setup(x => x.IsChanged(oldAppSonarqubeProject, appSonarqubeProject, nameof(SonarqubeProject))).Returns(false);

            // Act
            var result = service.UpdateApplicationSonarQubeProject(appSonarqubeProject);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationSonarQubeProjectUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSonarQubeProject_ApplicationSonarqubeProjectUpdateFails_ReturnServerError()
        {
            // Arrange
            var appSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var oldAppSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(appSonarqubeProject.SonarqubeProjectId)).Returns(oldAppSonarqubeProject);
            auditService.Setup(x => x.IsChanged(oldAppSonarqubeProject, appSonarqubeProject, nameof(SonarqubeProject))).Returns(true);
            repository.Setup(x => x.UpdateApplicationSonarQubeProject(appSonarqubeProject)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationSonarQubeProject(appSonarqubeProject);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSonarQubeProject_ApplicationSonarqubeProjectAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var appSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var oldAppSonarqubeProject = new SonarqubeProject();

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(appSonarqubeProject.SonarqubeProjectId)).Returns(appSonarqubeProject);
            auditService.Setup(x => x.IsChanged(appSonarqubeProject, appSonarqubeProject, nameof(SonarqubeProject))).Returns(true);
            repository.Setup(x => x.UpdateApplicationSonarQubeProject(appSonarqubeProject)).Returns(true);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationSonarQubeProject(appSonarqubeProject);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSonarQubeProject_UpdateApplicationSonarqubeProjectError_ReturnServiceErrorResult()
        {
            // Arrange
            var appSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var oldAppSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var auditInfo = new AuditInfo();

            var serviceResult = ServiceResult.Error(Messages.UpdateFails);

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(appSonarqubeProject.SonarqubeProjectId)).Returns(oldAppSonarqubeProject);
            auditService.Setup(x => x.IsChanged(oldAppSonarqubeProject, appSonarqubeProject, nameof(SonarqubeProject))).Returns(true);
            repository.Setup(x => x.UpdateApplicationSonarQubeProject(appSonarqubeProject)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(SonarqubeProject), oldAppSonarqubeProject.SonarqubeProjectId, oldAppSonarqubeProject, oldAppSonarqubeProject, appSonarqubeProject.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationSonarQubeProject(appSonarqubeProject);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateApplicationSonarQubeProject_UpdateApplicationSonarqubeProjectSucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var appSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var oldAppSonarqubeProject = new SonarqubeProject { SonarqubeProjectId = 3, SonarqubeProjectTypeName = "Test" };
            var auditInfo = new AuditInfo();

            var serviceResult = ServiceResult.Success(Messages.ApplicationSonarQubeProjectUpdated);

            repository.Setup(x => x.GetApplicationSonarQubeProjectById(appSonarqubeProject.SonarqubeProjectId)).Returns(oldAppSonarqubeProject);
            auditService.Setup(x => x.IsChanged(oldAppSonarqubeProject, appSonarqubeProject, nameof(SonarqubeProject))).Returns(true);
            repository.Setup(x => x.UpdateApplicationSonarQubeProject(appSonarqubeProject)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(SonarqubeProject), oldAppSonarqubeProject.SonarqubeProjectId, oldAppSonarqubeProject, oldAppSonarqubeProject, appSonarqubeProject.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationSonarQubeProject(appSonarqubeProject);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region DeleteApplicationSonarQubeProject

        [Test]
        public void DeleteApplicationSonarQubeProject_DeleteFails_ReturnServiceErrorResult()
        {
            // Arrange
            var projectId = 1;
            var updateResult = false;

            repository.Setup(x => x.DeleteApplicationSonarQubeProject(projectId)).Returns(updateResult);

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            // Act
            var result = service.DeleteApplicationSonarQubeProject(projectId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationSonarQubeProject_DeleteSucceeds_ReturnServiceSuccessResult()
        {
            var projectId = 1;
            var updateResult = true;

            repository.Setup(x => x.DeleteApplicationSonarQubeProject(projectId)).Returns(updateResult);

            var serviceResult = ServiceResult.Success(Messages.ApplicationSonarQubeProjectDeleted);

            // Act
            var result = service.DeleteApplicationSonarQubeProject(projectId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}