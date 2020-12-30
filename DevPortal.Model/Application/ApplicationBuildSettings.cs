using System.ComponentModel;

namespace DevPortal.Model
{
    public class ApplicationBuildSettings : Record
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        [DisplayName("Çalışma Alanı (*)")]
        public string Workspace { get; set; }

        [DisplayName("Solüsyon Adı (*)")]
        public string SolutionName { get; set; }

        [DisplayName("Proje Adı (*)")]
        public string ProjectName { get; set; }

        [DisplayName("Kurulum Yolu (*)")]
        public string DeployPath { get; set; }

        [DisplayName("Dev Yayınlama Profil Adı (*)")]
        public string DevPublishProfileName { get; set; }

        [DisplayName("Test Yayınlama Profil Adı (*)")]
        public string TestPublishProfileName { get; set; }

        [DisplayName("PreProd Yayınlama Profil Adı (*)")]
        public string PreProdPublishProfileName { get; set; }

        [DisplayName("Prod Yayınlama Profil Adı (*)")]
        public string ProdPublishProfileName { get; set; }

        [DisplayName("Dev Dizin Adresi (*)")]
        public string DevRemoteAddress { get; set; }

        [DisplayName("Test Dizin Adresi (*)")]
        public string TestRemoteAddress { get; set; }

        [DisplayName("PreProd Dizin Adresi (*)")]
        public string PreProdRemoteAddress { get; set; }

        [DisplayName("Prod Dizin Adresi (*)")]
        public string ProdRemoteAddress { get; set; }
    }
}