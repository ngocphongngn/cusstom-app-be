using Business.AppCore.IServices;
using Common.Model.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;


namespace Business.Infrastructure.Services
{
    public static class ServiceConfiguration
    {
        public static TConfig ConfigurationCenterService<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : CenterConfig
        {
            var config = Activator.CreateInstance<TConfig>();
            new ConfigureFromConfigurationOptions<TConfig>(configuration).Configure(config);
            services.AddSingleton(config);
            services.AddSingleton((CenterConfig)config);
            return config;
        }
        public static void ConfigurationRouterService(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("Router").Get<RouterConfig>();
            services.AddSingleton(config);
            services.AddSingleton<IRouterService, RouterService>();
        }

    }
}
