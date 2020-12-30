namespace DevPortal.Model
{
    public class Record
    {
        public RecordUpdateInfo RecordUpdateInfo { get; set; }

        public bool IsDeleted { get; set; }

        public Record()
        {
            RecordUpdateInfo = new RecordUpdateInfo();
        }
    }
}