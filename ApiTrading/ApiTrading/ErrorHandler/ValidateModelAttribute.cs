using System;
using ApiTrading.Exception;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiTrading
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new AppException("Invalid Payload");
            }
            base.OnActionExecuting(context);
        }
    }
}