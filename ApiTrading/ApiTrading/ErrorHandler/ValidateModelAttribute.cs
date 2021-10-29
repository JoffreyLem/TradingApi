using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiTrading
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new ApplicationException("Invalid Payload");

            }
            base.OnActionExecuting(context);
        }
    }
}