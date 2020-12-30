using System.ComponentModel;

namespace DevPortal.Model
{
    public class GeneralSettingsEditModel
    {
        public int Id { get; set; }

        [DisplayName("Redmine Url")]
        public string RedmineUrl { get; set; }

        [DisplayName("Svn Url")]
        public string SvnUrl { get; set; }

        [DisplayName("Jenkins Url")]
        public string JenkinsUrl { get; set; }

        [DisplayName("SonarQube Url")]
        public string SonarQubeUrl { get; set; }

        [DisplayName("ActiveBuilder NuGet Url")]
        public string NugetUrl { get; set; }

        [DisplayName("ActiveBuilder Nuget ApiKey")]
        public string NugetApiKey { get; set; }

        [DisplayName("ActiveBuilder Nuget Paket Arşivi Dizini")]
        public string NugetPackageArchiveFolderPath { get; set; }

        [DisplayName("Uygulama Başarılı Versiyon Paketleri Prod Dizini")]
        public string ApplicationVersionPackageProdFolderPath { get; set; }

        [DisplayName("Uygulama Başarılı Versiyon Paketleri PreProd Dizini")]
        public string ApplicationVersionPackagePreProdFolderPath { get; set; }

        [DisplayName("Veritabanı Aktarım Paketleri Prod Dizini")]
        public string DatabaseDeploymentPackageProdFolderPath { get; set; }

        [DisplayName("Veritabanı Aktarım Paketleri PreProd Dizini")]
        public string DatabaseDeploymentPackagePreProdFolderPath { get; set; }
    }
}