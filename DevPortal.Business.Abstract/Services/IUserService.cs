using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Business.Abstract
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int id);

        Task<JQTable> GetFilteredUsersJqTableAsync(UserSearchFilter searchFilter);

        Task<ICollection<UserStatus>> GetUserStatusListAsync();

        Task<ICollection<UserType>> GetUserTypeListAsync();

        Task<ServiceResult> UpdateUserAsync(User user);

        Task<ServiceResult> DeleteUserAsync(int userId);
    }
}