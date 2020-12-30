namespace DevPortal.Model
{
    public class ApplicationTableParam : TableParam
    {
        public string SearchText { get; set; }

        public string SortColumn { get; set; }

        public int ApplicationGroupId { get; set; }
    }
}