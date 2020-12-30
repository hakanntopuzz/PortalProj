using System.ComponentModel;

namespace DevPortal.Model
{
    public class ApplicationDependency : Record
    {
        public int Id { get; set; }

        public int DependedApplicationId { get; set; }

        public string Name { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationGroupName { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public int ApplicationGroupId { get; set; }

        public int DependentApplicationId { get; set; }

        public string RedmineProjectName { get; set; }
    }
}