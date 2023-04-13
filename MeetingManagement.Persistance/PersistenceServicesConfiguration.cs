﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingManagement.Persistance
{
    public static class PersistenceServicesConfiguration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var dbSettings = configuration.GetSection("MongoDatabase");
            services.Configure<MongoDbSettings>(dbSettings);

            return services;
        }
    }
}