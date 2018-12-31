using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TestApp.Core.Auth.Interfaces;

namespace TestApp.Service.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly TimeSpan _tokenExpirationInterval;

        public AuthController(
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IConfiguration configuration)
        {
            this._userRepository = userRepository;
            this._tokenRepository = tokenRepository;

            int.TryParse(configuration["Auth:TokenExpirationSeconds"], out int expirationSeconds);

            this._tokenExpirationInterval = new TimeSpan(0, 0, expirationSeconds);
        }

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

            if (!this._userRepository.ValidateCredentials(username, password).Result)
            {
                return this.Unauthorized();
            }

            return this.Ok(this._tokenRepository.Generate(username));
        }

        [Route("token/validate")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ValidateToken([FromQuery]string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("token", "Missing token");

                return BadRequest(ModelState);
            }

            var tokenData = this._tokenRepository.GetToken(token);

            if (tokenData == null
                || DateTime.UtcNow - tokenData.CreatedAtUtc > this._tokenExpirationInterval)
            {
                return this.Unauthorized();
            }

            return this.Ok(tokenData);
        }

        [Route("token/remove")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult RemoveToken([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("token", "Missing token");

                return BadRequest(ModelState);
            }

            var tokenData = this._tokenRepository.GetToken(token);

            if (tokenData == null)
            {
                ModelState.AddModelError("token", "Invalid token");

                return BadRequest(ModelState);
            }

            this._tokenRepository.Remove(token);

            return this.NoContent();
        }
    }
}