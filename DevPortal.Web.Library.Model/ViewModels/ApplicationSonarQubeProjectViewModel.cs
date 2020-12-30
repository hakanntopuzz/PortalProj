using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationSonarQubeProjectViewModel : AuthorizedBaseViewModel
    {
        public ICollection<SonarQubeProjectType> SonarQubeProjectTypeList { get; set; }

        public SonarqubeProject ApplicationSonarQubeProject { get; set; }

        public string SonarQubeProjectUrl { get; set; }
    }
}