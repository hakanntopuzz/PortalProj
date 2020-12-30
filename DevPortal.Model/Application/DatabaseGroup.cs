using System.ComponentModel;

namespace DevPortal.Model
{
    public class DatabaseGroup : Record
    {
        public int Id { get; set; }

        [DisplayName("Adı (*)")]
        public string Name { get; set; }

        public string DatabaseCount { get; set; }
    }
}