using System.Threading;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Infrastructure {
    public class AppUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser> {
        // region IUserStore
        public Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken) {
            UserRepository.Users.Add(new AppUser(user));

            return Task.FromResult(IdentityResult.Success);
        }
    }
}