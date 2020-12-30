using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationRedmineProjectsViewModel : AuthorizedBaseViewModel
    {
        public ApplicationRedmineProjectsViewModel()
        {
            ApplicationGroups = new List<ApplicationGroup>();
        }

        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }

        public int? ApplicationGroupId { get; set; }
    }
}