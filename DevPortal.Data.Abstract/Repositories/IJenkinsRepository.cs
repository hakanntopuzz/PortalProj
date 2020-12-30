using DevPortal.JenkinsManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IJenkinsRepository
    {
        Task<IEnumerable<JenkinsJobItem>> GetJobs(Uri uri);

        IEnumerable<JenkinsJobItem> GetFailedJobs(Uri uri);

        Task<JenkinsJobDetail> GetJobDetail(Uri uri);
    }
}