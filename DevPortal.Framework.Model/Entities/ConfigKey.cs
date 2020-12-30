namespace DevPortal.Framework.Model
{
    public enum ConfigKey
    {
        SiteUrl,

        ApplicationVersion,

        RegPathByTripleDes,

        RegPathByAes,

        DevPortalDbConnectionString,

        OldNugetPackagesPhysicalPath,

        NugetPackageListCacheTimeInMinutes,

        NumberOfNugetPackagePaging,

        /// <summary>
        /// SvnAdmin modülünün bağlanacağı Ssh sunucu adresi
        /// </summary>
        SshServerAddress,

        /// <summary>
        /// SvnAdmin modülünün svn sunucusunda komut çalıştırma esnasında kullanacağı kullanıcı adı
        /// </summary>
        SshUsername,

        /// <summary>
        /// SvnAdmin modülünün svn sunucusuna bağlanırken kullanacağı Rsa anahtar dosyasının konumu
        /// </summary>
        SshPrivateKeyFilePath,

        NugetPackagesPhysicalPath,

        JenkinsFailedJobCount,

        AuthenticationScheme,

        SmtpAddress,

        SmtpUsername,

        SmtpPassword,

        SmtpPortNumber,

        SmtpEnableSSL,

        SenderEmailAddress,

        DefaultMailSenderName,

        MailTemplateRootPath,

        PdfFilesRootPath,

        FailedJobsCacheTimeInMinutes,

        JenkinsJobCountCacheTimeInMinutes,

        FavouritePagesHomePageCount,

        NugetPackageCreationFilesPath
    }
}