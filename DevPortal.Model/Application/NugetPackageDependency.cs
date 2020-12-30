namespace DevPortal.Model
{
    public class NugetPackageDependency : Record
    {
        public int Id { get; set; }

        public string NugetPackageName { get; set; }

        public string ApplicationName { get; set; }

        public int DependentApplicationId { get; set; }

        public string PackageUrl { get; set; }
    }
}