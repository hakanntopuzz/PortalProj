using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationJenkinsJobViewModelFactory
    {
        #region application jenkins jobs

        ApplicationJenkinsJobViewModel CreateApplicationJenkinsJobViewModel(Application application, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl);

        ApplicationJenkinsJobViewModel CreateApplicationJenkinsJobViewModel(Application application, JenkinsJob jenkinsJob, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl);

        ApplicationJenkinsJobViewModel CreateDetailApplicationJenkinsJobViewModel(Application application, JenkinsJob jenkinsJob, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl);

        ApplicationJenkinsJobViewModel CreateEditApplicationJenkinsJobViewModel(Application application, JenkinsJob jenkinsJob, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl);

        #endregion
    }
}