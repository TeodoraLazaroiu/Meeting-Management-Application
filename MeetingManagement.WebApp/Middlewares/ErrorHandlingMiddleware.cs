using MeetingManagement.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MeetingManagement.WebApp.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string errorResponse = "The server encountered an unexpected error.";

            switch(exception)
            {
                case UserNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = "The user was not found";
                    break;
                case UserAlreadyExistsException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse = "User with this email already exists";
                    break;
                case UserInvalidCredentialsException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse = "Invalid user credentials";
                    break;
                case UserInsufficientPermissionsException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse = exception.Message;
                    break;
                case TeamNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = "The team was not found";
                    break;
                case TeamDeletionException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = exception.Message;
                    break;
                case EventValidationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = exception.Message;
                    break;
                case EventNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = "The event was not found";
                    break;
                case EventDeletionException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = exception.Message;
                    break;
                case ResponseNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = "The response was not found";
                    break;
                case ResponseValidationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = "The response is not valid";
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
