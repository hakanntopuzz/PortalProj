using System.ComponentModel;

namespace DevPortal.Model
{
    public class JenkinsJob : Record
    {
        public int JenkinsJobId { get; set; }

        [DisplayName("Görev Adı (*)")]
        public string JenkinsJobName { get; set; }

        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        [DisplayName("Görev Tipi (*)")]
        public int JenkinsJobTypeId { get; set; }

        public string JenkinsJobTypeName { get; set; }

        [DisplayName("Görev Url")]
        public string JobUrl { get; set; }
    }
}