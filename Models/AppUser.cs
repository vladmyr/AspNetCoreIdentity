namespace AspNetCoreIdentity.Models {
    public class AppUser {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NormalizeUserName { get; set; }
        public string PasswordHash { get; set; }

        public AppUser() {}
        
        public AppUser(
            string id, 
            string userName, 
            string email, 
            string normalizeUserName, 
            string passwordHash
        ) {
            Id = id;
            UserName = userName;
            Email = email;
            NormalizeUserName = normalizeUserName;
            PasswordHash = passwordHash;
        }

        public AppUser(AppUser user) {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            NormalizeUserName = user.NormalizeUserName;
            PasswordHash = user.PasswordHash;
        }
    }
}