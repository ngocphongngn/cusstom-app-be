using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.API.Config;
using System.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using Business.Infrastructure.Services;
using Business.Infrastructure;
using Common.Utility;
using Microsoft.OpenApi.Models;

namespace Business.API
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var apiConfig = services.ConfigurationCenterService<ApiConfig>(Configuration);
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //.AddNewtonsoftJson(options =>
            //   options.SerializerSettings.DateFormatHandling = Converter.JSon
            //);
            services.ConfigurationBusinessService(Configuration);
            services.ConfigurationUtilityService(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    //builder => builder.WithOrigins(apiConfig.Cors).AllowAnyHeader().AllowAnyMethod());
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Job Seeking Swagger", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiConfig apiConfig, ILogger<Startup> log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MISA Mintasx Service version 1");
                c.RoutePrefix = "swagger";
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
