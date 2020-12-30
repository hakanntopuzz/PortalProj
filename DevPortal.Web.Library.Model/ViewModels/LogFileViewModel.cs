using DevPortal.Model;

namespace DevPortal.Web.Library.Model
{
    public class LogFileViewModel
    {
        public LogFileModel LogFile { get; set; }

        public bool IsLogFile
        {
            get
            {
                return LogFile != null;
            }
        }

        public bool HasText
        {
            get
            {
                return LogFile.Text != null && !string.IsNullOrWhiteSpace(LogFile.Text);
            }
        }

        public BreadCrumbViewModel BreadCrumbViewModel { get; set; }
    }
}