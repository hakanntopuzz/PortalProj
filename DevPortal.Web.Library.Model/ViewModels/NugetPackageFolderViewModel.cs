using DevPortal.NugetManager.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class NugetPackageFolderViewModel
    {
        public List<NugetPackageFolder> NugetPackageFolders { get; set; }

        public bool IsAuthorized { get; set; }
    }
}