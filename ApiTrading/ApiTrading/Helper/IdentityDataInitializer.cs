using System.Configuration;
using Microsoft.AspNetCore.Identity;

namespace ApiTrading.Helper
{
    public class IdentityDataInitializer
    {
        public static void SeedData
        (UserManager<IdentityUser<int>> userManager, 
            RoleManager<IdentityRole<int>> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers
            (UserManager<IdentityUser<int>> userManager)
        {
            if (userManager.FindByNameAsync("System").Result == null)
            {
                IdentityUser<int> identityUser = new IdentityUser<int>();
                identityUser.UserName = "System";
                string passwd = PasswordGenerator.GenerateRandomPassword();
                IdentityResult result = userManager.CreateAsync(identityUser, passwd).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(identityUser, "System").Wait();
                    
                }
            }
        }

        public static void SeedRoles
            (RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            
            if (!roleManager.RoleExistsAsync("System").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "System";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            
        }
    }
}