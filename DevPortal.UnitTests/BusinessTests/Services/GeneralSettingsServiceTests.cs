using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GeneralSettingsServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IGeneralSettingsRepository> generalSettingsRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        GeneralSettingsService service;

        [SetUp]
        public void Initialize()
        {
            generalSettingsRepository = new StrictMock<IGeneralSettingsRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new GeneralSettingsService(
                generalSettingsRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            generalSettingsRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region  update general settings and add log

        [Test]
        public void UpdateGeneralSettingsAndAddlLog_UpdateGeneralSettingsNotSuccess_ReturnFalse()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            var oldGeneralSettings = new GeneralSettings();

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(oldGeneralSettings);
            auditService.Setup(x => x.IsChanged(oldGeneralSettings, generalSettings, nameof(GeneralSettingsEditModel))).Returns(true);
            generalSettingsRepository.Setup(x => x.UpdateGeneralSettings(generalSettings)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<TransactionIstopException>()));

            // Act
            var result = service.UpdateGeneralSettings(generalSettings);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.GeneralSettingsUpdateErrorOccured);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateGeneralSettingsAndAddlLog_HasNoChanges_ReturnTrue()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            var oldGeneralSettings = new GeneralSettings();

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(oldGeneralSettings);
            auditService.Setup(x => x.IsChanged(oldGeneralSettings, generalSettings, nameof(GeneralSettingsEditModel))).Returns(false);

            // Act

            var result = service.UpdateGeneralSettings(generalSettings);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.GeneralSettingsUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateGeneralSettingsAndAddlLog_Fails_ReturnSuccessResult()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            var oldGeneralSettings = new GeneralSettings();
            var auditInfo = new AuditInfo();

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(oldGeneralSettings);
            auditService.Setup(x => x.IsChanged(oldGeneralSettings, generalSettings, nameof(GeneralSettingsEditModel))).Returns(true);
            generalSettingsRepository.Setup(x => x.UpdateGeneralSettings(generalSettings)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(GeneralSettings), generalSettings.Id, oldGeneralSettings, oldGeneralSettings, generalSettings.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<TransactionIstopException>()));

            // Act
            var result = service.UpdateGeneralSettings(generalSettings);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.GeneralSettingsUpdateErrorOccured);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateGeneralSettingsAndAddlLog_Success_ReturnSuccessResult()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            var oldGeneralSettings = new GeneralSettings();
            var auditInfo = new AuditInfo();

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(oldGeneralSettings);
            auditService.Setup(x => x.IsChanged(oldGeneralSettings, generalSettings, nameof(GeneralSettingsEditModel))).Returns(true);
            generalSettingsRepository.Setup(x => x.UpdateGeneralSettings(generalSettings)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(GeneralSettings), generalSettings.Id, oldGeneralSettings, oldGeneralSettings, generalSettings.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateGeneralSettings(generalSettings);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.GeneralSettingsUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  get general settings

        [Test]
        public void GetGeneralSettings_NoCondition_ReturnModel()
        {
            // Arrange
            var generalSettings = new GeneralSettings();

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetGeneralSettings();

            // Assert
            result.Should().Be(generalSettings);
        }

        #endregion

        #region  get svn url

        [Test]
        public void GetSvnUrl_NoCondition_ReturnSvnUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                SvnUrl = "http://wwww.example.com/svn-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetSvnUrl();

            // Assert
            result.Should().Be(generalSettings.SvnUrl);
        }

        #endregion

        #region GetNugetServerUrl

        [Test]
        public void GetNugetServerUrl_NoCondition_ReturnGetNugetServerUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                NugetUrl = "http://nuget/url"
            };

            // Act
            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            var result = service.GetNugetServerUrl();

            // Assert
            result.Should().Be(generalSettings.NugetUrl);
        }

        #endregion

        #region  get nuget package archive folder path

        [Test]
        public void GetNugetPackageArchiveFolderPath_NoCondition_ReturnNugetPackageArchiveFolderPath()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                NugetPackageArchiveFolderPath = "NugetPackageArchiveFolderPath"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetNugetPackageArchiveFolderPath();

            // Assert
            result.Should().Be(generalSettings.NugetPackageArchiveFolderPath);
        }

        #endregion

        #region  get jenkins url

        [Test]
        public void GetJenkinsUrl_NoCondition_ReturnJenkinsUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                JenkinsUrl = "http://wwww.example.com/jenkins-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetJenkinsUrl();

            // Assert
            result.Should().Be(generalSettings.JenkinsUrl);
        }

        #endregion

        #region  get jenkins job url

        [Test]
        public void GetJenkinsJobUrl_NoCondition_ReturnJenkinsJobUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                JenkinsUrl = "http://wwww.example.com/jenkins-url"
            };

            var jenkinsJobUrl = $"{generalSettings.JenkinsUrl}job/";

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetJenkinsJobUrl();

            // Assert
            result.Should().Be(jenkinsJobUrl);
        }

        #endregion

        #region  get sonarqube url

        [Test]
        public void GetSonarQubeUrl_NoCondition_ReturnSonarQubeUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                SonarQubeUrl = "http://wwww.example.com/sonarqube-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetSonarQubeUrl();

            // Assert
            result.Should().Be(generalSettings.SonarQubeUrl);
        }

        #endregion

        #region  get redmine url

        [Test]
        public void GetRedmineUrl_NoCondition_ReturnRedmineUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                RedmineUrl = "http://wwww.example.com/redmine-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetRedmineUrl();

            // Assert
            result.Should().Be(generalSettings.RedmineUrl);
        }

        #endregion

        #region  get redmine project url

        [Test]
        public void GetRedmineProjectUrl_NoCondition_ReturnRedmineProjectUrl()
        {
            // Arrange
            var projectName = "project-name";

            var generalSettings = new GeneralSettings
            {
                RedmineUrl = "http://wwww.example.com/redmine-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetRedmineProjectUrl(projectName);

            // Assert
            result.Should().Be($"{generalSettings.RedmineUrl}projects/{projectName}");
        }

        #endregion

        #region  get sonarQube project url

        [Test]
        public void GetSonarqubeProjectUrl_NoCondition_ReturnSonarqubeProjectUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                SonarQubeUrl = "http://wwww.example.com/sonarqube-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetSonarqubeProjectUrl();

            // Assert
            result.Should().Be($"{generalSettings.SonarQubeUrl}dashboard/?id=");
        }

        #endregion

        #region  get jenkins job url

        [Test]
        public void GetJenkinsJobUrl_NoCondition_ReturnUrl()
        {
            // Arrange
            var name = "job-name";
            var generalSettings = new GeneralSettings
            {
                JenkinsUrl = "http://wwww.example.com/jenkins-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetJenkinsJobUrl(name);

            // Assert
            result.Should().Be($"{generalSettings.JenkinsUrl}job/{name}/api/json");
        }

        #endregion

        #region  get jenkins jobs url

        [Test]
        public void GetJenkinsJobsUrl_NoCondition_ReturnUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                JenkinsUrl = "http://wwww.example.com/jenkins-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetJenkinsJobsUrl();

            // Assert
            result.Should().Be($"{generalSettings.JenkinsUrl}api/json");
        }

        #endregion

        #region  get jenkins failed jobs url

        [Test]
        public void GetJenkinsFailedJobsUrl_NoCondition_ReturnUrl()
        {
            // Arrange
            var generalSettings = new GeneralSettings
            {
                JenkinsUrl = "http://wwww.example.com/jenkins-url"
            };

            generalSettingsRepository.Setup(x => x.GetGeneralSettings()).Returns(generalSettings);

            // Act
            var result = service.GetJenkinsFailedJobsUrl();

            // Assert
            result.Should().Be($"{generalSettings.JenkinsUrl}rssFailed");
        }

        #endregion
    }
}