namespace ApiTrading.Filter
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Service.ExternalAPIHandler;
    using XtbLibrairie.sync;

    public class XtbCheckConnectorFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var service = context.HttpContext.RequestServices.GetServices<IApiHandler>().FirstOrDefault();

            Connector connector = service.connector;
            if (connector is null)
            {
                var error = new ErrorModel();
                error.ErrorMessage.Add("La connection à l'api XTB n'est pas faite");
                context.Result = new BadRequestObjectResult(error);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //  throw new NotImplementedException();
        }
    }
}