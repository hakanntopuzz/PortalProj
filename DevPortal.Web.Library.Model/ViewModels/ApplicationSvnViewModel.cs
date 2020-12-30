using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationSvnViewModel : AuthorizedBaseViewModel
    {
        public SvnRepository ApplicationSvn { get; set; }

        public ICollection<SvnRepositoryType> SvnRepositoryTypeList { get; set; }
    }
}