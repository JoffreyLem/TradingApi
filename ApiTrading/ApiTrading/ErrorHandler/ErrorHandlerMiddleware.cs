using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ApiTrading.Exception;
using Microsoft.AspNetCore.Http;
using XtbLibrairie.errors;
using XtbLibrairie.responses;
using AuthenticationException = MailKit.Security.AuthenticationException;

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
            catch (CustomErrorException error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    case TimeFrameDontExistException e:
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    case AlreadyExistException e:
                        response.StatusCode = (int) HttpStatusCode.Conflict;
                        break;
                    case NotFoundException e:
                        response.StatusCode = (int) HttpStatusCode.NotFound;
                        break;
                    case AuthException e:
                        response.StatusCode = (int) HttpStatusCode.Forbidden;
                        break;
                    default:

                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }

                var result = new ErrorModel(response.StatusCode, error?.ErrorsMessages);
                await response.WriteAsync(result.ToString());
            }
            catch (APIErrorResponse err)
            {
                var response = context.Response;
                var result = new ErrorModel();
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                result.StatusCode = (int) HttpStatusCode.BadRequest;
                result.ErrorMessage.Add(err.ErrorDescr);
                await response.WriteAsync(result.ToString());
            }
            catch (APICommunicationException e)
            {
                var response = context.Response;
                var result = new ErrorModel();
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                result.StatusCode = (int) HttpStatusCode.InternalServerError;
                result.ErrorMessage.Add("Erreur communication API XTB");
                await response.WriteAsync(result.ToString());
            }
        }
    }
}