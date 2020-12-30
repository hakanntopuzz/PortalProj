using DevPortal.Model;
using System.Threading.Tasks;

namespace DevPortal.Business.Abstract
{
    public interface IAuditService
    {
        Task<bool> AddAsync(AuditInfo auditInfo);

        Task<JQTable> GetFilteredAuditListAsJqTableAsync(AuditTableParam tableParam);

        bool IsChanged(object oldRecord, object newRecord, string compareModelName);
    }
}