using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestApp.Core.Common.Extensions;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.FileStorage.Repositories;

namespace TestApp.Service.Files
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

        public void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = this.Configuration.GetIntConfig("Upload:SizeLimitBytes");
            });
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
