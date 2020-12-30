using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Services
{
    public class ApplicationNugetPackageService : IApplicationNugetPackageService
    {
        #region ctor

        readonly IApplicationNugetPackageRepository repository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        readonly INugetPackageService nugetPackageService;

        public ApplicationNugetPackageService(
            IApplicationNugetPackageRepository repository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService,
            INugetPackageService nugetPackageService)
        {
            this.repository = repository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
            this.nugetPackageService = nugetPackageService;
        }

        #endregion

        public ICollection<ApplicationNugetPackage> GetNugetPackages(int applicationId)
        {
            var packages = repository.GetNugetPackages(applicationId);
            packages.ToList().ForEach(q => q.PackageUrl = nugetPackageService.GetNugetPackageUrl(q.NugetPackageName).ToString());

            return packages;
        }

        public ApplicationNugetPackage GetApplicationNugetPackage(int id)
        {
            var user = repository.GetApplicationNugetPackageById(id);
            user.RecordUpdateInfo = repository.GetPackageUpdateInfo(id);

            return user;
        }

        public ServiceResult AddApplicationNugetPackage(ApplicationNugetPackage package)
        {
            var nugetPackage = GetApplicationNugetPackageByName(package.NugetPackageName);

            if (ApplicationNugetPackageExists(nugetPackage))
            {
                return ServiceResult.Error(Messages.ApplicationNugetPackageNameExists);
            }

            var addSucceeds = repository.AddApplicationNugetPackage(package);

            if (!addSucceeds)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationNugetPackageCreated);
        }

        public ServiceResult UpdateApplicationNugetPackage(ApplicationNugetPackage nugetPackage)
        {
            if (nugetPackage == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }
            var oldNugetPackage = repository.GetApplicationNugetPackageById(nugetPackage.NugetPackageId);

            var isChanged = auditService.IsChanged(oldNugetPackage, nugetPackage, nameof(ApplicationNugetPackage));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationNugetPackageUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationNugetPackageCore(nugetPackage);

                    var newNugetPackage = repository.GetApplicationNugetPackageById(nugetPackage.NugetPackageId);

                    AddAuditCore(newNugetPackage, oldNugetPackage, nugetPackage.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationNugetPackage), "Uygulama nuget paketi güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationNugetPackageUpdated);
        }

        void UpdateApplicationNugetPackageCore(ApplicationNugetPackage nugetPackage)
        {
            var updateSuccess = repository.UpdateApplicationNugetPackage(nugetPackage);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama nuget paketi güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(ApplicationNugetPackage nugetPackage, ApplicationNugetPackage oldNugetPackage, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(ApplicationNugetPackage), nugetPackage.NugetPackageId, oldNugetPackage, nugetPackage, userId);

            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        static bool ApplicationNugetPackageExists(ApplicationNugetPackage nugetPackage)
        {
            return nugetPackage != null;
        }

        public ApplicationNugetPackage GetApplicationNugetPackageByName(string packageName)
        {
            return repository.GetApplicationNugetPackageByName(packageName);
        }

        public ServiceResult DeleteApplicationNugetPackage(int packageId)
        {
            var isSuccess = repository.DeleteApplicationNugetPackage(packageId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationNugetPackageDeleted);
        }
    }
}