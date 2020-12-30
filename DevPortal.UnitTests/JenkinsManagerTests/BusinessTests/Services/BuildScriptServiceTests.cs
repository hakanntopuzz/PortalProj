using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Business.Services;
using DevPortal.JenkinsManager.Model;
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
    public class BuildScriptServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IBuildScriptRepository> repository;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationBuildScriptServiceProvider> serviceProvider;

        StrictMock<IApplicationBuildSettingsService> buildSettingsService;

        ApplicationBuildScriptService service;

        [SetUp]
        public void Initialize()
        {
            repository = new StrictMock<IBuildScriptRepository>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            serviceProvider = new StrictMock<IApplicationBuildScriptServiceProvider>();
            buildSettingsService = new StrictMock<IApplicationBuildSettingsService>();

            service = new ApplicationBuildScriptService(
                repository.Object,
                applicationReaderService.Object,
                serviceProvider.Object,
                buildSettingsService.Object);
        }

        Mock<IBuildScriptService> CreateScriptService(ApplicationTypeEnum applicationType)
        {
            var scriptService = new Mock<IBuildScriptService>();

            serviceProvider.Setup(x => x.GetBuildScriptService(applicationType)).Returns(scriptService.Object);

            return scriptService;
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            repository.VerifyAll();
            applicationReaderService.VerifyAll();
            serviceProvider.VerifyAll();
        }

        #endregion

        [Test]
        public void GetApplications_NoCondition_ReturnApplicationList()
        {
            // Arrange
            var applications = new List<ApplicationListItem>();

            applicationReaderService.Setup(x => x.GetApplications()).Returns(applications);

            // Act
            var result = service.Applications;

            // Assert
            result.Should().BeEquivalentTo(applications);
        }

        [Test]
        public void GetBuildTypes_NoCondition_ReturnBuildTypeList()
        {
            // Arrange
            var list = new List<BuildType>();

            repository.Setup(x => x.BuildTypes).Returns(list);

            // Act
            var result = service.BuildTypes;

            // Assert
            result.Should().BeEquivalentTo(list);
        }

        [Test]
        public void CreateBuildScript_RequestIsNull_ReturnErrorResult()
        {
            // Arrange
            BuildScriptRequest request = null;
            var serviceResult = BuildScriptServiceResult.Error(Messages.BuildScriptCreationFails);

            // Act
            var result = service.CreateBuildScript(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void CreateBuildScript_ApplicationTypeIsNull_ReturnApplicationTypeNone()
        {
            // Arrange
            var request = new BuildScriptRequest { BuildTypeId = 1, ApplicationId = 1 };
            ApplicationType applicationType = null;
            var serviceResult = BuildScriptServiceResult.Error(Messages.BuildScriptCreationFails);

            applicationReaderService.Setup(x => x.GetApplicationTypeByApplicationId(request.ApplicationId)).Returns(applicationType);

            // Act
            var result = service.CreateBuildScript(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void CreateBuildScript_CreateScriptService_ReturnBuildScriptServiceResult()
        {
            // Arrange
            var request = new BuildScriptRequest { BuildTypeId = 1, ApplicationId = 1 };
            var applicationType = new ApplicationType { Id = (int)ApplicationTypeEnum.AspNetMvc };
            var applicationTypeEnum = (ApplicationTypeEnum)Enum.ToObject(typeof(ApplicationTypeEnum), applicationType.Id);
            var buildTypeEnum = (BuildTypes)Enum.ToObject(typeof(BuildTypes), request.BuildTypeId);
            var buildSettings = new ApplicationBuildSettings { ApplicationId = 1 };
            var script = It.IsAny<string>();
            var serviceResult = BuildScriptServiceResult.Success(script);

            applicationReaderService.Setup(x => x.GetApplicationTypeByApplicationId(request.ApplicationId)).Returns(applicationType);
            buildSettingsService.Setup(x => x.GetApplicationBuildSettings(request.ApplicationId)).Returns(buildSettings);

            var scriptService = CreateScriptService(applicationTypeEnum);
            scriptService.Setup(x => x.CreateScript(buildSettings, buildTypeEnum, request)).Returns(serviceResult);

            // Act
            var result = service.CreateBuildScript(request);

            // Assert
            result.Should().Be(serviceResult);
        }
    }
}