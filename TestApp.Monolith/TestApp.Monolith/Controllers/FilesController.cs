using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
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
        [Consumes("multipart/form-data")]
        public IActionResult Upload(IFormFile file)
        {
            return this.Ok(this._fileRepository.SaveFile(file.FileName, file.OpenReadStream()));
        }
    }
}