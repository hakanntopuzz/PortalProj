using System.ComponentModel;

namespace DevPortal.Model
{
    public class ApplicationPublishProfileName
    {
        [DisplayName("Dev Yayınlama Profil Adı (*)")]
        public string Dev { get; set; }

        [DisplayName("Test Yayınlama Profil Adı (*)")]
        public string Test { get; set; }

        [DisplayName("PreProd Yayınlama Profil Adı (*)")]
        public string PreProd { get; set; }

        [DisplayName("Prod Yayınlama Profil Adı (*)")]
        public string Prod { get; set; }
    }
}