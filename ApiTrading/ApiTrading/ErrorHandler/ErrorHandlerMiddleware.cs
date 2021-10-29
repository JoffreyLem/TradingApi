using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Http;

namespace ApiTrading
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch(error)
                {
                    case ApplicationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                      
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case AuthenticationException e:
                        response.StatusCode = (int) HttpStatusCode.Forbidden;
                        break;
                    default:
                       
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                
                var result = new ErrorModel(response.StatusCode,error?.Message);
                await response.WriteAsync(result.ToString());
            }
        }
    }
}