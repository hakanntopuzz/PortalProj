using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;

namespace DevPortal.Business.Services
{
    public class NugetPackageDependencyService : INugetPackageDependencyService
    {
        #region ctor

        readonly INugetPackageDependencyRepository nugetPackageDependencyRepository;

        readonly INugetPackageService nugetPackageService;

        public NugetPackageDependencyService(
            INugetPackageDependencyRepository nugetPackageDependencyRepository,
            INugetPackageService nugetPackageService)

        {
            this.nugetPackageDependencyRepository = nugetPackageDependencyRepository;
            this.nugetPackageService = nugetPackageService;
        }

        #endregion

        public NugetPackageDependency GetNugetPackageDependencyById(int id)
        {
            var nugetPackageDependency = nugetPackageDependencyRepository.GetNugetPackageDependencyById(id);

            if (nugetPackageDependency == null)
            {
                return null;
            }

            var nugetPackageUrl = nugetPackageService.GetNugetPackageUrl(nugetPackageDependency.NugetPackageName);
            nugetPackageDependency.PackageUrl = nugetPackageUrl.ToString();
            nugetPackageDependency.RecordUpdateInfo = nugetPackageDependencyRepository.GetNugetPackageDependencyUpdateInfo(id);

            return nugetPackageDependency;
        }

        public ServiceResult AddNugetPackageDependency(NugetPackageDependency nugetPackageDependency)
        {
            var isSuccess = nugetPackageDependencyRepository.AddNugetPackageDependency(nugetPackageDependency);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.NugetPackageDependencyCreated);
        }

        public ServiceResult DeleteNugetPackageDependency(int nugetPackageDependencyId)
        {
            var isSuccess = nugetPackageDependencyRepository.DeleteNugetPackageDependency(nugetPackageDependencyId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.NugetPackageDependencyDeleted);
        }
    }
}