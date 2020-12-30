using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using Environment = DevPortal.Model.Environment;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationEnvironmentViewModelFactory
    {
        ApplicationEnvironment CreateApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        ApplicationEnvironment CreateApplicationEnvironment(int applicationId, string applicationName);

        ApplicationEnvironmentViewModel CreateApplicationEnvironmentViewModel(ApplicationEnvironmentViewModel model, ICollection<Environment> environments);

        ApplicationEnvironmentViewModel CreateApplicationEnvironmentViewModel(ApplicationEnvironment applicationEnvironment);

        ApplicationEnvironmentViewModel CreateApplicationEnvironmentDetailViewModel(ApplicationEnvironment applicationEnvironment);

        ApplicationEnvironmentViewModel CreateEditApplicationEnvironmentViewModel(ApplicationEnvironment applicationEnvironment, ICollection<Environment> environments);
    }
}