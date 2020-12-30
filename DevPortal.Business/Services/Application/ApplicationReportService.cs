using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;

namespace DevPortal.Business.Services
{
    public class ApplicationReportService : IApplicationReportService
    {
        #region ctor

        readonly IApplicationRepository applicationRepository;

        public ApplicationReportService(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        #endregion

        #region protected virtuals

        protected virtual int GetJenkinsJobCountCore()
        {
            return applicationRepository.GetJenkinsJobCount();
        }

        #endregion

        public ApplicationStats GetApplicationStats()
        {
            return new ApplicationStats
            {
                ApplicationCount = applicationRepository.GetApplicationCount(),
                ApplicationGroupCount = applicationRepository.GetApplicationGroupCount(),
                JenkinsJobCount = GetJenkinsJobCountCore(),
                SvnRepositoryCount = applicationRepository.GetSvnRepositoryCount(),
                SonarQubeProjectCount = applicationRepository.GetSonarQubeProjectCount(),
                ApplicationRedmineProjectCount = applicationRepository.GetApplicationRedmineProjectCount(),
                DatabaseRedmineProjectCount = applicationRepository.GetDatabaseRedmineProjectCount(),
                ApplicationCountsByType = applicationRepository.GetApplicationCountByType(),
                JenkinsJobCountsByType = applicationRepository.GetJenkinsJobCountByType(),
                SonarQubeProjectCountsByType = applicationRepository.GetSonarQubeProjectCountByType(),
                NugetPackageCount = applicationRepository.GetNugetPackageCount()
            };
        }
    }
}