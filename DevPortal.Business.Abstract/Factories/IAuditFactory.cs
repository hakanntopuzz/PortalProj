using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IAuditFactory
    {
        AuditInfo CreateAuditInfo(string tableName, int recordId, object oldRecord, object newRecord, int modifiedBy);
    }
}