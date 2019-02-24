using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TestApp.Core.FileStorage.Interfaces;

namespace TestApp.Functions.Functions
{
    public class ReportFunctions : FunctionBase
    {
        private readonly IDataRepository _dataRepository;

        public ReportFunctions() : base()
        {
            this._dataRepository = (IDataRepository)this.Container.GetService(typeof(IDataRepository));            
        }

        [FunctionName(nameof(GetReport))]
        public async Task<IActionResult> GetReport(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "report/get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(GetReport)} function processed a request.");

            string name = req.Query["name"];

            var result = await this._dataRepository.GetAnalysisResult(name);

            if (result.IsEmpty)
            {
                return this.NotFound(name);
            }

            return this.Ok(result);
        }
    }
}
