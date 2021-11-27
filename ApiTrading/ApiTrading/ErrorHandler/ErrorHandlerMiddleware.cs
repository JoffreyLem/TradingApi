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
                    case AppException appException:
                    case TimeFrameDontExistException timeFrameDontExistException:
                    case PasswordUpdateException passwordUpdateException:
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

                
                if (err.ErrorCode.ToString() == ERR_CODE.LOGIN_NOT_FOUND.ToString())
                {
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    result.ErrorMessage.Add("Login/Password XTB Incorrect");
                }
                else
                {
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    result.ErrorMessage.Add("Erreur de communication avec l'API XTB");
                }
               
                
               
               
                await response.WriteAsync(result.ToString());
            }
            catch (APICommunicationException e)
            {
                var response = context.Response;
                var result = new ErrorModel();
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
          
                result.ErrorMessage.Add("Erreur communication API XTB");
                await response.WriteAsync(result.ToString());
            }
        }
    }
}