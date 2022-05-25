using Business.AppCore.IServices.IBusinessServices;
using Business.Infrastructure.Services.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure
{
    public static class BusinessServiceFactory
    {
        public static void ConfigurationBusinessService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITestService, TestService>();
        }
    }
}
