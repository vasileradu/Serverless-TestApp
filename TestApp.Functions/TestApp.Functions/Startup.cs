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
            this.ConfigureServices(this._services);

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
                    Environment.GetEnvironmentVariable("Storage:Locations:ConnectionString"),
                    Environment.GetEnvironmentVariable("Storage:Locations:Root"),
                    Environment.GetEnvironmentVariable("Storage:Locations:Uploads"),
                    Environment.GetEnvironmentVariable("Storage:Locations:Reports")));
        }

        private void ConfigureDevelopmentServices(IServiceCollection services)
        {
            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s => new LocalFileShareRepository(
                Path.Combine(
                    Environment.GetEnvironmentVariable("Storage:Locations:Root"),
                    Environment.GetEnvironmentVariable("Storage:Locations:Uploads")),
                Path.Combine(
                    Environment.GetEnvironmentVariable("Storage:Locations:Root"),
                    Environment.GetEnvironmentVariable("Storage:Locations:Reports"))));
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddDbContext<TestAppAuthContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:AuthDb")));
        }
    }
}
