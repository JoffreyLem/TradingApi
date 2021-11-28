namespace ApiTrading
{
    using System;
    using Exception;
    using Microsoft.AspNetCore.Mvc.Filters;

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