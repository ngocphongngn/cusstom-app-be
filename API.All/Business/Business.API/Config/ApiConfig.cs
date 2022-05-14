using Common.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Config
{
    public class ApiConfig : CenterConfig
    {
        public SwaggerConfig Swagger { get; set; }

    }
}
