using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DatabaseGroupViewModel : AuthorizedBaseViewModel
    {
        public ICollection<DatabaseGroup> DatabaseGroups { get; set; }

        public ICollection<Database> Databases { get; set; }

        public DatabaseGroup DatabaseGroup { get; set; }

        public bool HasDatabaseGroups
        {
            get
            {
                return DatabaseGroups != null && DatabaseGroups.Count > 0;
            }
        }

        public bool HasDatabases
        {
            get
            {
                return Databases != null && Databases.Count > 0;
            }
        }
    }
}