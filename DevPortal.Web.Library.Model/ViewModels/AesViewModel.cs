using DevPortal.Cryptography.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class AesViewModel : CryptoModel
    {
        public AesViewModel()
        {
            CryptoApplications = new List<CryptographyClientApplication>();
        }

        public int SiteId { get; set; }

        public List<CryptographyClientApplication> CryptoApplications { get; set; }
    }
}