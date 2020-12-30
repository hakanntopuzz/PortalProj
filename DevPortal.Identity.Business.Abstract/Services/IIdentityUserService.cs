using DevPortal.Model;
using System.Threading.Tasks;

namespace DevPortal.Identity.Business.Abstract
{
    public interface IIdentityUserService
    {
        Task<Int32ServiceResult> CreateUserAsync(User user, string password);

        Task<ServiceResult> UpdateUserAsync(User user);

        Task<ServiceResult> LoginAsync(string emailAddress, string password, bool isRemember);

        Task<ServiceResult> LogOutAsync();

        Task<User> GetUserAsync();

        Task<User> GetUserWithUpdateInfoAsync(int userId);

        Task<ServiceResult> ChangePasswordAsync(string password, string newPassword);

        Task<ServiceResult> ForgotPasswordAsync(string emailAddress);

        Task<ServiceResult> ResetPasswordAsync(string password, string token);
    }
}