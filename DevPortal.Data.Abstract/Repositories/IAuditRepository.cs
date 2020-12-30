using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Abstract
{
    public interface IAuditRepository
    {
        Task<bool> AddAsync(string tableName, int recordId, string fieldName, string oldValue, string newValue, int modifiedBy);

        Task<ICollection<Audit>> GetFilteredAuditListAsync(int skip, int take, string orderBy, string orderDir, string searchText, string tableName, int recordId);
    }
}