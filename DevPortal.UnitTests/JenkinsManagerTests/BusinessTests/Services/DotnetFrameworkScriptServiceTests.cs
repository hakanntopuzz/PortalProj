using AB.Framework.UnitTests;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Business.Services;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DotnetFrameworkScriptServiceTests
    {
        #region members & setup

        StrictMock<IJenkinsService> jenkinsService;

        DotnetFrameworkBuildScriptService service;

        [SetUp]
        public void Initialize()
        {
            jenkinsService = new StrictMock<IJenkinsService>();
            service = new DotnetFrameworkBuildScriptService(jenkinsService.Object);
        }

        #endregion

        #region Create Build Project

        [Test]
        public void CreateBuildProject_VariableIsNull_ReturnEmptyString()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateBuildProject(buildSettings);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateBuildProject_WorkspaceIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = null };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateBuildProject(buildSettings);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateBuildProject_SolutionNameIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = "workspace", SolutionName = null };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateBuildProject(buildSettings);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateBuildProject_VariableExists_ReturnBuildScript()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = "solution-name",
                DeployPath = "deploy-path"
            };
            var script = $@"C:\Program Files(x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe {buildSettings.Workspace}\src\{ buildSettings.SolutionName} /t:Rebuild";

            // Act
            var result = service.CreateBuildProject(buildSettings);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(script);
        }

        #endregion

        #region Create Package Restore

        [Test]
        public void CreatePackageRestore_VariableIsNull_ReturnEmptyString()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreatePackageRestore(buildSettings);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreatePackageRestore_WorkspaceIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = null };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreatePackageRestore(buildSettings);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreatePackageRestore_SolutionNameIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = "workspace", SolutionName = null };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreatePackageRestore(buildSettings);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreatePackageRestore_VariableExists_ReturnBuildScript()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = "solution-name",
                DeployPath = "deploy-path"
            };
            var script = $@"C:\ci-tools\nuget\nuget.exe restore {buildSettings.Workspace}\src\{ buildSettings.SolutionName} -ConfigFile C:\ci-tools\nuget\nuget.config";

            // Act
            var result = service.CreatePackageRestore(buildSettings);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(script);
        }

        #endregion

        #region Create Unit Test

        [Test]
        public void CreateUnitTest_RequestIsNull_ReturnInvalidRequest()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            BuildScriptRequest request = null;
            var serviceResult = BuildScriptServiceResult.Error(Messages.InvalidRequest);

            // Act
            var result = service.CreateUnitTest(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(serviceResult.Message);
        }

        [Test]
        public void CreateUnitTest_VariableIsNull_ReturnEmptyString()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateUnitTest(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateUnitTest_WorkspaceIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = null };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateUnitTest(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateUnitTest_SolutionNameIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = "workspace", SolutionName = null };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateUnitTest(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateUnitTest_VariableExists_ReturnBuildScript()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = "solution-name",
                DeployPath = "deploy-path"
            };

            var request = new BuildScriptRequest { ApplicationId = 1 };
            var jobName = "job-name";

            jenkinsService.Setup(x => x.GetJobNameByApplicationIdAsync(request.ApplicationId)).ReturnsAsync(jobName);

            var script = $@"C:\ci-tools\NUnit.Console-3.10.0\bin\net35\nunit3-console.exe {buildSettings.Workspace}\src\{ buildSettings.SolutionName} /result:{jobName}-UnitTestResults.xml";

            // Act
            var result = service.CreateUnitTest(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(script);
        }

        #endregion

        #region Create Test Coverage

        [Test]
        public void CreateTestCoverage_RequestIsNull_ReturnInvalidRequest()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            BuildScriptRequest request = null;
            var serviceResult = BuildScriptServiceResult.Error(Messages.InvalidRequest);

            // Act
            var result = service.CreateTestCoverage(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(serviceResult.Message);
        }

        [Test]
        public void CreateTestCoverage_VariableIsNull_ReturnEmptyString()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateTestCoverage(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateTestCoverage_WorkspaceIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = null };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateTestCoverage(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateTestCoverage_SolutionNameIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings { Workspace = "workspace", SolutionName = null };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateTestCoverage(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateTestCoverage_VariableExists_ReturnBuildScript()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = "solution-name",
                DeployPath = "deploy-path"
            };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var script = $@"C:\ci-tools\opencover.4.7.922\OpenCover.Console.exe -target:C:\ci-tools\NUnit.Console-3.10.0\bin\net35\nunit3-console.exe -targetargs:{buildSettings.Workspace}\src\{ buildSettings.SolutionName}\{buildSettings.SolutionName}.UnitTests\bin\{buildSettings.SolutionName}.UnitTests.dll /result:{buildSettings.SolutionName}-UnitTestResults.xml";

            // Act
            var result = service.CreateTestCoverage(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(script);
        }

        #endregion

        #region Create Deploy Project

        [Test]
        public void CreateDeployProject_RequestIsNull_ReturnInvalidRequest()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            BuildScriptRequest request = null;
            var serviceResult = BuildScriptServiceResult.Error(Messages.InvalidRequest);

            // Act
            var result = service.CreateDeployProject(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(serviceResult.Message);
        }

        [Test]
        public void CreateDeployProject_VariableIsNull_ReturnEmptyString()
        {
            // Arrange
            ApplicationBuildSettings buildSettings = null;
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateDeployProject(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateDeployProject_WorkspaceIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = null,
                SolutionName = "solution-name",
                DeployPath = "deploy-path"
            };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateDeployProject(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateDeployProject_SolutionNameIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = null,
                DeployPath = "deploy-path"
            };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateDeployProject(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateDeployProject_DeployPathIsNull_ReturnEmptyString()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = "solution-name",
                DeployPath = null
            };
            var request = new BuildScriptRequest { ApplicationId = 1 };
            var serviceResult = BuildScriptServiceResult.Error(string.Empty);

            // Act
            var result = service.CreateDeployProject(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(string.Empty);
        }

        [Test]
        public void CreateDeployProject_VariableExists_ReturnBuildScript()
        {
            // Arrange
            var buildSettings = new ApplicationBuildSettings
            {
                Workspace = "workspace",
                SolutionName = "solution-name",
                DeployPath = "deploy-path"
            };
            var request = new BuildScriptRequest { ApplicationId = 1 };

            // Act
            var result = service.CreateDeployProject(buildSettings, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        #endregion
    }
}