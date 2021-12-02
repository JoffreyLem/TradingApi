using System;
using System.Net;
using System.Threading.Tasks;
using ApiTrading.Exception;
using Microsoft.AspNetCore.Http;
using XtbLibrairie.errors;
using XtbLibrairie.responses;

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
                    case UpdateException passwordUpdateException:
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


                if (err.ErrorCode.StringValue == ERR_CODE.LOGIN_NOT_FOUND.ToString())
                {
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    result.ErrorMessage.Add("Login/Password XTB Incorrect");
                }
                else if (err.ErrorCode.StringValue == "BE115")
                {
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    result.ErrorMessage.Add("Le symbole n'existe pas");
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
            catch (ArgumentNullException e)
            {
                var response = context.Response;
                var result = new ErrorModel();
                response.StatusCode = (int) HttpStatusCode.InternalServerError;

                result.ErrorMessage.Add("Erreur traitement interne");
                await response.WriteAsync(result.ToString());
            }
        }
    }
}