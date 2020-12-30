namespace DevPortal.Web.Library.Model
{
    public class NugetControllerActionNames : BaseActionNames
    {
        public static string Packages => SetActionName(nameof(Packages));

        public static string GetFilteredNugetPackages => SetActionName(nameof(GetFilteredNugetPackages));

        public static string GetNugetPackageRootUrl => SetActionName(nameof(GetNugetPackageRootUrl));
    }
}