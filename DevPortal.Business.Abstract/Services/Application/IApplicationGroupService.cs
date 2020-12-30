using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationGroupService
    {
        ICollection<ApplicationGroup> GetApplicationGroups();

        Int32ServiceResult AddApplicationGroup(ApplicationGroup applicationGroup);

        ApplicationGroup GetApplicationGroupByName(string name);

        ApplicationGroup GetApplicationGroupById(int applicationGroupId);

        ServiceResult UpdateApplicationGroup(ApplicationGroup applicationGroup);

        ICollection<ApplicationGroupStatus> GetApplicationGroupStatusList();

        ServiceResult DeleteApplicationGroup(int groupId);
    }
}