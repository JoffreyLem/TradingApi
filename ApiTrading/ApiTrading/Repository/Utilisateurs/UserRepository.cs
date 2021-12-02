using System.Threading.Tasks;
using ApiTrading.DbContext;
using Microsoft.AspNetCore.Identity;

namespace ApiTrading.Repository.Utilisateurs
{
    public class UserRepository : GenericRepository<IdentityUser<int>>, IUserRepository
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public UserRepository(ApiTradingDatabaseContext context, UserManager<IdentityUser<int>> userManager,
            RoleManager<IdentityRole<int>> roleManager) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IdentityResult> CreateAsync(IdentityUser<int> user, string password, string role = "User")
        {
            using var transaction = Context.Database.BeginTransaction();

            var created = await _userManager.CreateAsync(user, password);
            var roleCreated = await _userManager.AddToRoleAsync(user, role);
            await transaction.CommitAsync();
            return created;
        }

        public async Task<IdentityUser<int>> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityUser<int>> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser<int> user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> UpdatePasswordAsync(IdentityUser<int> user, string oldpwd, string newpwd)
        {
            return await _userManager.ChangePasswordAsync(user, oldpwd, newpwd);
        }

        public async Task<IdentityResult> DeleteUser(IdentityUser<int> user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUser(IdentityUser<int> user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}