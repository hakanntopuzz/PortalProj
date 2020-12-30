using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Model
{
    public class DatabaseRedmineProjectsViewModel : AuthorizedBaseViewModel
    {
        public DatabaseRedmineProjectsViewModel()
        {
            DatabaseGroups = new List<DatabaseGroup>();
        }

        public ICollection<DatabaseGroup> DatabaseGroups { get; set; }

        public int? DatabaseGroupId { get; set; }
    }
}