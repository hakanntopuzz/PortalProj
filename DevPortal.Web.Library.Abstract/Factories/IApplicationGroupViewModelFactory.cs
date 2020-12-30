using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationGroupViewModelFactory
    {
        #region application group

        ApplicationGroupViewModel CreateApplicationGroupsViewModel(ICollection<ApplicationGroup> applicationGroups);

        ApplicationGroupViewModel CreateEditApplicationGroup(ApplicationGroup applicationGroup, ICollection<ApplicationGroupStatus> status);

        ApplicationGroupViewModel CreateApplicationGroupAddViewModel(ICollection<ApplicationGroupStatus> status);

        ApplicationGroupViewModel CreateDetailApplicationGroup(ApplicationGroup applicationGroup, ICollection<ApplicationListItem> applicationLists);

        #endregion
    }
}