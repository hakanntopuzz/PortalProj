namespace DevPortal.Framework.Abstract
{
    public interface ISettings
    {
        string SiteUrl { get; }

        string ApplicationVersion { get; }

        string RegPathByTripleDes { get; }

        string RegPathByAes { get; }

        string DevPortalDbConnectionString { get; }

        string OldNugetPackagesPhysicalPath { get; }

        int NugetPackageListCacheTimeInMinutes { get; }

        int NumberOfNugetPackagePaging { get; }

        string SshServerAddress { get; }

        string SshUsername { get; }

        string SshPrivateKeyFilePath { get; }

        string NugetPackagesPhysicalPath { get; }

        int JenkinsFailedJobCount { get; }

        string AuthenticationScheme { get; }

        int FailedJobsCacheTimeInMinutes { get; }

        int JenkinsJobCountCacheTimeInMinutes { get; }

        int FavouritePagesHomePageCount { get; }

        string NugetPackageCreationFilesPath { get; }

        #region Email

        string SmtpAddress { get; }

        string SmtpUsername { get; }

        string SmtpPassword { get; }

        int SmtpPortNumber { get; }

        bool SmtpEnableSSL { get; }

        string SenderEmailAddress { get; }

        string DefaultMailSenderName { get; }

        string MailTemplateRootPath { get; }

        string PdfFilesRootPath { get; }

        #endregion
    }
}