using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DeploymentPackageViewModel
    {
        public ICollection<DeploymentPackageFolder> Applications { get; set; }

        public BreadCrumbViewModel BreadCrumbViewModel { get; set; }
    }
}