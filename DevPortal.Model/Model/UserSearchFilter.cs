namespace DevPortal.Model
{
    public class UserSearchFilter : TableParam
    {
        public string SearchText { get; set; }

        public string SortColumn { get; set; }
    }
}