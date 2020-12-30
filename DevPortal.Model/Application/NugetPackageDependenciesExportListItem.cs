using System;

namespace DevPortal.Model
{
    public class NugetPackageDependenciesExportListItem
    {
        public string Name { get; set; }

        public string PackageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUserEmail { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedUserEmail { get; set; }
    }
}