using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Functions.Common;

namespace TestApp.Functions.Files.Upload
{
    public class Function1 : FunctionBase
    {
        private readonly IDataRepository _fileRepository;

        public Function1() : base()
        {
            this._fileRepository = (IDataRepository)this.Container.GetService(typeof(IDataRepository));
        }

        [FunctionName(nameof(Upload))]
        public async Task<IActionResult> Upload(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "files/upload")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(Upload)} function processed a request.");

            var result = await this._fileRepository.SaveFile("azFunction", req.Body);

            return this.Ok(new { filename = result });
        }
    }
}
