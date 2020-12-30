namespace DevPortal.Model
{
    public class AuditTableParam : TableParam
    {
        public string TableName { get; set; }

        public int RecordId { get; set; }

        public string SortColumn { get; set; }

        public string SearchText { get; set; }
    }
}