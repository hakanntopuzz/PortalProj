using System.ComponentModel;

namespace DevPortal.Model
{
    public abstract class BaseMenu : Record
    {
        public int Id { get; set; }

        [DisplayName("Üst Menü")]
        public int? ParentId { get; set; }

        [DisplayName("Ad (*)")]
        public string Name { get; set; }

        public string Link { get; set; }

        [DisplayName("Sıra")]
        public int? Order { get; set; }

        [DisplayName("Menü Grubu")]
        public int? MenuGroupId { get; set; }

        [DisplayName("Simge")]
        public string Icon { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}