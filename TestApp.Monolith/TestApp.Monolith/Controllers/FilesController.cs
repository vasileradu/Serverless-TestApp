using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.Core.FileStorage.Interfaces;

namespace TestApp.Monolith.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private IDataRepository _fileRepository;

        public FilesController(IDataRepository fileRepository)
        {
            this._fileRepository = fileRepository;
        }

        [Route("upload")]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Upload()
        {
            return this.Ok();
        }
    }
}