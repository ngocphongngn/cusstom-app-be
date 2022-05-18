using Common.Utility.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility
{
    public static class UtilityFactory
    {
        public static void ConfigurationUtilityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IParamService, ParamService>();
            services.AddSingleton<ITypeService, TypeService>();
        }
    }
}
