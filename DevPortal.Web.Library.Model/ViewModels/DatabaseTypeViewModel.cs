using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DatabaseTypeViewModel : AuthorizedBaseViewModel
    {
        public ICollection<DatabaseType> DatabaseTypes { get; set; }

        public ICollection<Database> Databases { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public bool HasDatabaseTypes
        {
            get
            {
                return DatabaseTypes != null && DatabaseTypes.Count > 0;
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