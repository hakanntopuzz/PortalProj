using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Abstract
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(int id);

        Task<User> GetUserAsync(string svnUserName);

        Task<ICollection<User>> GetFilteredUserListAsync(UserSearchFilter searchFilter);

        Task<ICollection<UserStatus>> GetUserStatusListAsync();

        Task<ICollection<UserType>> GetUserTypeListAsync();

        Task<int> AddUserAsync(User user);

        Task<User> GetUserByEmailAddressAsync(string emailAddress);

        Task<bool> AddUserLogOnLogAsync(UserLogOnLog userLogOnLog);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(int userId);

        Task<bool> UpdateOwnUserAsync(User user);

        Task<RecordUpdateInfo> GetUserUpdateInfoAsync(int id);

        Task<bool> AddPasswordResetRequestAsync(int userId, string ipAddress, string requestCode);

        Task<int> CheckPasswordResetRequestAsync(string requestCode);

        Task<bool> UpdatePasswordResetRequestAsync(int userId);
    }
}