using DevPortal.Cryptography.Model;

namespace DevPortal.Web.Library.Model
{
    public class HashViewModel : BaseViewModel
    {
        public string HashToText { get; set; }

        public string HashedText { get; set; }

        public HashTypes HashType { get; set; }

        public bool HashedTextHasValue
        {
            get
            {
                return !string.IsNullOrEmpty(HashedText);
            }
        }
    }
}