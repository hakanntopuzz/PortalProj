using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class EnvironmentViewModel : AuthorizedBaseViewModel
    {
        public ICollection<Environment> Environments { get; set; }

        public Environment Environment { get; set; }

        public bool HasEnvironments
        {
            get
            {
                return Environments != null && Environments.Count > 0;
            }
        }
    }
}