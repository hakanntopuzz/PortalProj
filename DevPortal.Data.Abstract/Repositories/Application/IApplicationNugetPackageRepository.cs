using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationNugetPackageRepository
    {
        ICollection<ApplicationNugetPackage> GetNugetPackages(int applicationId);

        ApplicationNugetPackage GetApplicationNugetPackageById(int packageId);

        RecordUpdateInfo GetPackageUpdateInfo(int id);

        bool AddApplicationNugetPackage(ApplicationNugetPackage package);

        bool UpdateApplicationNugetPackage(ApplicationNugetPackage nugetPackage);

        ApplicationNugetPackage GetApplicationNugetPackageByName(string packageName);

        bool DeleteApplicationNugetPackage(int packageId);
    }
}