using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationDependencyViewModelFactory
    {
        ApplicationDependencyViewModel CreateApplicationDependencyViewModel(ApplicationDependency applicationDependency);

        ApplicationDependencyViewModel CreatApplicationDependencyViewModelAddView(int applicationId, ICollection<ApplicationListItem> applicationLists, ICollection<ApplicationGroup> applicationGroups);

        ApplicationDependencyViewModel CreateApplicationDependencyEditViewModel(ApplicationDependency applicationDependency);
    }
}