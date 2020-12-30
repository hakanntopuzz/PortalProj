namespace DevPortal.Web.Library.Model
{
    public static class ControllerNames
    {
        static string SetControllerName(string controllerName)
        {
            return controllerName.ToLowerInvariant();
        }

        public static string Home => SetControllerName(nameof(Home));

        public static string Site => SetControllerName(nameof(Site));

        public static string Security => SetControllerName(nameof(Security));

        public static string Log => SetControllerName(nameof(Log));

        public static string Application => SetControllerName(nameof(Application));

        public static string ApplicationGroup => SetControllerName(nameof(ApplicationGroup));

        public static string ApplicationEnvironment => SetControllerName(nameof(ApplicationEnvironment));

        public static string ApplicationJenkinsJob => SetControllerName(nameof(ApplicationJenkinsJob));

        public static string GeneralSettings => SetControllerName(nameof(GeneralSettings));

        public static string ApplicationSvn => SetControllerName(nameof(ApplicationSvn));

        public static string SvnAdmin => SetControllerName(nameof(SvnAdmin));

        public static string SvnAdminExport => SetControllerName(nameof(SvnAdminExport));

        public static string ApplicationSonarqubeProject => SetControllerName(nameof(ApplicationSonarqubeProject));

        public static string Error => SetControllerName(nameof(Error));

        public static string Menu => SetControllerName(nameof(Menu));

        public static string Export => SetControllerName(nameof(Export));

        public static string User => SetControllerName(nameof(User));

        public static string Account => SetControllerName(nameof(Account));

        public static string Nuget => SetControllerName(nameof(Nuget));

        public static string Jenkins => SetControllerName(nameof(Jenkins));

        public static string ApplicationNugetPackage => SetControllerName(nameof(ApplicationNugetPackage));

        public static string Samples => SetControllerName(nameof(Samples));

        public static string Environment => SetControllerName(nameof(Environment));

        public static string ApplicationCsvExport => SetControllerName(nameof(ApplicationCsvExport));

        public static string ApplicationPdfExport => SetControllerName(nameof(ApplicationPdfExport));

        public static string ApplicationWikiExport => SetControllerName(nameof(ApplicationWikiExport));

        public static string DatabaseType => SetControllerName(nameof(DatabaseType));

        public static string Database => SetControllerName(nameof(Database));

        public static string DatabaseGroup => SetControllerName(nameof(DatabaseGroup));

        public static string ExternalDependency => SetControllerName(nameof(ExternalDependency));

        public static string DatabaseExport => SetControllerName(nameof(DatabaseExport));

        public static string DependencyList => SetControllerName(nameof(DependencyList));

        public static string DependenciesExport => SetControllerName(nameof(DependenciesExport));

        public static string DatabaseDependency => SetControllerName(nameof(DatabaseDependency));

        public static string ApplicationDependency => SetControllerName(nameof(ApplicationDependency));

        public static string NugetPackageDependency => SetControllerName(nameof(NugetPackageDependency));

        public static string Favourites => SetControllerName(nameof(Favourites));

        public static string Redmine => SetControllerName(nameof(Redmine));

        public static string ApplicationBuildSettings => SetControllerName(nameof(ApplicationBuildSettings));

        public static string DeploymentPackage => SetControllerName(nameof(DeploymentPackage));
    }
}