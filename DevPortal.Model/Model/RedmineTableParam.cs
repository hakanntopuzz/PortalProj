namespace DevPortal.Model
{
    public class RedmineTableParam : TableParam
    {
        public string SearchText { get; set; }

        public string SortColumn { get; set; }

        public int ApplicationGroupId { get; set; }
    }
}