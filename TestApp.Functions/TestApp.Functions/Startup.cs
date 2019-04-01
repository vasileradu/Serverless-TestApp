using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TestApp.Core.Auth.Interfaces;
using TestApp.Core.Auth.Models;
using TestApp.Core.Auth.Repositories;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.FileStorage.Repositories;
using TestApp.Functions;

//[assembly: WebJobsStartup(typeof(Startup))]
namespace TestApp.Functions
{
    public class Startup //: IWebJobsStartup
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
            switch (this.GetEnvironmentType())
            {
                case "Development":
                    {
                        this.ConfigureDevelopmentServices(this._services);
                    }
                    break;
                case "NoDependencies":
                    {
                        this.ConfigureNoDependenciesServices(this._services);
                    }
                    break;
                default:
                case "Production":
                    {
                        this.ConfigureServices(this._services);
                    }
                    break;
            }

            return this;
        }

        // Automatically called by runtime, but only in conjunction with 'WebJobsStartup';
        // Not working at the moment.
        public void Configure(IWebJobsBuilder builder)
        {
            this.ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s =>
                new AzureFileShareRepository(
                    Environment.GetEnvironmentVariable("ConnectionStrings_Storage"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Root"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Uploads"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Reports")));
        }

        private void ConfigureDevelopmentServices(IServiceCollection services)
        {
            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s => new LocalFileShareRepository(
                Path.Combine(
                    Environment.GetEnvironmentVariable("Storage_Locations_Root"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Uploads")),
                Path.Combine(
                    Environment.GetEnvironmentVariable("Storage_Locations_Root"),
                    Environment.GetEnvironmentVariable("Storage_Locations_Reports"))));
        }

        private void ConfigureNoDependenciesServices(IServiceCollection services)
        {
            services.AddTransient<IDataRepository, MockFileRepository>();
            services.AddTransient<IUserRepository, MockRepository>();
            services.AddTransient<ITokenRepository, MockRepository>();
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddDbContext<TestAppAuthContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings_AuthDb")));
        }

        private string GetEnvironmentType()
        {
            return Environment.GetEnvironmentVariable("EnvironmentType") ?? "Development";
        }
    }
}
