using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library
{
    public class AuditViewModelFactory : IAuditViewModelFactory
    {
        public AuditViewModel CreateAuditViewModel(string tableName, int recordId)
        {
            return new AuditViewModel
            {
                TableName = tableName,
                RecordId = recordId
            };
        }
    }
}