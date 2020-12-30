using System.Security.Claims;
using System.Threading.Tasks;

namespace DevPortal.Framework.Abstract
{
    //TODO: Cohesion olarak incelenmeli ve sorumlulukları dağıtılmalı. HttpContext işlemlerini doğrudan controllerdan kullanmak yerine ilgili sınıftan kullanılmalı.
    public interface IHttpContextWrapper
    {
        string GetUserAgentString();

        string GetUserIpAddress();

        Task SignInAsync(ClaimsPrincipal claimsPrincipal, bool isRemember);

        Task SignOutAsync();

        ClaimsPrincipal GetCurrentUser();

        void DownloadAsPdf(string fileName);
    }
}