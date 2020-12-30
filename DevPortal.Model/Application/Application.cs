namespace DevPortal.Model
{
    public class Application : BaseApplication
    {
        public string Status { get; set; }

        public string RedmineProjectUrl { get; set; }

        public string ApplicationGroupName { get; set; }

        public string ApplicationType { get; set; }

        public int TotalCount { get; set; }
    }
}