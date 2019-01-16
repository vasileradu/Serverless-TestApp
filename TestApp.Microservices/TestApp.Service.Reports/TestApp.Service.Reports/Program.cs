using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TestApp.Service.Reports
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var hostUrl = config["hosturl"];

            var builder = WebHost.CreateDefaultBuilder(args);

            if (!string.IsNullOrEmpty(hostUrl))
            {
                builder = builder.UseUrls(hostUrl);
            }

            return builder.UseStartup<Startup>();
        }

    }
}
