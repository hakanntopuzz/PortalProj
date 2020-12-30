using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;

namespace DevPortal.Business.Services
{
    public class GeneralSettingsService : BaseService, IGeneralSettingsService
    {
        #region ctor

        readonly IGeneralSettingsRepository generalSettingsRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public GeneralSettingsService(
            IGeneralSettingsRepository generalSettingsRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.generalSettingsRepository = generalSettingsRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region get general settings

        public GeneralSettings GetGeneralSettings()
        {
            return generalSettingsRepository.GetGeneralSettings();
        }

        #endregion

        #region update general settings

        public ServiceResult UpdateGeneralSettings(GeneralSettings generalSettings)
        {
            var oldGeneralSettings = generalSettingsRepository.GetGeneralSettings();

            var isChanged = auditService.IsChanged(oldGeneralSettings, generalSettings, nameof(GeneralSettingsEditModel));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.GeneralSettingsUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateGeneralSettingsCore(generalSettings);

                    var newGeneralSettings = generalSettingsRepository.GetGeneralSettings();

                    AddAuditCore(oldGeneralSettings, newGeneralSettings, generalSettings.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(
                    SetMethodNameForLogMessage(nameof(UpdateGeneralSettings)),
                    Messages.GeneralSettingsUpdateErrorOccured,
                    ex);

                return ServiceResult.Error(Messages.GeneralSettingsUpdateErrorOccured);
            }

            return ServiceResult.Success(Messages.GeneralSettingsUpdated);
        }

        void AddAuditCore(GeneralSettings oldGeneralSettings, GeneralSettings generalSettings, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(GeneralSettings), oldGeneralSettings.Id, oldGeneralSettings, generalSettings, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Genel ayarlar güncellenirken Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void UpdateGeneralSettingsCore(GeneralSettings generalSettings)
        {
            var updateSuccess = generalSettingsRepository.UpdateGeneralSettings(generalSettings);

            if (!updateSuccess)
            {
                throw new TransactionIstopException();
            }
        }

        #endregion

        #region svn

        public Uri GetSvnUrl()
        {
            string url = GetGeneralSettings().SvnUrl;

            return new Uri(url);
        }

        #endregion

        #region nuget

        public Uri GetNugetServerUrl()
        {
            var settings = GetGeneralSettings();

            return new Uri(settings.NugetUrl);
        }

        public string GetNugetPackageArchiveFolderPath()
        {
            return GetGeneralSettings().NugetPackageArchiveFolderPath;
        }

        #endregion

        #region redmine

        public Uri GetRedmineUrl()
        {
            string url = GetGeneralSettings().RedmineUrl;

            return new Uri(url);
        }

        public Uri GetRedmineProjectUrl(string redmineProjectName)
        {
            var redmineUrl = GetRedmineUrl();

            return new Uri($"{redmineUrl}projects/{redmineProjectName}");
        }

        #endregion

        #region sonarqube

        public Uri GetSonarQubeUrl()
        {
            var url = GetGeneralSettings().SonarQubeUrl;

            return new Uri(url);
        }

        public Uri GetSonarqubeProjectUrl()
        {
            var sonarqubeUrl = GetSonarQubeUrl();

            return new Uri($"{sonarqubeUrl}dashboard/?id=");
        }

        #endregion

        #region jenkins

        public Uri GetJenkinsUrl()
        {
            string url = GetGeneralSettings().JenkinsUrl;

            return new Uri(url);
        }

        public Uri GetJenkinsJobUrl()
        {
            var jenkinsUrl = GetGeneralSettings().JenkinsUrl;

            return new Uri($"{jenkinsUrl}job/");
        }

        public Uri GetJenkinsJobUrl(string name)
        {
            var jenkinsUrl = GetJenkinsUrl();

            return new Uri($"{jenkinsUrl}job/{name}/api/json");
        }

        public Uri GetJenkinsJobsUrl()
        {
            var jenkinsUrl = GetJenkinsUrl();

            return new Uri($"{jenkinsUrl}api/json");
        }

        public Uri GetJenkinsFailedJobsUrl()
        {
            var jenkinsUrl = GetJenkinsUrl();

            return new Uri($"{jenkinsUrl}rssFailed");
        }

        #endregion

        #region application version packages

        public string GetApplicationVersionPackageProdFolderPath()
        {
            return GetGeneralSettings().ApplicationVersionPackageProdFolderPath;
        }

        public string GetApplicationVersionPackagePreProdFolderPath()
        {
            return GetGeneralSettings().ApplicationVersionPackagePreProdFolderPath;
        }

        #endregion

        #region database deployment packages

        public string GetDatabaseDeploymentPackageProdFolderPath()
        {
            return GetGeneralSettings().DatabaseDeploymentPackageProdFolderPath;
        }

        public string GetDatabaseDeploymentPackagePreProdFolderPath()
        {
            return GetGeneralSettings().DatabaseDeploymentPackagePreProdFolderPath;
        }

        #endregion
    }
}