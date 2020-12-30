using DevPortal.Model;
using System;

namespace DevPortal.Business.Abstract
{
    public interface IGeneralSettingsService
    {
        GeneralSettings GetGeneralSettings();

        ServiceResult UpdateGeneralSettings(GeneralSettings generalSettings);

        #region svn

        Uri GetSvnUrl();

        #endregion

        #region nuget

        Uri GetNugetServerUrl();

        string GetNugetPackageArchiveFolderPath();

        #endregion

        #region redmine

        Uri GetRedmineUrl();

        Uri GetRedmineProjectUrl(string redmineProjectName);

        #endregion

        #region sonarqube

        Uri GetSonarQubeUrl();

        Uri GetSonarqubeProjectUrl();

        #endregion

        #region jenkins

        Uri GetJenkinsUrl();

        Uri GetJenkinsJobUrl();

        Uri GetJenkinsJobUrl(string name);

        Uri GetJenkinsJobsUrl();

        Uri GetJenkinsFailedJobsUrl();

        #endregion

        #region application version packages

        string GetApplicationVersionPackageProdFolderPath();

        string GetApplicationVersionPackagePreProdFolderPath();

        #endregion

        #region database deployment packages

        string GetDatabaseDeploymentPackageProdFolderPath();

        string GetDatabaseDeploymentPackagePreProdFolderPath();

        #endregion
    }
}