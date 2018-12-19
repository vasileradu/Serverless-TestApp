using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace TestApp.Monolith.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private bool isAuthenticated = true;
        private bool isValidToken = true;

        [Route("token/get")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult GetToken([FromBody]JObject credentials)
        {
            string username = credentials["username"].ToObject<string>();
            string password = credentials["password"].ToObject<string>();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("credentials", "Missing credentials");

                return this.BadRequest(ModelState);
            }

            if (!this.isAuthenticated)
            {
                return this.Unauthorized();
            }

            return this.Ok("token");
        }

        [Route("token/validate")]
        [HttpGet]
        [ProducesResponseType(203)]
        [ProducesResponseType(400)]
        public IActionResult ValidateToken([FromQuery]string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("token", "Missing token");

                return BadRequest(ModelState);
            }

            return this.Ok(this.isValidToken); // call to repo.
        }
    }
}
