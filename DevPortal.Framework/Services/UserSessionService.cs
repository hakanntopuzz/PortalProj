using DevPortal.Framework.Abstract;
using System.Linq;
using System.Security.Claims;

namespace DevPortal.Framework.Services
{
    public class UserSessionService : IUserSessionService
    {
        readonly IHttpContextWrapper httpContextWrapper;

        public UserSessionService(IHttpContextWrapper httpContextWrapper)
        {
            this.httpContextWrapper = httpContextWrapper;
        }

        private ClaimsPrincipal GetCurrentUser()
        {
            return httpContextWrapper.GetCurrentUser();
        }

        public int GetCurrentUserId()
        {
            var currentUser = GetCurrentUser();
            var currentUserId = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return int.Parse(currentUserId);
        }
    }
}