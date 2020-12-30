using DevPortal.Model;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract.Factories
{
    public interface INugetPackageDependencyViewModelFactory
    {
        NugetPackageDependencyViewModel CreateNugetPackageDependencyViewModel(NugetPackageDependency nugetPackageDependency);

        NugetPackageDependencyViewModel CreateNugetPackageDependencyViewModelAddView(Application application);
    }
}