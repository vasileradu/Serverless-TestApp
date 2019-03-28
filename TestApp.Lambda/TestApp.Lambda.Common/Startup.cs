using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TestApp.Core.Auth.Interfaces;
using TestApp.Core.Auth.Models;
using TestApp.Core.Auth.Repositories;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.FileStorage.Repositories;

namespace TestApp.Lambda
{
    public class Startup
    {
        private readonly IServiceCollection _services;

        public Startup()
        {
            this._services = new ServiceCollection();
        }

        public IServiceProvider Build()
        {
            return this._services.BuildServiceProvider();
        }

        public Startup Configure()
        {
            if (this.IsDevelopment())
            {
                this.ConfigureDevelopmentServices(this._services);                
            }
            else
            {
                this.ConfigureServices(this._services);
            }

            return this;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s =>
                new AmazonFileShareRepository(
                    Environment.GetEnvironmentVariable("AccessKey_Storage"),
                    Environment.GetEnvironmentVariable("SecretKey_Storage"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Root"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Uploads"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Reports")));
        }

        private void ConfigureDevelopmentServices(IServiceCollection services)
        {
            this.LoadDevelopmentEnvironmentVariables();

            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s => new LocalFileShareRepository(
                Path.Combine(
                    Environment.GetEnvironmentVariable("Storage_Locations_Root"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Uploads")),
                Path.Combine(
                    Environment.GetEnvironmentVariable("Storage_Locations_Root"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Reports"))));
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddDbContext<TestAppAuthContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings_AuthDb")));
        }

        private bool IsDevelopment()
        {
            string envType = Environment.GetEnvironmentVariable("EnvironmentType") ?? "Development";

            return envType.Equals("Development");
        }

        // Load variables from appsettings.json;
        private void LoadDevelopmentEnvironmentVariables()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Environment.SetEnvironmentVariable("Storage_Locations_Root", config["Values:Storage_Locations_Root"]);
            Environment.SetEnvironmentVariable("Storage_Locations_Uploads", config["Values:Storage_Locations_Uploads"]);
            Environment.SetEnvironmentVariable("Storage_Locations_Reports", config["Values:Storage_Locations_Reports"]);
            Environment.SetEnvironmentVariable("AccessKey_Storage", config["Values:AccessKey_Storage"]);
            Environment.SetEnvironmentVariable("SecretKey_Storage", config["Values:SecretKey_Storage"]);
            Environment.SetEnvironmentVariable("ConnectionStrings_AuthDb", config["Values:ConnectionStrings_AuthDb"]);
            Environment.SetEnvironmentVariable("EnvironmentType", config["Values:EnvironmentType"]);
        }
    }
}
