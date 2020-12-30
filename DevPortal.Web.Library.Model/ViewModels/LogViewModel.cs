using DevPortal.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevPortal.Web.Library.Model
{
    public class LogViewModel
    {
        public LogViewModel()
        {
            Folders = new Collection<LogFileModel>();
        }

        public string Text { get; set; }

        public Collection<LogFileModel> Folders { get; set; }

        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }

        public ICollection<Application> Applications { get; set; }

        public ICollection<ApplicationEnvironment> Environments { get; set; }

        public string LogFilePath { get; set; }

        public BreadCrumbViewModel BreadCrumbViewModel { get; set; }
    }
}