using System.Collections.Generic;

namespace DevPortal.Model
{
    public class DependenciesExportListItem : Record
    {
        public DependenciesExportListItem()
        {
            this.ApplicationDependenciesExportListItem = new List<ApplicationDependenciesExportListItem>();
            this.DatabaseDependenciesExportListItem = new List<DatabaseDependenciesExportListItem>();
            this.ExternalDependenciesExportListItem = new List<ExternalDependenciesExportListItem>();
        }

        public ICollection<ApplicationDependenciesExportListItem> ApplicationDependenciesExportListItem { get; set; }

        public ICollection<DatabaseDependenciesExportListItem> DatabaseDependenciesExportListItem { get; set; }

        public ICollection<ExternalDependenciesExportListItem> ExternalDependenciesExportListItem { get; set; }
    }
}