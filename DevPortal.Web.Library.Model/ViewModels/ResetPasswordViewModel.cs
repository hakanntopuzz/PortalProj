using System.ComponentModel;

namespace DevPortal.Web.Library.Model
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        [DisplayName("Yeni Parola (*)")]
        public string NewPassword { get; set; }

        [DisplayName("Yeni Parola Tekrar (*)")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}