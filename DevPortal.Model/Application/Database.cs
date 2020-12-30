using System;
using System.ComponentModel;

namespace DevPortal.Model
{
    public class Database : Record
    {
        public int Id { get; set; }

        [DisplayName("Adı (*)")]
        public string Name { get; set; }

        public string DatabaseGroupName { get; set; }

        public string DatabaseTypeName { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int TotalCount { get; set; }

        public string RedmineProjectUrl { get; set; }

        [DisplayName("Redmine Proje Adı")]
        public string RedmineProjectName { get; set; }

        [DisplayName("Veritabanı Tipi (*)")]
        public int DatabaseTypeId { get; set; }

        [DisplayName("Veritabanı Grubu (*)")]
        public int DatabaseGroupId { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}