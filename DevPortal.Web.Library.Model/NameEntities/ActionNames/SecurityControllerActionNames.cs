namespace DevPortal.Web.Library.Model
{
    public class SecurityControllerActionNames : BaseActionNames
    {
        public static string SingleCryptography => SetActionName(nameof(SingleCryptography));

        public static string MultipleEncryptWithAes256 => SetActionName(nameof(MultipleEncryptWithAes256));

        public static string MultipleDecryptWithAes256 => SetActionName(nameof(MultipleDecryptWithAes256));

        public static string MultipleEncryptWithTripleDes => SetActionName(nameof(MultipleEncryptWithTripleDes));

        public static string MultipleDecryptWithTripleDes => SetActionName(nameof(MultipleDecryptWithTripleDes));

        public static string Aes => SetActionName(nameof(Aes));

        public static string Hash => SetActionName(nameof(Hash));

        public static string Guid => SetActionName(nameof(Guid));

        public static string GenerateGuid => SetActionName(nameof(GenerateGuid));

        public static string Password => SetActionName(nameof(Password));
    }
}