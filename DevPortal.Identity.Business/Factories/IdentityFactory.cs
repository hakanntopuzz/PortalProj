using DevPortal.Framework.Abstract;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Model;
using System.Security.Claims;

namespace DevPortal.Identity.Business
{
    public class IdentityFactory : IIdentityFactory
    {
        readonly IUserAgentService userAgentService;

        public IdentityFactory(IUserAgentService userAgentService)
        {
            this.userAgentService = userAgentService;
        }

        static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value);
        }

        public ClaimsPrincipal CreateUserClaims(User user)
        {
            var claimsIdentity = new ClaimsIdentity("cookies");
            claimsIdentity.AddClaim(CreateClaim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(CreateClaim(ClaimTypes.Name, user.DisplayName));
            claimsIdentity.AddClaim(CreateClaim(ClaimTypes.Role, user.UserTypeId.ToString()));
            claimsIdentity.AddClaim(CreateClaim(ClaimTypes.Email, user.EmailAddress));
            claimsIdentity.AddClaim(CreateClaim(ClaimTypes.Sid, user.SecureId));

            return new ClaimsPrincipal(claimsIdentity);
        }

        public UserLogOnLog CreateUserLogOnLog(string emailAddress, int userId)
        {
            var userAgent = userAgentService.GetUserAgent();

            return new UserLogOnLog
            {
                BrowserName = userAgent.BrowserName,
                BrowserVersion = userAgent.BrowserVersion,
                IpAddress = userAgent.IpAddress,
                EmailAddress = emailAddress,
                UserId = userId
            };
        }
    }
}