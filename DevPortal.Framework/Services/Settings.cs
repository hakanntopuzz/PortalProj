using AB.Framework.SettingsReader.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;

namespace DevPortal.Framework.Services
{
    public class Settings : ISettings
    {
        #region ctor

        readonly ISettingsReader settingsReader;

        public Settings(ISettingsReader settingsReader)
        {
            this.settingsReader = settingsReader;
        }

        #endregion

        #region private helpers

        string GetConnectionString(ConfigKey key)
        {
            return settingsReader.GetConnectionString(key.ToString());
        }

        string GetAppSettingValueAsString(ConfigKey key)
        {
            return settingsReader.GetAppSettingValueAsString(key.ToString());
        }

        int GetAppSettingValueAsInt(ConfigKey key)
        {
            return settingsReader.GetAppSettingValueAsInt(key.ToString());
        }

        bool GetAppSettingValueAsBool(ConfigKey key)
        {
            return settingsReader.GetAppSettingValueAsBool(key.ToString());
        }

        #endregion

        public string SiteUrl => GetAppSettingValueAsString(ConfigKey.SiteUrl);

        public string DevPortalDbConnectionString => GetConnectionString(ConfigKey.DevPortalDbConnectionString);

        public string ApplicationVersion => GetAppSettingValueAsString(ConfigKey.ApplicationVersion);

        public string RegPathByAes => GetAppSettingValueAsString(ConfigKey.RegPathByAes);

        public string RegPathByTripleDes => GetAppSettingValueAsString(ConfigKey.RegPathByTripleDes);

        public string OldNugetPackagesPhysicalPath => GetAppSettingValueAsString(ConfigKey.OldNugetPackagesPhysicalPath);

        public int NugetPackageListCacheTimeInMinutes => GetAppSettingValueAsInt(ConfigKey.NugetPackageListCacheTimeInMinutes);

        public int NumberOfNugetPackagePaging => GetAppSettingValueAsInt(ConfigKey.NumberOfNugetPackagePaging);

        public string SshServerAddress => GetAppSettingValueAsString(ConfigKey.SshServerAddress);

        public string SshUsername => GetAppSettingValueAsString(ConfigKey.SshUsername);

        public string SshPrivateKeyFilePath => GetAppSettingValueAsString(ConfigKey.SshPrivateKeyFilePath);

        public string NugetPackagesPhysicalPath => GetAppSettingValueAsString(ConfigKey.NugetPackagesPhysicalPath);

        public int JenkinsFailedJobCount => GetAppSettingValueAsInt(ConfigKey.JenkinsFailedJobCount);

        public string AuthenticationScheme => GetAppSettingValueAsString(ConfigKey.AuthenticationScheme);

        public string SmtpAddress => GetAppSettingValueAsString(ConfigKey.SmtpAddress);

        public string SmtpUsername => GetAppSettingValueAsString(ConfigKey.SmtpUsername);

        public string SmtpPassword => GetAppSettingValueAsString(ConfigKey.SmtpPassword);

        public int SmtpPortNumber => GetAppSettingValueAsInt(ConfigKey.SmtpPortNumber);

        public bool SmtpEnableSSL => GetAppSettingValueAsBool(ConfigKey.SmtpEnableSSL);

        public string SenderEmailAddress => GetAppSettingValueAsString(ConfigKey.SenderEmailAddress);

        public string DefaultMailSenderName => GetAppSettingValueAsString(ConfigKey.DefaultMailSenderName);

        public string MailTemplateRootPath => GetAppSettingValueAsString(ConfigKey.MailTemplateRootPath);

        public string PdfFilesRootPath => GetAppSettingValueAsString(ConfigKey.PdfFilesRootPath);

        public int FailedJobsCacheTimeInMinutes => GetAppSettingValueAsInt(ConfigKey.FailedJobsCacheTimeInMinutes);

        public int JenkinsJobCountCacheTimeInMinutes => GetAppSettingValueAsInt(ConfigKey.JenkinsJobCountCacheTimeInMinutes);

        public int FavouritePagesHomePageCount => GetAppSettingValueAsInt(ConfigKey.FavouritePagesHomePageCount);
        public string NugetPackageCreationFilesPath => GetAppSettingValueAsString(ConfigKey.NugetPackageCreationFilesPath);
    }
}