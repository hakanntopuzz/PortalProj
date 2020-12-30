using System.ComponentModel;

namespace DevPortal.Web.Library.Model
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        [DisplayName("E-posta Adresi (*)")]
        public string EmailAddress { get; set; }
    }
}