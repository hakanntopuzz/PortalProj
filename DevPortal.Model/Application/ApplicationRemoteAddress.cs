using System.ComponentModel;

namespace DevPortal.Model
{
    public class ApplicationRemoteAddress
    {
        [DisplayName("Dev Dizin Adresi (*)")]
        public string Dev { get; set; }

        [DisplayName("Test Dizin Adresi (*)")]
        public string Test { get; set; }

        [DisplayName("PreProd Dizin Adresi (*)")]
        public string PreProd { get; set; }

        [DisplayName("Prod Dizin Adresi (*)")]
        public string Prod { get; set; }
    }
}