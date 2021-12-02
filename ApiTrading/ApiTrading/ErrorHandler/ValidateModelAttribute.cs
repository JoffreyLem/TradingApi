using System;
using ApiTrading.Exception;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiTrading
{
    public class ValidateModelAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid) throw new AppException("Invalid Payload");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}