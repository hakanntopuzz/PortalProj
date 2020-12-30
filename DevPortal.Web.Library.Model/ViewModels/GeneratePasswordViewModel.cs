using DevPortal.Cryptography.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Web.Library.Model
{
    public class GeneratePasswordViewModel : BaseViewModel
    {
        public string Password { get; set; }

        public PasswordModel PasswordModel { get; set; }

        public Dictionary<string, int> PasswordLengthNumbers => Enumerable.Range(8, 25).ToDictionary(x => x.ToString(), x => x);

        public bool HasPasswordValue
        {
            get
            {
                return !string.IsNullOrEmpty(Password);
            }
        }
    }
}