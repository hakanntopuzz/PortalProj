namespace DevPortal.Model
{
    public class DatabaseTableParam : TableParam
    {
        public string SearchText { get; set; }

        public string SortColumn { get; set; }

        public int DatabaseGroupId { get; set; }
    }
}