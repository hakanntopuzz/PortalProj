using System;

namespace DevPortal.Model
{
    public class ApplicationListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ApplicationGroupName { get; set; }

        public string ApplicationTypeName { get; set; }

        public string Status { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string RedmineProjectName { get; set; }
    }
}