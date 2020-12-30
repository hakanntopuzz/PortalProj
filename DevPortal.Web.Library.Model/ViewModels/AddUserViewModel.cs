using DevPortal.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevPortal.Web.Library.Model
{
    public class AddUserViewModel : BaseViewModel
    {
        public User User { get; set; }

        [DisplayName("Parola (*)")]
        public string Password { get; set; }

        [DisplayName("Parola Tekrar (*)")]
        public string ConfirmPassword { get; set; }

        public ICollection<UserType> UserTypes { get; set; }

        public ICollection<UserStatus> UserStatus { get; set; }
    }
}