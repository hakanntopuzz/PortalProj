using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class EditApplicationViewModel : BaseViewModel
    {
        public Application Application { get; set; }

        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }

        public ICollection<ApplicationType> ApplicationTypes { get; set; }

        public ICollection<ApplicationStatus> ApplicationStatusList { get; set; }
    }
}