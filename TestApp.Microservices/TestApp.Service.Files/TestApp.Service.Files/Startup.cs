using System.IO;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IDataRepository>(s => new LocalFileShareRepository(
                Path.Combine(this.Configuration["Storage:Locations:Root"],
                    this.Configuration["Storage:Locations:Uploads"]),
                Path.Combine(this.Configuration["Storage:Locations:Root"],
                    this.Configuration["Storage:Locations:Reports"])));

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = this.GetConfigToInt("Upload:SizeLimitBytes");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new
                        {
                            context.Response.StatusCode,
                            Message = "Internal Server Error.",
                            StackTrace = contextFeature.Error.ToString()
                        }.ToString());
                    }

                });
            });

            app.UseMvc();
        }

        private int GetConfigToInt(string key, int defaultValue = 0)
        {
            if (!int.TryParse(this.Configuration[key], out int result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
