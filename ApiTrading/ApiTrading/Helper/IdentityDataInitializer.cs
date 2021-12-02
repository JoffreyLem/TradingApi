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
                var identityUser = new IdentityUser<int>();
                identityUser.UserName = "System";
                var passwd = PasswordGenerator.GenerateRandomPassword();
                var result = userManager.CreateAsync(identityUser, passwd).Result;

                if (result.Succeeded) userManager.AddToRoleAsync(identityUser, "System").Wait();
            }
        }

        public static void SeedRoles
            (RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole<int>();
                role.Name = "User";
                var roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("System").Result)
            {
                var role = new IdentityRole<int>();
                role.Name = "System";
                var roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole<int>();
                role.Name = "Admin";
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}