using System;
using System.ComponentModel;

namespace DevPortal.Model
{
    public class UserModel
    {
        public int Id { get; set; }

        [DisplayName("Ad (*)")]
        public string FirstName { get; set; }

        [DisplayName("Soyad (*)")]
        public string LastName { get; set; }

        public string SecureId { get; set; }

        [DisplayName("Svn Kullanıcı Adı (*)")]
        public string SvnUserName { get; set; }

        [DisplayName("E-posta Adresi (*)")]
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public int UserStatusId { get; set; }

        [DisplayName("Durum")]
        public string UserStatus { get; set; }

        [DisplayName("Kullanıcı Tipi")]
        public int UserTypeId { get; set; }

        public string UserType { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [DisplayName("Sil")]
        public bool IsDeleted { get; set; }

        public int TotalCount { get; set; }
    }
}