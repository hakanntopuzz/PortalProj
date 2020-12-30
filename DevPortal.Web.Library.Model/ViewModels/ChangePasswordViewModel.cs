using System.ComponentModel;

namespace DevPortal.Web.Library.Model
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        [DisplayName("Mevcut Parola (*)")]
        public string Password { get; set; }

        [DisplayName("Yeni Parola (*)")]
        public string NewPassword { get; set; }

        [DisplayName("Yeni Parola Tekrar (*)")]
        public string ConfirmPassword { get; set; }
    }
}