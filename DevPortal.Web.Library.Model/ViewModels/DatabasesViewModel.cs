using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DatabasesViewModel : AuthorizedBaseViewModel
    {
        public DatabasesViewModel()
        {
            Databases = new List<Database>();
        }

        public ICollection<DatabaseGroup> DatabaseGroups { get; set; }

        public ICollection<Database> Databases { get; set; }

        public int? DatabaseGroupId { get; set; }

        public string DatabaseName { get; set; }

        public bool HasDatabase
        {
            get
            {
                return Databases != null && Databases.Count > 0;
            }
        }
    }
}