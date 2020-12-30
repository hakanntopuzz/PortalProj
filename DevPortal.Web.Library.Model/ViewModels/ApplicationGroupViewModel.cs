using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationGroupViewModel : AuthorizedBaseViewModel
    {
        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }

        public ApplicationGroup ApplicationGroup { get; set; }

        public ICollection<ApplicationGroupStatus> Status { get; set; }

        public ICollection<ApplicationListItem> ApplicationList { get; set; }

        public bool HasApplicationGroup
        {
            get
            {
                return ApplicationGroups != null && ApplicationGroups.Count > 0;
            }
        }

        public bool HasApplications
        {
            get
            {
                return ApplicationList != null && ApplicationList.Count > 0;
            }
        }
    }
}