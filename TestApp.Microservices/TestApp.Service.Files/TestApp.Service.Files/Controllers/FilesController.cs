using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestApp.Core.FileStorage.Interfaces;

namespace TestApp.Service.Files.Controllers
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