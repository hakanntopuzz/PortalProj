using DevPortal.Model;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface INugetPackageDependencyRepository
    {
        NugetPackageDependency GetNugetPackageDependencyById(int id);

        RecordUpdateInfo GetNugetPackageDependencyUpdateInfo(int id);

        bool AddNugetPackageDependency(NugetPackageDependency nugetPackageDependency);

        bool DeleteNugetPackageDependency(int nugetPackageDependencyId);
    }
}