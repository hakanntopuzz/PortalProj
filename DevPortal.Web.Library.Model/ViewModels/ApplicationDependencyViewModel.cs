using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationDependencyViewModel : AuthorizedBaseViewModel
    {
        public ApplicationDependency ApplicationDependency { get; set; }

        public ICollection<ApplicationListItem> Applications { get; set; }

        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }
    }
}