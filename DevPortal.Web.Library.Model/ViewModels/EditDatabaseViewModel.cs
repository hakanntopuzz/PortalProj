using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class EditDatabaseViewModel : BaseViewModel
    {
        public Database Database { get; set; }

        public ICollection<DatabaseGroup> DatabaseGroups { get; set; }

        public ICollection<DatabaseType> DatabaseTypes { get; set; }
    }
}