using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using TestApp.Core.Common.Extensions;

namespace TestApp.Monolith
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args).UseCliUrls(args).UseStartup<Startup>();
    }
}
