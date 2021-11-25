using System;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CodeActions;

namespace ApiTrading.Filter
{
    public static class ClaimUserHelper
    {
        public static  IdentityUser<int> GetCurrentUser(this HttpContext context)
        {
            var service =(UserManager<IdentityUser<int>>) context.RequestServices.GetService(typeof(UserManager<IdentityUser<int>>));
            var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var user = service.FindByEmailAsync(claim.Value).Result;

            return (IdentityUser<int>) user;

        }
    }
}