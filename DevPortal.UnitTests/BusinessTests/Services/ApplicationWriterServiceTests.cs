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
    public class ApplicationWriterServiceTest : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationRepository> applicationRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationWriterService service;

        [SetUp]
        public void Initialize()
        {
            applicationRepository = new StrictMock<IApplicationRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationWriterService(
                applicationRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region add application

        [Test]
        public void AddApplication_ApplicationNull_ReturnServiceErrorResult()
        {
            // Arrange
            Application application = null;
            var serviceResult = ServiceResult.Error(Messages.NullParameterError);

            // Act
            var result = service.AddApplication(application);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplication_AddApplicationFails_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationModel = new Application();
            Application application = null;
            var applicationId = 0;
            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(applicationModel.Name)).Returns(application);
            applicationRepository.Setup(x => x.AddApplication(applicationModel)).Returns(applicationId);

            // Act
            var result = service.AddApplication(applicationModel);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplication_AddApplicationIsExits_ReturnServiceErrorResult()
        {
            // Arrange
            var application = new Application();
            var serviceResult = ServiceResult.Error(Messages.ApplicationNameExists);

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(application.Name)).Returns(application);

            // Act
            var result = service.AddApplication(application);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplication_AddApplicationSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationModel = new Application();
            Application application = null;
            var applicationId = 1054;
            var serviceResult = ServiceResult.Success(Messages.ApplicationCreated);

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(applicationModel.Name)).Returns(application);
            applicationRepository.Setup(x => x.AddApplication(applicationModel)).Returns(applicationId);

            // Act
            var result = service.AddApplication(applicationModel);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region edit application

        [Test]
        public void EditApplication_ApplicationNull_ReturnServiceErrorResult()
        {
            // Arrange
            Application application = null;

            // Act
            var result = service.UpdateApplication(application);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditApplication_ApplicationExists_ReturnApplicationNameExistsError()
        {
            // Arrange
            var application = new Application { Id = 1, Name = "Test" };
            var app = new Application { Id = 2, Name = "Test" };

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(application.Name)).Returns(app);

            // Act
            var result = service.UpdateApplication(application);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.ApplicationNameExists);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditApplication_ApplicationHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var application = new Application
            {
                Id = 3,
                Name = "Test",
                ApplicationGroupId = 1,
                ApplicationTypeId = 2,
                RedmineProjectName = "redmine",
                Description = null,
                StatusId = 1905
            };
            var app = new Application
            {
                Id = 3,
                Name = "Test"
            };
            var oldApplication = new Application
            {
                Id = 3,
                Name = "Test",
                ApplicationGroupId = 1,
                ApplicationTypeId = 2,
                RedmineProjectName = "redmine",
                Description = null,
                StatusId = 1905
            };

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(application.Name)).Returns(app);
            applicationRepository.Setup(x => x.GetApplication(application.Id)).Returns(oldApplication);
            auditService.Setup(x => x.IsChanged(oldApplication, application, nameof(BaseApplication))).Returns(false);

            // Act
            var result = service.UpdateApplication(application);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditApplication_ApplicationDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var application = new Application
            {
                Id = 1,
                Name = "Test",
                Description = null
            };
            var app = new Application
            {
                Id = 1,
                Name = "Test"
            };
            var oldApplication = new Application
            {
                Id = 1,
                Name = "Test",
                Description = "asd"
            };

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(application.Name)).Returns(app);
            applicationRepository.Setup(x => x.GetApplication(application.Id)).Returns(oldApplication);
            auditService.Setup(x => x.IsChanged(oldApplication, application, nameof(BaseApplication))).Returns(true);
            applicationRepository.Setup(x => x.UpdateApplication(application)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplication(application);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditApplication_ApplicationDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            Application application = new Application
            {
                Id = 1,
                Name = "Test"
            };
            var app = new Application
            {
                Id = 1,
                Name = "Testt"
            };
            var oldApplication = new Application();
            var auditInfo = new AuditInfo();

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(application.Name)).Returns(app);
            applicationRepository.Setup(x => x.GetApplication(application.Id)).Returns(oldApplication);
            auditService.Setup(x => x.IsChanged(oldApplication, application, nameof(BaseApplication))).Returns(true);
            applicationRepository.Setup(x => x.UpdateApplication(application)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Application), oldApplication.Id, oldApplication, oldApplication, application.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplication(application);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void EditApplication_ApplicationUpdateSuccess_ReturnApplicationUpdated()
        {
            // Arrange
            var application = new Application
            {
                Id = 1,
                Name = "Test"
            };
            var app = new Application
            {
                Id = 1,
                Name = "Testt"
            };
            var oldApplication = new Application();
            var auditInfo = new AuditInfo();

            applicationRepository.Setup(x => x.GetApplicationByApplicationName(application.Name)).Returns(app);
            applicationRepository.Setup(x => x.GetApplication(application.Id)).Returns(oldApplication);
            auditService.Setup(x => x.IsChanged(oldApplication, application, nameof(BaseApplication))).Returns(true);
            applicationRepository.Setup(x => x.UpdateApplication(application)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Application), oldApplication.Id, oldApplication, oldApplication, application.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplication(application);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region delete application

        [Test]
        public void DeleteApplication_Fails_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationId = 1;
            var deleteResult = false;

            applicationRepository.Setup(x => x.DeleteApplication(applicationId)).Returns(deleteResult);

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            // Act
            var result = service.DeleteApplication(applicationId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplication_Succeeds_ReturnServiceErrorResult()
        {
            // Arrange
            var applicationId = 1;
            var deleteResult = true;

            applicationRepository.Setup(x => x.DeleteApplication(applicationId)).Returns(deleteResult);

            var serviceResult = ServiceResult.Success(Messages.ApplicationDeleted);

            // Act
            var result = service.DeleteApplication(applicationId);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}