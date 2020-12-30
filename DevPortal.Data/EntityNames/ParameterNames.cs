namespace DevPortal.Data
{
    public static class ParameterNames
    {
        public static string Id => $"@{nameof(Id)}";

        public static string ApplicationId => $"@{nameof(ApplicationId)}";

        public static string ApplicationGroupId => $"@{nameof(ApplicationGroupId)}";

        public static string ApplicationTypeId => $"@{nameof(ApplicationTypeId)}";

        public static string ApplicationGroupName => $"@{nameof(ApplicationGroupName)}";

        public static string Name => $"@{nameof(Name)}";

        public static string Description => $"@{nameof(Description)}";

        public static string StatusId => $"@{nameof(StatusId)}";

        public static string RedmineProjectName => $"@{nameof(RedmineProjectName)}";

        public static string RedmineUrl => $"@{nameof(RedmineUrl)}";

        public static string SvnUrl => $"@{nameof(SvnUrl)}";

        public static string JenkinsUrl => $"@{nameof(JenkinsUrl)}";

        public static string SonarQubeUrl => $"@{nameof(SonarQubeUrl)}";

        public static string NugetUrl => $"@{nameof(NugetUrl)}";

        public static string NugetApiKey => $"@{nameof(NugetApiKey)}";

        public static string NugetPackageArchiveFolderPath => $"@{nameof(NugetPackageArchiveFolderPath)}";

        public static string ApplicationVersionPackageProdFolderPath => $"@{nameof(ApplicationVersionPackageProdFolderPath)}";

        public static string ApplicationVersionPackagePreProdFolderPath => $"@{nameof(ApplicationVersionPackagePreProdFolderPath)}";

        public static string DatabaseDeploymentPackageProdFolderPath => $"@{nameof(DatabaseDeploymentPackageProdFolderPath)}";

        public static string DatabaseDeploymentPackagePreProdFolderPath => $"@{nameof(DatabaseDeploymentPackagePreProdFolderPath)}";

        public static string DisplayLength => $"@{nameof(DisplayLength)}";

        public static string DisplayStart => $"@{nameof(DisplayStart)}";

        public static string SortCol => $"@{nameof(SortCol)}";

        public static string SearchValue => $"@{nameof(SearchValue)}";

        public static string SortDir => $"@{nameof(SortDir)}";

        public static string ParentId => $"@{nameof(ParentId)}";

        public static string Link => $"@{nameof(Link)}";

        public static string Order => $"@{nameof(Order)}";

        public static string Group => $"@{nameof(Group)}";

        public static string Icon => $"@{nameof(Icon)}";

        public static string IsDeleted => $"@{nameof(IsDeleted)}";

        public static string TableName => $"@{nameof(TableName)}";

        public static string RecordId => $"@{nameof(RecordId)}";

        public static string FieldName => $"@{nameof(FieldName)}";

        public static string OldValue => $"@{nameof(OldValue)}";

        public static string NewValue => $"@{nameof(NewValue)}";

        #region application environment

        public static string EnvironmentId => $"@{nameof(EnvironmentId)}";

        public static string HasLog => $"@{nameof(HasLog)}";

        public static string Url => $"@{nameof(Url)}";

        public static string PhysicalPath => $"@{nameof(PhysicalPath)}";

        public static string LogFilePath => $"@{nameof(LogFilePath)}";

        public static string ApplicationEnvironmentId => $"@{nameof(ApplicationEnvironmentId)}";

        #endregion

        #region application jenkins job

        public static string JobTypeId => $"@{nameof(JobTypeId)}";

        public static string ApplicationJenkinsJobId => $"@{nameof(ApplicationJenkinsJobId)}";

        #endregion

        #region application svn repository

        public static string SvnRepositoryId => $"@{nameof(SvnRepositoryId)}";

        public static string RepositoryTypeId => $"@{nameof(RepositoryTypeId)}";

        #endregion

        #region sonarQube project

        public static string ProjectTypeId => $"@{nameof(ProjectTypeId)}";

        #endregion

        #region user

        public static string SecureId => $"@{nameof(SecureId)}";

        public static string FirstName => $"@{nameof(FirstName)}";

        public static string LastName => $"@{nameof(LastName)}";

        public static string SvnUserName => $"@{nameof(SvnUserName)}";

        public static string EmailAddress => $"@{nameof(EmailAddress)}";

        public static string PasswordHash => $"@{nameof(PasswordHash)}";

        public static string UserStatusId => $"@{nameof(UserStatusId)}";

        public static string UserTypeId => $"@{nameof(UserTypeId)}";

        public static string CreatedBy => $"@{nameof(CreatedBy)}";

        public static string ModifiedBy => $"@{nameof(ModifiedBy)}";

        #endregion

        #region userlogonlog

        public static string UserId => $"@{nameof(UserId)}";

        public static string IpAddress => $"@{nameof(IpAddress)}";

        public static string BrowserInfo => $"@{nameof(BrowserInfo)}";

        public static string BrowserName => $"@{nameof(BrowserName)}";

        public static string BrowserVersion => $"@{nameof(BrowserVersion)}";

        #endregion

        #region password reset request

        public static string RequestCode => $"@{nameof(RequestCode)}";

        #endregion

        public static string JenkinsJobName => $"@{nameof(JenkinsJobName)}";

        public static string NugetPackageName => $"@{nameof(NugetPackageName)}";

        public static string ExternalDependencyId => $"@{nameof(ExternalDependencyId)}";

        public static string DatabaseGroupId => $"@{nameof(DatabaseGroupId)}";

        public static string DatabaseId => $"@{nameof(DatabaseId)}";

        public static string DatabaseTypeId => $"@{nameof(DatabaseTypeId)}";

        public static string DependentApplicationId => $"@{nameof(DependentApplicationId)}";

        public static string DependedApplicationId => $"@{nameof(DependedApplicationId)}";

        public static string PageName => $"@{nameof(PageName)}";

        public static string PageUrl => $"@{nameof(PageUrl)}";

        public static string PageIdList => $"@{nameof(PageIdList)}";

        #region application build settings

        public static string Workspace => $"@{nameof(Workspace)}";

        public static string SolutionName => $"@{nameof(SolutionName)}";

        public static string ProjectName => $"@{nameof(ProjectName)}";

        public static string DeployPath => $"@{nameof(DeployPath)}";

        public static string DevPublishProfileName => $"@{nameof(DevPublishProfileName)}";

        public static string TestPublishProfileName => $"@{nameof(TestPublishProfileName)}";

        public static string PreProdPublishProfileName => $"@{nameof(PreProdPublishProfileName)}";

        public static string ProdPublishProfileName => $"@{nameof(ProdPublishProfileName)}";

        public static string DevRemoteAddress => $"@{nameof(DevRemoteAddress)}";

        public static string TestRemoteAddress => $"@{nameof(TestRemoteAddress)}";

        public static string PreProdRemoteAddress => $"@{nameof(PreProdRemoteAddress)}";

        public static string ProdRemoteAddress => $"@{nameof(ProdRemoteAddress)}";

        #endregion
    }
}