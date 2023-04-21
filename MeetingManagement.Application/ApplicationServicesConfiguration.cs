using MeetingManagement.Application.Interfaces;
using MeetingManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingManagement.Application
{
    public static class ApplicationServicesConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
