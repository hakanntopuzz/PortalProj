using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationsViewModel : AuthorizedBaseViewModel
    {
        public ApplicationsViewModel()
        {
            Applications = new List<ApplicationListItem>();
        }

        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }

        public ICollection<ApplicationListItem> Applications { get; set; }

        public int? ApplicationGroupId { get; set; }

        public string ApplicationName { get; set; }

        public bool HasApplication
        {
            get
            {
                return Applications != null && Applications.Count > 0;
            }
        }
    }
}