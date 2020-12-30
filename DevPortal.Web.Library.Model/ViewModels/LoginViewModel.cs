using System.ComponentModel;

namespace DevPortal.Web.Library.Model
{
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        [DisplayName("Beni Hatırla")]
        public bool IsRemember { get; set; }

        public string ReturnUrl { get; set; }
    }
}