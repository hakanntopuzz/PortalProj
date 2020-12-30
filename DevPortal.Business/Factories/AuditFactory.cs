using DevPortal.Business.Abstract;
using DevPortal.Model;

namespace DevPortal.Business.Factories
{
    public class AuditFactory : IAuditFactory
    {
        public AuditInfo CreateAuditInfo(string tableName, int recordId, object oldRecord, object newRecord, int modifiedBy)
        {
            return new AuditInfo
            {
                TableName = tableName,
                RecordId = recordId,
                OldRecord = oldRecord,
                NewRecord = newRecord,
                ModifiedBy = modifiedBy
            };
        }
    }
}