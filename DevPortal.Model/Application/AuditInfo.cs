namespace DevPortal.Model
{
    public class AuditInfo
    {
        public string TableName { get; set; }

        public int RecordId { get; set; }

        public object OldRecord { get; set; }

        public object NewRecord { get; set; }

        public int ModifiedBy { get; set; }
    }
}