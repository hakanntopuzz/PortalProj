using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationNugetPackageService
    {
        ICollection<ApplicationNugetPackage> GetNugetPackages(int applicationId);

        ApplicationNugetPackage GetApplicationNugetPackage(int id);

        ServiceResult AddApplicationNugetPackage(ApplicationNugetPackage package);

        ServiceResult UpdateApplicationNugetPackage(ApplicationNugetPackage nugetPackage);

        ServiceResult DeleteApplicationNugetPackage(int packageId);
    }
}