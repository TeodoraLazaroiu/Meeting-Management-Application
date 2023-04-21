using MeetingManagement.WebApp.Middlewares;

namespace MeetingManagement.WebApp.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
