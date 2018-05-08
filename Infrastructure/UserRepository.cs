
using System.Collections.Generic;
using AspNetCoreIdentity.Models;

namespace AspNetCoreIdentity.Infrastructure {
    public static class UserRepository {
        public static List<AppUser> Users;

        static UserRepository() {
            Users = new List<AppUser>();
        }
    }
}