namespace DevPortal.Web.Library.Model
{
    public class DependencyListControllerActionNames : BaseActionNames
    {
        public static string GetApplicationDependenciesByApplicationId => SetActionName(nameof(GetApplicationDependenciesByApplicationId));

        public static string GetDatabaseDependenciesByApplicationId => SetActionName(nameof(GetDatabaseDependenciesByApplicationId));

        public static string GetExternalDependenciesByApplicationId => SetActionName(nameof(GetExternalDependenciesByApplicationId));

        public static string GetNugetPackageDependenciesByApplicationId => SetActionName(nameof(GetNugetPackageDependenciesByApplicationId));
    }
}