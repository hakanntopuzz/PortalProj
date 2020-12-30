using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationGroupRepository
    {
        ICollection<ApplicationGroup> GetApplicationGroups();

        int AddApplicationGroup(ApplicationGroup applicationGroup);

        ApplicationGroup GetApplicationGroupByName(string name);

        ApplicationGroup GetApplicationGroupById(int id);

        bool UpdateApplicationGroup(ApplicationGroup group);

        ICollection<ApplicationGroupStatus> GetApplicationGroupStatusList();

        bool DeleteApplicationGroup(int groupId);

        RecordUpdateInfo GetApplicationGroupUpdateInfo(int applicationGroupId);
    }
}