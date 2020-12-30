using DevPortal.Model;

namespace DevPortal.Business.Abstract.Services
{
    public interface INugetPackageDependencyService
    {
        NugetPackageDependency GetNugetPackageDependencyById(int id);

        ServiceResult AddNugetPackageDependency(NugetPackageDependency nugetPackageDependency);

        ServiceResult DeleteNugetPackageDependency(int nugetPackageDependencyId);
    }
}