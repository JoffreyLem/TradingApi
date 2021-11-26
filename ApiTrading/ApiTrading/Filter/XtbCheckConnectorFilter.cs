namespace ApiTrading.Filter
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Service.ExternalAPIHandler;
    using XtbLibrairie.sync;

    public class XtbCheckConnectorFilter :IActionFilter
    {
        public  void OnActionExecuting(ActionExecutingContext context)
        {
            IApiHandler service = context.HttpContext.RequestServices.GetServices<IApiHandler>().FirstOrDefault();
         
                Connector connector = service.connector;
                if (connector is null)
                {
                    ErrorModel error = new ErrorModel();
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