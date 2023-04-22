﻿using MeetingManagement.Application.Exceptions;
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
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}