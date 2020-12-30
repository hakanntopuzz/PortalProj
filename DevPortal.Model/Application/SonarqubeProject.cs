using System.ComponentModel;

namespace DevPortal.Model
{
    public class SonarqubeProject : Record
    {
        public int SonarqubeProjectId { get; set; }

        [DisplayName("Proje Adı (*)")]
        public string SonarqubeProjectName { get; set; }

        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        [DisplayName("Proje Tipi (*)")]
        public int SonarqubeProjectTypeId { get; set; }

        public string SonarqubeProjectTypeName { get; set; }

        [DisplayName("Proje Url")]
        public string ProjectUrl { get; set; }
    }
}