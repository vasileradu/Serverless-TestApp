using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Core.FileStorage.Interfaces;

namespace TestApp.Service.Reports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;

        public ReportController(IDataRepository dataRepository)
        {
            this._dataRepository = dataRepository;
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string name)
        {
            var result = await this._dataRepository.GetAnalysisResult(name);

            if (result.IsEmpty)
            {
                return this.NotFound(name);
            }

            return this.Ok(result);
        }
    }
}