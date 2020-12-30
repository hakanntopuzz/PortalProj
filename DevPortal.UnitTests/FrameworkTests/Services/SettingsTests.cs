using AB.Framework.SettingsReader.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Framework.Model;
using DevPortal.Framework.Services;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SettingsTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ISettingsReader> settingsReader;

        Settings settings;

        [SetUp]
        public void Initialize()
        {
            settingsReader = new StrictMock<ISettingsReader>();

            settings = new Settings(settingsReader.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            settingsReader.VerifyAll();
        }

        #endregion

        #region DevPortalDbConnectionString

        [Test]
        public void DevPortalDbConnectionString_NoCondition_ReturnValue()
        {
            //Arrange
            string value = "conn-string";
            ConfigKey configKey = ConfigKey.DevPortalDbConnectionString;

            settingsReader.Setup(x => x.GetConnectionString(configKey.ToString())).Returns(value);

            //Act
            var result = settings.DevPortalDbConnectionString;

            //Assert
            result.Should().Be(value);
        }

        #endregion

        #region SiteUrl

        [Test]
        public void SiteUrl_NoCondition_ReturnValue()
        {
            //Arrange
            string value = "siteUrl";
            ConfigKey configKey = ConfigKey.SiteUrl;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(value);

            //Act
            var result = settings.SiteUrl;

            //Assert
            result.Should().Be(value);
        }

        #endregion

        #region ApplicationVersion

        [Test]
        public void ApplicationVersion_NoCondition_ReturnValue()
        {
            //Arrange
            string value = "conn-string";
            ConfigKey configKey = ConfigKey.ApplicationVersion;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(value);

            //Act
            var result = settings.ApplicationVersion;

            //Assert
            result.Should().Be(value);
        }

        #endregion

        #region RegPathByAes

        [Test]
        public void RegPathByAes_NoCondition_ReturnValue()
        {
            //Arrange
            string value = "conn-string";
            ConfigKey configKey = ConfigKey.RegPathByAes;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(value);

            //Act
            var result = settings.RegPathByAes;

            //Assert
            result.Should().Be(value);
        }

        #endregion

        #region RegPathByTripleDes

        [Test]
        public void RegPathByTripleDes_NoCondition_ReturnValue()
        {
            //Arrange
            string value = "conn-string";
            ConfigKey configKey = ConfigKey.RegPathByTripleDes;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(value);

            //Act
            var result = settings.RegPathByTripleDes;

            //Assert
            result.Should().Be(value);
        }

        #endregion

        #region OldNugetPackagesPhysicalPath

        [Test]
        public void OldNugetPackagesPhysicalPath_NoCondition_ReturnValue()
        {
            //Arrange
            string value = "conn-string";
            ConfigKey configKey = ConfigKey.OldNugetPackagesPhysicalPath;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(value);

            //Act
            var result = settings.OldNugetPackagesPhysicalPath;

            //Assert
            result.Should().Be(value);
        }

        #endregion

        #region NugetPackageListCacheTimeInMinutes

        [Test]
        public void NugetPackageListCacheTimeInMinutes_NoCondition_ReturnValue()
        {
            //Arrange
            int expected = 123;
            ConfigKey configKey = ConfigKey.NugetPackageListCacheTimeInMinutes;

            settingsReader.Setup(x => x.GetAppSettingValueAsInt(configKey.ToString())).Returns(expected);

            //Act
            var result = settings.NugetPackageListCacheTimeInMinutes;

            //Assert
            result.Should().Be(expected);
        }

        #endregion

        #region CacheTime

        [Test]
        public void NumberOfNugetPackagePaging_NoCondition_ReturnValue()
        {
            //Arrange
            int expected = 100;
            ConfigKey configKey = ConfigKey.NumberOfNugetPackagePaging;

            settingsReader.Setup(x => x.GetAppSettingValueAsInt(configKey.ToString())).Returns(expected);

            //Act
            var result = settings.NumberOfNugetPackagePaging;

            //Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SshServerAddress

        [Test]
        public void SshServerAddress_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "mahmut";
            ConfigKey configKey = ConfigKey.SshServerAddress;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SshServerAddress;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SshUsername

        [Test]
        public void SshUsername_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "mahmut";
            ConfigKey configKey = ConfigKey.SshUsername;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SshUsername;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SshPrivateKeyFilePath

        [Test]
        public void SshPrivateKeyFilePath_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "mahmut";
            ConfigKey configKey = ConfigKey.SshPrivateKeyFilePath;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SshPrivateKeyFilePath;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region NugetPackagesPhysicalPath

        [Test]
        public void NugetPackagesPhysicalPath_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "NugetPackagesPhysicalPath";
            ConfigKey configKey = ConfigKey.NugetPackagesPhysicalPath;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.NugetPackagesPhysicalPath;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region JenkinsFailedJobCount

        [Test]
        public void JenkinsFailedJobCount_NoCondition_ReturnValue()
        {
            // Arrange
            const int expected = 5;
            ConfigKey configKey = ConfigKey.JenkinsFailedJobCount;

            settingsReader.Setup(x => x.GetAppSettingValueAsInt(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.JenkinsFailedJobCount;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SmtpPortNumber

        [Test]
        public void SmtpPortNumber_NoCondition_ReturnValue()
        {
            // Arrange
            const int expected = 5;
            ConfigKey configKey = ConfigKey.SmtpPortNumber;

            settingsReader.Setup(x => x.GetAppSettingValueAsInt(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SmtpPortNumber;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SmtpEnableSSL

        [Test]
        public void SmtpEnableSSL_NoCondition_ReturnValue()
        {
            // Arrange
            const bool expected = true;
            ConfigKey configKey = ConfigKey.SmtpEnableSSL;

            settingsReader.Setup(x => x.GetAppSettingValueAsBool(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SmtpEnableSSL;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region AuthenticationScheme

        [Test]
        public void AuthenticationScheme_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "AuthenticationScheme";
            ConfigKey configKey = ConfigKey.AuthenticationScheme;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.AuthenticationScheme;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SenderEmailAddress

        [Test]
        public void SenderEmailAddress_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "SenderEmailAddress";
            ConfigKey configKey = ConfigKey.SenderEmailAddress;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SenderEmailAddress;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region DefaultMailSenderName

        [Test]
        public void DefaultMailSenderName_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "DefaultMailSenderName";
            ConfigKey configKey = ConfigKey.DefaultMailSenderName;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.DefaultMailSenderName;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region MailTemplateRootPath

        [Test]
        public void MailTemplateRootPath_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "MailTemplateRootPath";
            ConfigKey configKey = ConfigKey.MailTemplateRootPath;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.MailTemplateRootPath;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region PdfFilesRootPath

        [Test]
        public void PdfFilesRootPath_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "PdfFilesRootPath";
            ConfigKey configKey = ConfigKey.PdfFilesRootPath;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.PdfFilesRootPath;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SmtpAddress

        [Test]
        public void SmtpAddress_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "SmtpAddress";
            ConfigKey configKey = ConfigKey.SmtpAddress;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SmtpAddress;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SmtpUsername

        [Test]
        public void SmtpUsername_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "SmtpUsername";
            ConfigKey configKey = ConfigKey.SmtpUsername;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SmtpUsername;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region SmtpPassword

        [Test]
        public void SmtpPassword_NoCondition_ReturnValue()
        {
            // Arrange
            const string expected = "SmtpPassword";
            ConfigKey configKey = ConfigKey.SmtpPassword;

            settingsReader.Setup(x => x.GetAppSettingValueAsString(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.SmtpPassword;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region FailedJobsCacheTimeInMinutes

        [Test]
        public void FailedJobsCacheTimeInMinutes_NoCondition_ReturnValue()
        {
            // Arrange
            const int expected = 5;
            ConfigKey configKey = ConfigKey.FailedJobsCacheTimeInMinutes;

            settingsReader.Setup(x => x.GetAppSettingValueAsInt(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.FailedJobsCacheTimeInMinutes;

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region JenkinsJobCountCacheTimeInMinutes

        [Test]
        public void JenkinsJobCountCacheTimeInMinutes_NoCondition_ReturnValue()
        {
            // Arrange
            const int expected = 5;
            ConfigKey configKey = ConfigKey.JenkinsJobCountCacheTimeInMinutes;

            settingsReader.Setup(x => x.GetAppSettingValueAsInt(configKey.ToString())).Returns(expected);

            // Act
            var result = settings.JenkinsJobCountCacheTimeInMinutes;

            // Assert
            result.Should().Be(expected);
        }

        #endregion
    }
}