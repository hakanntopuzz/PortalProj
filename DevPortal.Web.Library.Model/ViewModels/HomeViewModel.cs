using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using DevPortal.NugetManager.Model;
using DevPortal.SvnAdmin.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Web.Library.Model
{
    public class HomeViewModel : BaseViewModel
    {
        public ApplicationStats ApplicationStats { get; set; }

        public ICollection<ApplicationListItem> LastUpdatedApplications { get; set; }

        public SvnRepositoryListResult LastUpdatedSvnRepositories { get; set; }

        public int SvnRepositoryCount { get; set; }

        public string JenkinsUrl { get; set; }

        public string SonarQubeUrl { get; set; }

        public string RedmineUrl { get; set; }

        public ICollection<Link> RedmineProjects { get; set; }

        public ICollection<Link> RedmineWikiPages { get; set; }

        public ICollection<Link> ToolPages { get; set; }

        public IEnumerable<JenkinsJobItem> JenkinsFailedJobs { get; set; }

        public IEnumerable<LastUpdatedNugetPackageListItem> LastUpdatedNugetPackages { get; set; }

        public NugetPackagesStats NugetPackagesStats { get; set; }

        public IEnumerable<Link> FavouritePages { get; set; }

        public bool HasFavouritePages
        {
            get
            {
                return this.FavouritePages.Any();
            }
        }

        public bool HasToolPages
        {
            get
            {
                return this.ToolPages.Any();
            }
        }

        public HomeViewModel()
        {
            this.FavouritePages = new List<Link>();
            this.ToolPages = new List<Link>();
        }
    }
}