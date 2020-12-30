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
    public class ApplicationJenkinsJobServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationJenkinsJobRepository> repository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationJenkinsJobService service;

        [SetUp]
        public void Initialize()
        {
            repository = new StrictMock<IApplicationJenkinsJobRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationJenkinsJobService(
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

        #region GetJenkinsJobTypes

        [Test]
        public void GetJenkinsJobTypes_NoCondition_ReturnJenkinsJobTypeList()
        {
            // Arrange
            var jenkinsJobTypeList = new List<JenkinsJobType>();

            repository.Setup(x => x.GetJenkinsJobTypes()).Returns(jenkinsJobTypeList);

            // Act
            var result = service.GetJenkinsJobTypes();

            // Assert
            result.Should().BeSameAs(jenkinsJobTypeList);
        }

        #endregion

        #region AddApplicationJenkinsJob

        [Test]
        public void AddApplicationJenkinsJob_Fails_ReturnServiceErrorResult()
        {
            // Arrange
            var jenkinsJob = new JenkinsJob();
            var addResult = false;

            repository.Setup(x => x.AddApplicationJenkinsJob(jenkinsJob)).Returns(addResult);

            var serviceResult = ServiceResult.Error(Messages.AddingFails);

            // Act
            var result = service.AddApplicationJenkinsJob(jenkinsJob);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddApplicationJenkinsJob_Succeeds_ReturnServiceErrorResult()
        {
            var jenkinsJob = new JenkinsJob();
            var addResult = true;

            repository.Setup(x => x.AddApplicationJenkinsJob(jenkinsJob)).Returns(addResult);

            var serviceResult = ServiceResult.Success(Messages.ApplicationJenkinsJobCreated);

            // Act
            var result = service.AddApplicationJenkinsJob(jenkinsJob);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region GetApplicationJenkinsJobById

        [Test]
        public void GetApplicationJenkinsJobById_JobNotFound_ReturnNull()
        {
            // Arrange
            var jenkinsJobId = 1;
            JenkinsJob jenkinsJob = null;

            repository.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJobId)).Returns(jenkinsJob);

            // Act
            var result = service.GetApplicationJenkinsJob(jenkinsJobId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetApplicationJenkinsJobById_JobFound_ReturnJenkinsJob()
        {
            // Arrange
            var jenkinsJobId = 1;
            var jenkinsJob = new JenkinsJob();
            var recordUpdateInfo = new RecordUpdateInfo();

            repository.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJobId)).Returns(jenkinsJob);
            repository.Setup(x => x.GetApplicationJenkinsJobUpdateInfo(jenkinsJobId)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetApplicationJenkinsJob(jenkinsJobId);

            // Assert
            result.Should().Be(jenkinsJob);
        }

        #endregion

        #region UpdateApplicationJenkinsJob

        [Test]
        public void UpdateApplicationJenkinsJob_ApplicationJenkinsJobNull_ReturnServiceErrorResult()
        {
            // Arrange
            JenkinsJob jenkinsJob = null;

            // Act
            var result = service.UpdateApplicationJenkinsJob(jenkinsJob);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationJenkinsJob_ApplicationJenkinsJobHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var jenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1905 };
            var oldJenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1905 };

            repository.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJob.JenkinsJobId)).Returns(oldJenkinsJob);
            auditService.Setup(x => x.IsChanged(oldJenkinsJob, jenkinsJob, nameof(JenkinsJob))).Returns(false);

            // Act
            var result = service.UpdateApplicationJenkinsJob(jenkinsJob);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationJenkinsJobUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationJenkinsJob_ApplicationJenkinsJobDoesNotExistUpdateFails_ReturnServerError()
        {
            // Arrange
            var jenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1903 };
            var oldJenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1905 };

            repository.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJob.JenkinsJobId)).Returns(oldJenkinsJob);
            auditService.Setup(x => x.IsChanged(oldJenkinsJob, jenkinsJob, nameof(JenkinsJob))).Returns(true);
            repository.Setup(x => x.UpdateApplicationJenkinsJob(jenkinsJob)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationJenkinsJob(jenkinsJob);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationJenkinsJob_ApplicationJenkinsJobDoesNotExistAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var jenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1903 };
            var oldJenkinsJob = new JenkinsJob();
            var auditInfo = new AuditInfo();

            repository.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJob.JenkinsJobId)).Returns(oldJenkinsJob);
            auditService.Setup(x => x.IsChanged(oldJenkinsJob, jenkinsJob, nameof(JenkinsJob))).Returns(true);
            repository.Setup(x => x.UpdateApplicationJenkinsJob(jenkinsJob)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(JenkinsJob), oldJenkinsJob.JenkinsJobId, oldJenkinsJob, oldJenkinsJob, jenkinsJob.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationJenkinsJob(jenkinsJob);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateJenkinsJob_UpdateApplicationJenkinsJobSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var jenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1903 };
            var oldJenkinsJob = new JenkinsJob { JenkinsJobId = 3, JenkinsJobName = "Test", JenkinsJobTypeId = 1905 };
            var auditInfo = new AuditInfo();

            repository.Setup(x => x.GetApplicationJenkinsJobById(jenkinsJob.JenkinsJobId)).Returns(oldJenkinsJob);
            auditService.Setup(x => x.IsChanged(oldJenkinsJob, jenkinsJob, nameof(JenkinsJob))).Returns(true);
            repository.Setup(x => x.UpdateApplicationJenkinsJob(jenkinsJob)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(JenkinsJob), oldJenkinsJob.JenkinsJobId, oldJenkinsJob, oldJenkinsJob, jenkinsJob.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationJenkinsJob(jenkinsJob);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationJenkinsJobUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region DeleteApplicationJenkinsJob

        [Test]
        public void DeleteApplicationJenkinsJob_NoCondition_ReturnTrue()
        {
            // Arrange
            var id = 5;

            // Act
            repository.Setup(x => x.DeleteApplicationJenkinsJob(id)).Returns(true);

            var serviceResult = ServiceResult.Success(Messages.ApplicationJenkinsJobDeleted);

            var result = service.DeleteApplicationJenkinsJob(id);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteApplicationJenkinsJob_NoCondition_ReturnFalse()
        {
            // Arrange
            var id = 5;

            // Act
            repository.Setup(x => x.DeleteApplicationJenkinsJob(id)).Returns(false);

            var serviceResult = ServiceResult.Error(Messages.DeleteFails);

            var result = service.DeleteApplicationJenkinsJob(id);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion
    }
}