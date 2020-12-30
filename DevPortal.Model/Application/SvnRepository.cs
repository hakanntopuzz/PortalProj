using System.ComponentModel;

namespace DevPortal.Model
{
    public class SvnRepository : Record
    {
        public int Id { get; set; }

        [DisplayName("Depo Adı (*)")]
        public string Name { get; set; }

        [DisplayName("Depo Tipi (*)")]
        public int SvnRepositoryTypeId { get; set; }

        public string SvnRepositoryTypeName { get; set; }

        public int ApplicationId { get; set; }

        public string SvnUrl { get; set; }

        public string ApplicationName { get; set; }

        [DisplayName("Depo Url")]

        //TODO: Bu özellik ve SvnUrl özelliğinin kullanım ayrımları belirlenmeli. Karışıklığa neden oluyor ve hatalı veri oluşturabiliyor.
        public string SvnRepositoryUrl
        {
            get
            {
                return string.Concat(SvnUrl, Name);
            }
        }
    }
}