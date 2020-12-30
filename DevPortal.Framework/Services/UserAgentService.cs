using DevPortal.Framework.Abstract;
using UAParser;

namespace DevPortal.Framework.Services
{
    public class UserAgentService : IUserAgentService
    {
        readonly IHttpContextWrapper httpContextWrapper;

        public UserAgentService(IHttpContextWrapper httpContextWrapper)
        {
            this.httpContextWrapper = httpContextWrapper;
        }

        public DevPortal.Model.UserAgent GetUserAgent()
        {
            string userAgentString = httpContextWrapper.GetUserAgentString();
            string ipAddress = httpContextWrapper.GetUserIpAddress();

            var userInfo = ParseUserAgent(userAgentString);

            return new DevPortal.Model.UserAgent
            {
                BrowserName = userInfo.Family,
                BrowserVersion = $"{userInfo.Major}.{userInfo.Minor}",
                IpAddress = ipAddress
            };
        }

        private UserAgent ParseUserAgent(string userAgentString)
        {
            var parser = Parser.GetDefault();

            return parser.ParseUserAgent(userAgentString);
        }
    }
}