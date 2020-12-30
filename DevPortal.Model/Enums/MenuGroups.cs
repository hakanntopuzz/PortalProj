using System.ComponentModel.DataAnnotations;

namespace DevPortal.Model
{
    public enum MenuGroups
    {
        [Display(Name = "Ana Menü")]
        Navbar = 1,

        [Display(Name = "Kullanıcı Profili")]
        UserProfile = 2
    }
}