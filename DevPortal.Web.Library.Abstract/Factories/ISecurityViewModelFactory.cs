using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface ISecurityViewModelFactory
    {
        CryptoViewModel CreateCryptoViewModel();

        GeneratePasswordViewModel CreateGeneratePasswordViewModel(string password = null);

        AesViewModel CreateAesViewModel(string convertedText = null);

        HashViewModel CreateHashViewModel(string hashedText = null);

        GuidViewModel CreateGuidViewModel(string guid = null);
    }
}