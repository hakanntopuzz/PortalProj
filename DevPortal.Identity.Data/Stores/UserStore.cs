using DevPortal.Data.Abstract;
using DevPortal.Model;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace DevPortal.Identity.Data
{
    public class UserStore : IUserPasswordStore<User>, IUserEmailStore<User>
    {
        #region ctor

        readonly IUserRepository userRepository;

        public UserStore(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        #endregion

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int userId = await userRepository.AddUserAsync(user);

            if (userId == 0)
            {
                return IdentityResult.Failed(new IdentityError { Description = Messages.AddingFails });
            }

            user.Id = userId;
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return userRepository.GetUserByEmailAddressAsync(normalizedEmail.ToLower(new CultureInfo("en-US", false)));
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int userIdInt = int.Parse(userId);
            return userRepository.GetUserAsync(userIdInt);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return userRepository.GetUserAsync(normalizedUserName.ToLower(new CultureInfo("en-US", false)));
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.DisplayName);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.DisplayName);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.EmailAddress = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isSuccess = await userRepository.UpdateOwnUserAsync(user);

            if (isSuccess)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = Messages.UpdateFails });
        }

        #region Dispose

        public void Dispose()
        {
        }

        #endregion
    }
}