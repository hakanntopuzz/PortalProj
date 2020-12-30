namespace DevPortal.Web.Library.Model
{
    public static class PartialViewNames
    {
        static string CreatePartialViewName(string partialViewName)
        {
            return $"_{partialViewName}";
        }

        public static string GetResultFileListPartial => nameof(GetResultFileListPartial);

        public static string AesSidebarMenu => CreatePartialViewName(nameof(AesSidebarMenu));

        public static string SamplesSidebarMenu => CreatePartialViewName(nameof(SamplesSidebarMenu));

        public static string AccountSidebarMenu => CreatePartialViewName(nameof(AccountSidebarMenu));

        public static string SecuritySidebarMenu => CreatePartialViewName(nameof(SecuritySidebarMenu));

        public static string Message => CreatePartialViewName(nameof(Message));

        public static string BreadCrumb => CreatePartialViewName(nameof(BreadCrumb));

        public static string Urls => CreatePartialViewName(nameof(Urls));

        public static string List => CreatePartialViewName(nameof(List));
    }
}