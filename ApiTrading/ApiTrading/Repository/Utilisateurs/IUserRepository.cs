namespace ApiTrading.Repository.Utilisateurs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    public interface IUserRepository : IGenericRepository<IdentityUser<int>>
    {

        public Task<IdentityResult> CreateAsync(IdentityUser<int> user, string password, string role = "User");

        public Task<IdentityUser<int>> FindByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(IdentityUser<int> user, string password);

        public Task<IdentityResult> UpdatePasswordAsync(IdentityUser<int> user, string oldpwd, string newpwd);
        
        public Task<IdentityResult> DeleteUser(IdentityUser<int> user);



    }
}