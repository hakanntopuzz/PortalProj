namespace DevPortal.Web.Library.Model
{
    public class DependenciesExportControllerActionNames : BaseActionNames
    {
        public static string ExportApplicationDependenciesCsv => SetActionName(nameof(ExportApplicationDependenciesCsv));

        public static string ExportDatabaseDependenciesCsv => SetActionName(nameof(ExportDatabaseDependenciesCsv));

        public static string ExportExternalDependenciesCsv => SetActionName(nameof(ExportExternalDependenciesCsv));

        public static string ExportNugetPackageDependenciesCsv => SetActionName(nameof(ExportNugetPackageDependenciesCsv));

        public static string ExportDependenciesAsWiki => SetActionName(nameof(ExportDependenciesAsWiki));

        public static string ExportDependenciesAsWikiFile => SetActionName(nameof(ExportDependenciesAsWikiFile));
    }
}