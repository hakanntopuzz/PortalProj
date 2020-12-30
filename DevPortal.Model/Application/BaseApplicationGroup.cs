using System.ComponentModel;

namespace DevPortal.Model
{
    public abstract class BaseApplicationGroup : Record
    {
        public int Id { get; set; }

        [DisplayName("Adı (*)")]
        public string Name { get; set; }

        [DisplayName("Durumu (*)")]
        public int StatusId { get; set; }
    }
}