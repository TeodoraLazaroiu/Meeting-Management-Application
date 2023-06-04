using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;
using MeetingManagement.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingManagement.Persistance
{
    public static class PersistenceServicesConfiguration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var dbSettings = configuration.GetSection("MongoDatabase");
            services.Configure<MongoDbSettings>(dbSettings);

            services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();
            services.AddScoped<IRecurringPatternRepository, RecurringPatternRepository>();

            return services;
        }
    }
}
