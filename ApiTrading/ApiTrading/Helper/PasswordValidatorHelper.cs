using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ApiTrading.Helper
{
    public class PasswordValidatorHelper<T> : IPasswordValidator<T> where T : IdentityUser<int>

    {
        public Task<IdentityResult> ValidateAsync(UserManager<T> manager, T user, string password)
        {
            if (password.Length < 10)
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "",
                    Description = "Le mot de passe doit faire un minimun de 10 caractères"
                }));
            return Task.FromResult(IdentityResult.Success);
        }
    }
}