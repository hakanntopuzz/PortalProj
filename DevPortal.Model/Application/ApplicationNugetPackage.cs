using System.ComponentModel;

namespace DevPortal.Model
{
    public class ApplicationNugetPackage : Record
    {
        public int NugetPackageId { get; set; }

        [DisplayName("Paket Adı (*)")]
        public string NugetPackageName { get; set; }

        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        [DisplayName("Paket Url")]
        public string PackageUrl { get; set; }
    }
}