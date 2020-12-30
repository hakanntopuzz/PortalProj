using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract.Services
{
    public interface IApplicationJenkinsJobService
    {
        ICollection<JenkinsJobType> GetJenkinsJobTypes();

        ServiceResult AddApplicationJenkinsJob(JenkinsJob applicationJenkinsJob);

        JenkinsJob GetApplicationJenkinsJob(int jenkinsJobId);

        ServiceResult UpdateApplicationJenkinsJob(JenkinsJob applicationJenkinsJob);

        ServiceResult DeleteApplicationJenkinsJob(int jenkinsJobId);
    }
}