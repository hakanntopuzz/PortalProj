using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IEnvironmentViewModelFactory
    {
        #region environments

        EnvironmentViewModel CreateEnvironmentsViewModel(ICollection<Environment> environments);

        EnvironmentViewModel CreateEnvironmentDetailViewModel(Environment environment);

        EnvironmentViewModel CreateEnvironmentEditViewModel(Environment environment);

        EnvironmentViewModel CreateEnvironmentAddView();

        #endregion
    }
}