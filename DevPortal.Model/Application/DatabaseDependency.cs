using System.ComponentModel;

namespace DevPortal.Model
{
    public class DatabaseDependency : Record
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public int DatabaseId { get; set; }

        public int DependentApplicationId { get; set; }

        public int DatabaseGroupId { get; set; }

        [DisplayName("Veritabanı")]
        public string Name { get; set; }

        [DisplayName("Veritabanı Grubu")]
        public string DatabaseGroupName { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public string ApplicationName { get; set; }

        public string RedmineProjectName { get; set; }
    }
}