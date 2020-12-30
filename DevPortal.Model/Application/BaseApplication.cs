using System.ComponentModel;

namespace DevPortal.Model
{
    public abstract class BaseApplication : Record
    {
        public int Id { get; set; }

        [DisplayName("Uygulama Adı (*)")]
        public string Name { get; set; }

        [DisplayName("Durum")]
        public int StatusId { get; set; }

        [DisplayName("Redmine Proje Adı")]
        public string RedmineProjectName { get; set; }

        [DisplayName("Uygulama Grubu (*)")]
        public int ApplicationGroupId { get; set; }

        [DisplayName("Uygulama Tipi")]
        public int ApplicationTypeId { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}