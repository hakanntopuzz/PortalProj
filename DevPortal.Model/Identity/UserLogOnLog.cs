namespace DevPortal.Model
{
    public class UserLogOnLog
    {
        public int UserId { get; set; }

        public string IpAddress { get; set; }

        public string EmailAddress { get; set; }

        public string BrowserInfo { get; set; }

        public string BrowserName { get; set; }

        public string BrowserVersion { get; set; }
    }
}