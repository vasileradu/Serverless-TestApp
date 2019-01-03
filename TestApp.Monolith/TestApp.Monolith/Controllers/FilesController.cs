using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.Core.FileStorage.Interfaces;

namespace TestApp.Monolith.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IDataRepository _fileRepository;

        public FilesController(IDataRepository fileRepository)
        {
            this._fileRepository = fileRepository;
        }

        [Route("upload")]
        [HttpPost]
        [ProducesResponseType(200)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await this._fileRepository.SaveFile(file.FileName, file.OpenReadStream());

            return this.Ok(new { filename = result });
        }
    }
}