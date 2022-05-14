using Business.AppCore.IServices;
using Common.Model.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure.Services
{
    internal class RouterService :IRouterService
    {
        private readonly CenterConfig _config;
        private readonly RouterConfig _routerConfig;
        public RouterService(
            CenterConfig config,
            RouterConfig routerConfig
            )
        {
            _config = config;
            _routerConfig = routerConfig;

        }
        public string GetEnvName(string version)
        {
            string env = null;
            if(!string.IsNullOrEmpty(version) && _routerConfig.VersionMapping.ContainsKey(version))
            {
                env = _routerConfig.VersionMapping[version];
            }
            else
            {
                env = _routerConfig.DefaultEnv;
            }
            return env;
        }
    }
}
