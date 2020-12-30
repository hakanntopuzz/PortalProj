namespace DevPortal.Framework
{
    public static class CacheKeyNames
    {
        public static string FailedJobsCacheKey => "CachedJenkinsService.FailedJobs";

        public static string JenkinsJobCountCacheKey => "CachedApplicationReportService.JenkinsJobCount";

        public static string NugetPackageList => nameof(NugetPackageList);
    }
}