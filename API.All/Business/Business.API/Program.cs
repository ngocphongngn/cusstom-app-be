using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
       
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppsettings(typeof(Program), new string[]
                {
                    "Connections.json",
                    "AuthenToken.json"
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
    public static class ConfigApp
    {
        private const string ROOT_FOLDER = "ROOT_FOLDER";
        private const string _configFolderName = "Config";
        private static string _configFolder = null;
        private static string _rootFolder = null;
        public static IHostBuilder ConfigureAppsettings(this IHostBuilder builder, Type type, IEnumerable<string> files)
        {
            if (string.IsNullOrEmpty(_rootFolder))
            {
                string path = AppContext.BaseDirectory;
                string rootFolder = Environment.GetEnvironmentVariable(ROOT_FOLDER) ?? "";
                _rootFolder = Path.Combine(path, rootFolder);
            }
            if (string.IsNullOrEmpty(_configFolder))
            {
                _configFolder = Path.Combine(_rootFolder, _configFolderName);
            }
            foreach(var item in files)
            {
                var file = Path.Combine(_configFolder, item);
                if (File.Exists(file))
                {
                    builder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile(file);
                    });
                }
                else
                {
                    Console.WriteLine($"Not found: {file}.json");
                }
            }
            return builder;
        }
    }
}
