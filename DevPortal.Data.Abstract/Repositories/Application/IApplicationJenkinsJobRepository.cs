using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationJenkinsJobRepository
    {
        ICollection<JenkinsJobType> GetJenkinsJobTypes();

        bool AddApplicationJenkinsJob(JenkinsJob applicationJenkinsJob);

        JenkinsJob GetApplicationJenkinsJobById(int jenkinsJobId);

        bool UpdateApplicationJenkinsJob(JenkinsJob jenkinsJob);

        bool DeleteApplicationJenkinsJob(int jenkinsJobId);

        RecordUpdateInfo GetApplicationJenkinsJobUpdateInfo(int jenkinsJobId);
    }
}