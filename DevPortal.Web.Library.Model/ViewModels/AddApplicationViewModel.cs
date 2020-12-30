using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class AddApplicationViewModel : BaseViewModel
    {
        public Application Application { get; set; }

        public ICollection<ApplicationGroup> ApplicationGroups { get; set; }

        public ICollection<ApplicationType> ApplicationTypes { get; set; }

        public ICollection<ApplicationStatus> ApplicationStatus { get; set; }
    }
}