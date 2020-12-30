using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortal.Business.Services
{
    public class UserService : IUserService
    {
        #region ctor

        readonly IUserRepository userRepository;

        readonly IAuditService auditService;

        readonly ILoggingService loggingService;

        readonly IAuditFactory auditFactory;

        public UserService(
            IUserRepository userRepository,
            IAuditService auditService,
            ILoggingService loggingService,
            IAuditFactory auditFactory)
        {
            this.userRepository = userRepository;
            this.auditService = auditService;
            this.loggingService = loggingService;
            this.auditFactory = auditFactory;
        }

        #endregion

        #region GetUserAsync

        public async Task<User> GetUserAsync(int id)
        {
            var user = await userRepository.GetUserAsync(id);

            if (user == null)
            {
                return null;
            }

            user.RecordUpdateInfo = await userRepository.GetUserUpdateInfoAsync(id);

            return user;
        }

        #endregion

        #region GetFilteredUsersJqTableAsync

        public async Task<JQTable> GetFilteredUsersJqTableAsync(UserSearchFilter searchFilter)
        {
            var data = await userRepository.GetFilteredUserListAsync(searchFilter);
            int recordCount = data.Any() ? data.First().TotalCount : 0;

            return new JQTable
            {
                data = data,
                recordsFiltered = recordCount,
                recordsTotal = recordCount
            };
        }

        #endregion

        #region GetUserStatusListAsync

        public async Task<ICollection<UserStatus>> GetUserStatusListAsync()
        {
            return await userRepository.GetUserStatusListAsync();
        }

        #endregion

        #region GetUserTypeListAsync

        public async Task<ICollection<UserType>> GetUserTypeListAsync()
        {
            return await userRepository.GetUserTypeListAsync();
        }

        #endregion

        #region UpdateUserAsync

        public async Task<ServiceResult> UpdateUserAsync(User user)
        {
            if (user == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            if (UserEmailAddressExists(user))
            {
                return ServiceResult.Error(Messages.UserEmailAddressExists);
            }

            if (SvnUserNameExists(user))
            {
                return ServiceResult.Error(Messages.SvnUserNameExists);
            }

            var oldUser = await userRepository.GetUserAsync(user.Id);

            var isChanged = auditService.IsChanged(oldUser, user, nameof(User));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.UserUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateUserCore(user);

                    var newUser = await userRepository.GetUserAsync(user.Id);

                    await AddAuditCore(newUser, oldUser, user.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateUserAsync), "Kullanıcı güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.UserUpdated);
        }

        void UpdateUserCore(User user)
        {
            var isSuccess = userRepository.UpdateUserAsync(user).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Kullanıcı güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        async Task AddAuditCore(User user, User oldUser, int modifiedBy)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(User), user.Id, oldUser, user, modifiedBy);
            var isSuccess = await auditService.AddAsync(auditInfo);

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        bool UserEmailAddressExists(User user)
        {
            var userByEmail = GetUserByEmailAddress(user.EmailAddress);

            if (!UserExists(userByEmail))
            {
                return false;
            }

            if (userByEmail.Result.Id == user.Id)
            {
                return false;
            }

            return true;
        }

        bool SvnUserNameExists(User user)
        {
            var userBySvnName = GetUserBySvnUserName(user.SvnUserName);

            if (!UserExists(userBySvnName))
            {
                return false;
            }

            if (userBySvnName.Result.Id == user.Id)
            {
                return false;
            }

            return true;
        }

        public Task<User> GetUserByEmailAddress(string emailAddress)
        {
            return userRepository.GetUserByEmailAddressAsync(emailAddress);
        }

        static bool UserExists(Task<User> user)
        {
            return user.Result != null;
        }

        public Task<User> GetUserBySvnUserName(string svnUserName)
        {
            return userRepository.GetUserAsync(svnUserName);
        }

        #region DeleteUserAsync

        public async Task<ServiceResult> DeleteUserAsync(int userId)
        {
            var deleteSucceeds = await userRepository.DeleteUserAsync(userId);

            if (!deleteSucceeds)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.UserDeleted);
        }

        #endregion
    }
}