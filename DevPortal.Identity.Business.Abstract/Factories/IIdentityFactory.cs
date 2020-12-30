using DevPortal.Model;
using System.Security.Claims;

namespace DevPortal.Identity.Business.Abstract
{
    public interface IIdentityFactory
    {
        ClaimsPrincipal CreateUserClaims(User user);

        UserLogOnLog CreateUserLogOnLog(string emailAddress, int userId);
    }
}