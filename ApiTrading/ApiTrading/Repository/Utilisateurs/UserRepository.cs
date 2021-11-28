namespace ApiTrading.Repository.Utilisateurs
{
    using System.Threading.Tasks;
    using DbContext;
    using Exception;
    using Microsoft.AspNetCore.Identity;

    public class UserRepository : GenericRepository<IdentityUser<int>>, IUserRepository
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        
        public UserRepository(ApiTradingDatabaseContext context, UserManager<IdentityUser<int>> userManager, RoleManager<IdentityRole<int>> roleManager) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IdentityResult> CreateAsync(IdentityUser<int> user, string password, string role = "User")
        {
            var created = await _userManager.CreateAsync(user, password);
            var roleCreated = await _userManager.AddToRoleAsync(user, role);
            return (created);
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
    }
}