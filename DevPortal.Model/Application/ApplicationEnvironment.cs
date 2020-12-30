using System.ComponentModel;

namespace DevPortal.Model
{
    public class ApplicationEnvironment : Record
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        [DisplayName("Ortam")]
        public int EnvironmentId { get; set; }

        [DisplayName("Log Görüntüleme Durumu")]
        public bool HasLog { get; set; }

        [DisplayName("Url")]
        public string Url { get; set; }

        [DisplayName("Fiziksel Yol (*)")]
        public string PhysicalPath { get; set; }

        [DisplayName("Log Dizini Yolu")]
        public string LogFilePath { get; set; }

        [DisplayName("Ortam")]
        public string EnvironmentName { get; set; }

        public string HasLogText
        {
            get
            {
                return HasLog ? ConstStrings.Viewable : ConstStrings.NonViewable;
            }
        }
    }
}