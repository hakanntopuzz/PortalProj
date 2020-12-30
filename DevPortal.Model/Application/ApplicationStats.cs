using System.Collections.Generic;

namespace DevPortal.Model
{
    public class ApplicationStats
    {
        public int ApplicationCount { get; set; }

        public int ApplicationGroupCount { get; set; }

        public int JenkinsJobCount { get; set; }

        public int SvnRepositoryCount { get; set; }

        public int SvnServerRepositoryCount { get; set; }

        public int SonarQubeProjectCount { get; set; }

        public int ApplicationRedmineProjectCount { get; set; }

        public int DatabaseRedmineProjectCount { get; set; }

        public int NugetPackageCount { get; set; }

        public ICollection<ApplicationCountByTypeModel> ApplicationCountsByType { get; set; }

        public ICollection<JenkinsJobCountByTypeModel> JenkinsJobCountsByType { get; set; }

        public ICollection<SonarQubeProjectCountByTypeModel> SonarQubeProjectCountsByType { get; set; }
    }
}