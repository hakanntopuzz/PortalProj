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
    public class DotnetCoreScriptServiceTests
    {
        #region members & setup

        StrictMock<IJenkinsService> jenkinsService;

        DotnetCoreBuildScriptService service;

        [SetUp]
        public void Initialize()
        {
            jenkinsService = new StrictMock<IJenkinsService>();
            service = new DotnetCoreBuildScriptService(jenkinsService.Object);
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
            var script = $@"dotnet build --no-restore {buildSettings.Workspace}\src\{buildSettings.SolutionName}.sln";

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
            var script = $@"dotnet restore {buildSettings.Workspace}\src\{buildSettings.SolutionName}.sln";

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
            var script = $@"dotnet test --no-restore --no-build {buildSettings.Workspace}\src\{buildSettings.SolutionName}\{buildSettings.SolutionName}.UnitTests\{buildSettings.SolutionName}.UnitTests.csproj --logger \nunit;LogFilePath='{buildSettings.Workspace}\{jobName}-UnitTestResults.xml'";

            jenkinsService.Setup(x => x.GetJobNameByApplicationIdAsync(request.ApplicationId)).ReturnsAsync(jobName);

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
            var jobName = "job-name";
            var script = $@"dotnet test --no-restore workspace\src\solution-name\solution-name.UnitTests\solution-name.UnitTests.csproj /p:CollectCoverage=true /p:Exclude=[*] *.Wrappers.* /p:CoverletOutputFormat=opencover /p:CoverletOutput=workspace\{jobName}-OpenCoverResults.xml";

            jenkinsService.Setup(x => x.GetJobNameByApplicationIdAsync(request.ApplicationId)).ReturnsAsync(jobName);

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