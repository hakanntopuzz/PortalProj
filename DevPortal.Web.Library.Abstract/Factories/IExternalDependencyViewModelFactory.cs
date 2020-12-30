using DevPortal.Model;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface IExternalDependencyViewModelFactory
    {
        ExternalDependencyViewModel CreateExternalDependencyViewModel(ExternalDependency externalDependency);

        ExternalDependencyViewModel CreateAddExternalDependencyViewModel(string applicationName, int applicationId);

        ExternalDependencyViewModel CreateEditExternalDependencyViewModel(ExternalDependency externalDependency);
    }
}