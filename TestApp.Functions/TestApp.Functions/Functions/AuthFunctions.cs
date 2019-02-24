using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestApp.Core.Auth.Interfaces;

namespace TestApp.Functions.Functions
{
    public class AuthFunctions : FunctionBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public AuthFunctions() : base()
        {
            this._userRepository = (IUserRepository)this._container.GetService(typeof(IUserRepository));
            this._tokenRepository = (ITokenRepository)this._container.GetService(typeof(ITokenRepository));
        }

        [FunctionName(nameof(GetToken))]
        public async Task<IActionResult> GetToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/token/get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(GetToken)} function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string username = data?.username;
            string password = data?.password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                this.ModelState.AddModelError("credentials", "Missing credentials");

                return this.BadRequest(ModelState);
            }

            if (!this._userRepository.ValidateCredentials(username, password).Result)
            {
                return this.Unauthorized();
            }

            return this.Ok(this._tokenRepository.Generate(username));
        }
    }
}
