using DevPortal.NugetManager.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class NugetPackageViewModel
    {
        public List<NugetPackage> NugetPackages { get; set; }

        public bool IsAuthorized { get; set; }
    }
}