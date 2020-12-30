using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DatabaseViewModel : AuthorizedBaseViewModel
    {
        public Database Database { get; set; }

        public ICollection<DatabaseType> DatabaseTypes { get; set; }

        public ICollection<DatabaseGroup> DatabaseGroups { get; set; }
    }
}