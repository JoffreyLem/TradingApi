namespace ApiTrading.Filter
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public static class ClaimUserHelper
    {
        public static IdentityUser<int> GetCurrentUser(this HttpContext context)
        {
            var service =
                (UserManager<IdentityUser<int>>)context.RequestServices.GetService(
                    typeof(UserManager<IdentityUser<int>>));
            var claim = context.User.FindFirst("Id");
            var user = service.FindByIdAsync(claim.Value).Result;
            return user;
        }
    }
}