using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationEnvironmentViewModel : AuthorizedBaseViewModel
    {
        public ICollection<Environment> EnvironmentList { get; set; }

        public ApplicationEnvironment ApplicationEnvironment { get; set; }

        public bool HasEnvironmentList
        {
            get
            {
                return EnvironmentList != null && EnvironmentList.Count > 0;
            }
        }
    }
}