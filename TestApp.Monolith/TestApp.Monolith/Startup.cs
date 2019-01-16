using System.IO;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestApp.Core.Auth.Interfaces;
using TestApp.Core.Auth.Models;
using TestApp.Core.Auth.Repositories;
using TestApp.Core.Common.Extensions;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.FileStorage.Repositories;

namespace TestApp.Monolith
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s =>
                new AzureFileShareRepository(
                    this.Configuration["Storage:Locations:ConnectionString"],
                    this.Configuration["Storage:Locations:Root"],
                    this.Configuration["Storage:Locations:Uploads"],
                    this.Configuration["Storage:Locations:Reports"]));
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            this.ConfigureCommonServices(services);

            services.AddTransient<IDataRepository>(s => new LocalFileShareRepository(
                Path.Combine(this.Configuration["Storage:Locations:Root"],
                    this.Configuration["Storage:Locations:Uploads"]),
                Path.Combine(this.Configuration["Storage:Locations:Root"],
                    this.Configuration["Storage:Locations:Reports"])));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = this.Configuration.GetIntConfig("Upload:SizeLimitBytes");
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddDbContext<TestAppAuthContext>(options =>
                options.UseSqlServer(this.Configuration["ConnectionStrings:AuthDb"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGlobalExceptionHandler().UseMvc();
        }
    }
}
