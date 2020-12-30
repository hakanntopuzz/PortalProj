using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using DevPortal.Validation;
using DevPortal.Validation.Abstract;
using DevPortal.Web.Library.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class JenkinsControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IJenkinsService> jenkinsService;

        StrictMock<IRequestValidator> requestValidator;

        StrictMock<IEnvironmentService> environmentService;

        StrictMock<IApplicationBuildScriptService> buildScriptService;

        StrictMock<IApplicationJenkinsJobService> jenkinsJobService;

        JenkinsController controller;

        [SetUp]
        public void Initialize()
        {
            jenkinsService = new StrictMock<IJenkinsService>();
            requestValidator = new StrictMock<IRequestValidator>();
            environmentService = new StrictMock<IEnvironmentService>();
            buildScriptService = new StrictMock<IApplicationBuildScriptService>();
            jenkinsJobService = new StrictMock<IApplicationJenkinsJobService>();

            controller = new JenkinsController(
                jenkinsService.Object,
                requestValidator.Object,
                environmentService.Object,
                buildScriptService.Object,
                jenkinsJobService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            jenkinsService.VerifyAll();
            requestValidator.VerifyAll();
            environmentService.VerifyAll();
            buildScriptService.VerifyAll();
            jenkinsJobService.VerifyAll();
        }

        #endregion

        [Test]
        public void Jobs_NoCondition_ReturnJobs()
        {
            // Arrange
            var jobs = new List<JenkinsJobItem>();

            jenkinsService.Setup(x => x.GetJobsAsync()).ReturnsAsync(jobs);

            // Act
            var result = controller.Jobs();

            // Assert
            result.Result.Should().BeSameAs(jobs);
        }

        [Test]
        public void FailedJobs_NoCondition_ReturnFailedJobs()
        {
            // Arrange
            var jobs = new List<JenkinsJobItem>();

            jenkinsService.Setup(x => x.GetFailedJobs()).Returns(jobs);

            // Act
            var result = controller.FailedJobs();

            // Assert
            result.Should().BeSameAs(jobs);
        }

        [Test]
        public void AllFailedJobs_NoCondition_ReturnFailedJobs()
        {
            // Arrange
            var jobs = new List<JenkinsJobItem>();

            jenkinsService.Setup(x => x.GetAllFailedJobs()).Returns(jobs);

            // Act
            var result = controller.AllFailedJobs();

            // Assert
            result.Should().BeSameAs(jobs);
        }

        [Test]
        public void JobDetail_JobNameIsEmpty_ReturnNull()
        {
            // Arrange
            var name = "";

            // Act
            var result = controller.JobDetail(name);

            // Assert
            result.Result.Should().BeNull();
            jenkinsService.Verify(x => x.GetJobDetailAsync(""), Times.Never);
        }

        [Test]
        public void JobDetail_NoCondition_ReturnJobs()
        {
            // Arrange
            var name = "mx-kobi";
            var jobDetail = new JenkinsJobDetail();

            jenkinsService.Setup(x => x.GetJobDetailAsync(name)).ReturnsAsync(jobDetail);

            // Act
            var result = controller.JobDetail(name);

            // Assert
            result.Result.Should().BeSameAs(jobDetail);
        }

        [Test]
        public void GenerateBuildScript_InvalidRequest_ReturnError()
        {
            // Arrange
            var request = new BuildScriptRequest { ApplicationId = 0, BuildTypeId = 1 };
            var validationResult = ValidationResult.Error(It.IsAny<string>());

            requestValidator.Setup(x => x.Validate(request)).Returns(validationResult);

            // Act
            var result = controller.GenerateBuildScript(request);

            // Assert
            result.Should().BeEquivalentTo(new BadRequestObjectResult(validationResult.ErrorMessage));
        }

        [Test]
        public void GenerateBuildScript_GeneratingScriptFails_ReturnServiceErrorResult()
        {
            // Arrange
            var request = new BuildScriptRequest { ApplicationId = 1, BuildTypeId = 1 };
            var validationResult = ValidationResult.Success;
            var serviceResult = BuildScriptServiceResult.Error(It.IsAny<string>());

            requestValidator.Setup(x => x.Validate(request)).Returns(validationResult);
            buildScriptService.Setup(x => x.CreateBuildScript(request)).Returns(serviceResult);

            // Act
            var result = controller.GenerateBuildScript(request);

            // Assert
            result.Should().BeEquivalentTo(new OkObjectResult(serviceResult));
        }

        [Test]
        public void GenerateBuildScript_GeneratingScriptSucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var request = new BuildScriptRequest { ApplicationId = 1, BuildTypeId = 1 };
            var validationResult = ValidationResult.Success;
            var serviceResult = BuildScriptServiceResult.Success(It.IsAny<string>());

            requestValidator.Setup(x => x.Validate(request)).Returns(validationResult);
            buildScriptService.Setup(x => x.CreateBuildScript(request)).Returns(serviceResult);

            // Act
            var result = controller.GenerateBuildScript(request);

            // Assert
            result.Should().BeEquivalentTo(new OkObjectResult(serviceResult));
        }

        [Test]
        public void Applications_NoCondition_ReturnApplications()
        {
            // Arrange
            var list = new List<ApplicationListItem>();

            buildScriptService.Setup(x => x.Applications).Returns(list);

            // Act
            var result = controller.Applications();

            // Assert
            result.Should().BeEquivalentTo(list);
        }

        [Test]
        public void BuildTypes_NoCondition_ReturnBuildTypes()
        {
            // Arrange
            var list = new List<BuildType>();

            buildScriptService.Setup(x => x.BuildTypes).Returns(list);

            // Act
            var result = controller.BuildTypes();

            // Assert
            result.Should().BeEquivalentTo(list);
        }

        [Test]
        public void Environments_NoCondition_ReturnEnvironments()
        {
            // Arrange
            var list = new List<Environment>();

            environmentService.Setup(x => x.GetEnvironments()).Returns(list);

            // Act
            var result = controller.Environments();

            // Assert
            result.Should().BeEquivalentTo(list);
        }

        [Test]
        public void JenkinsJobTypes_NoCondition_ReturnJenkinsJobTypes()
        {
            // Arrange
            var list = new List<JenkinsJobType>();

            jenkinsJobService.Setup(x => x.GetJenkinsJobTypes()).Returns(list);

            // Act
            var result = controller.JenkinsJobTypes();

            // Assert
            result.Should().BeEquivalentTo(list);
        }
    }
}