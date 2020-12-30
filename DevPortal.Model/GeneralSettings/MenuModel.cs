namespace DevPortal.Model
{
    public class MenuModel : BaseMenu
    {
        public string ParentName { get; set; }

        public int TotalCount { get; set; }

        public string GroupName { get; set; }

        bool HasGroup()
        {
            return MenuGroupId != null && MenuGroupId != 0;
        }
    }
}