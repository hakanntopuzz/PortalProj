using System.ComponentModel;

namespace DevPortal.Model
{
    public class Environment : Record
    {
        public int Id { get; set; }

        [DisplayName("Adı (*)")]
        public string Name { get; set; }
    }
}