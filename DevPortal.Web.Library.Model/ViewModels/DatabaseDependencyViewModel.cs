using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DatabaseDependencyViewModel : AuthorizedBaseViewModel
    {
        public DatabaseDependency DatabaseDependency { get; set; }

        public ICollection<Database> Databases { get; set; }

        public ICollection<DatabaseGroup> DatabaseGroups { get; set; }
    }
}