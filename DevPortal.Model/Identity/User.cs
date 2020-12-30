using System.ComponentModel;

namespace DevPortal.Model
{
    public class User : Record
    {
        public int Id { get; set; }

        public string SecureId { get; set; }

        [DisplayName("Adı (*)")]
        public string FirstName { get; set; }

        [DisplayName("Soyadı (*)")]
        public string LastName { get; set; }

        [DisplayName("SVN Kullanıcı Adı")]
        public string SvnUserName { get; set; }

        [DisplayName("E-posta Adresi (*)")]
        public string EmailAddress { get; set; }

        public string PasswordHash { get; set; }

        [DisplayName("Kullanıcı Tipi")]
        public int UserTypeId { get; set; }

        [DisplayName("Durum")]
        public int UserStatusId { get; set; }

        public int TotalCount { get; set; }

        public string UserStatus { get; set; }

        public string UserType { get; set; }

        public string DisplayName
        {
            get
            {
                return $"{this.FirstName[0]}{this.LastName[0]}";
            }
        }
    }
}