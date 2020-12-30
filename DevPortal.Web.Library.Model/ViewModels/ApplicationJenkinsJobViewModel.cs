using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationJenkinsJobViewModel : AuthorizedBaseViewModel
    {
        public ICollection<JenkinsJobType> JenkinsJobTypeList { get; set; }

        public JenkinsJob ApplicationJenkinsJob { get; set; }

        public string JenkinsUrl { get; set; }

        public string JenkinsJobUrl { get; set; }
    }
}