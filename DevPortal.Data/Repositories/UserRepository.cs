using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Repositories
{
    public class UserRepository : BaseDevPortalRepository, IUserRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public UserRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory applicationDataRequestFactory,
            ISettings settings)
            : base(dataClient, applicationDataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public async Task<User> GetUserAsync(int id)
        {
            var dataRequest = dataRequestFactory.GetUser(id);
            const User defaultReturnValue = null;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<User> GetUserAsync(string svnUserName)
        {
            var dataRequest = dataRequestFactory.GetUser(svnUserName);
            const User defaultReturnValue = null;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<User>> GetFilteredUserListAsync(UserSearchFilter searchFilter)
        {
            var dataRequest = dataRequestFactory.GetFilteredUserList(searchFilter);
            var defaultReturnValue = new List<User>();

            return await dataClient.GetCollectionAsync<User, RecordUpdateInfo, User>(
               dataRequest,
               DataClientMapFactory.UsersMap,
               defaultReturnValue,
               dataRequest.SplitOnParameters);
        }

        public async Task<ICollection<UserStatus>> GetUserStatusListAsync()
        {
            var dataRequest = dataRequestFactory.GetUserStatusList();
            var defaultReturnValue = new List<UserStatus>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<UserType>> GetUserTypeListAsync()
        {
            var dataRequest = dataRequestFactory.GetUserTypeList();
            var defaultReturnValue = new List<UserType>();

            return await dataClient.GetCollectionAsync(dataRequest, defaultReturnValue);
        }

        public async Task<int> AddUserAsync(User user)
        {
            var dataRequest = dataRequestFactory.AddUser(user);
            const int defaultReturnValue = 0;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<User> GetUserByEmailAddressAsync(string emailAddress)
        {
            var dataRequest = dataRequestFactory.GetUserByEmailAddress(emailAddress);
            const User defaultReturnValue = null;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> AddUserLogOnLogAsync(UserLogOnLog userLogOnLog)
        {
            var dataRequest = dataRequestFactory.AddUserLogOnLog(userLogOnLog);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var dataRequest = dataRequestFactory.UpdateUser(user);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var dataRequest = dataRequestFactory.DeleteUser(userId);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> UpdateOwnUserAsync(User user)
        {
            var dataRequest = dataRequestFactory.UpdateOwnUser(user);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<RecordUpdateInfo> GetUserUpdateInfoAsync(int id)
        {
            var dataRequest = dataRequestFactory.GetUserUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> AddPasswordResetRequestAsync(int userId, string ipAddress, string requestCode)
        {
            var dataRequest = dataRequestFactory.AddPasswordResetRequest(userId, ipAddress, requestCode);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<int> CheckPasswordResetRequestAsync(string requestCode)
        {
            var dataRequest = dataRequestFactory.CheckPasswordResetRequest(requestCode);
            const int defaultReturnValue = 0;

            return await dataClient.GetItemAsync(dataRequest, defaultReturnValue);
        }

        public async Task<bool> UpdatePasswordResetRequestAsync(int userId)
        {
            var dataRequest = dataRequestFactory.UpdatePasswordResetRequest(userId);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }
    }
}