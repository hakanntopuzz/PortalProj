using System.ComponentModel;

namespace DevPortal.Model
{
    public class ExternalDependency : Record
    {
        public int Id { get; set; }

        [DisplayName("Bağımlılık Adı (*)")]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }
    }
}