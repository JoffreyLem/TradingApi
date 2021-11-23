using System;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CodeActions;

namespace ApiTrading.Filter
{
    public class ClaimUserFilter: ActionFilterAttribute
    {
        
    

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var service =(UserManager<IdentityUser<int>>) context.HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser<int>>));
            var claim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var user = await service.FindByEmailAsync(claim.Value);
            context.HttpContext.Items.Add("user",user);
            //       var user = _userManager.FindByIdAsync()
            //context.HttpContext.Request[""] = "";
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

     
    }


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