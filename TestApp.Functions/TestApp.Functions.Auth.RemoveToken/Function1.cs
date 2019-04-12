using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestApp.Core.Auth.Interfaces;
using TestApp.Functions.Common;

namespace TestApp.Functions.Auth.RemoveToken
{
    public class Function1 : FunctionBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public Function1() : base()
        {
            this._userRepository = (IUserRepository)this.Container.GetService(typeof(IUserRepository));
            this._tokenRepository = (ITokenRepository)this.Container.GetService(typeof(ITokenRepository));
        }

        [FunctionName(nameof(RemoveToken))]
        public IActionResult RemoveToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/token/remove")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(RemoveToken)} function processed a request.");

            string token = req.Query["token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                this.ModelState.AddModelError("token", "Missing token");

                return this.BadRequest(ModelState);
            }

            var tokenData = this._tokenRepository.GetToken(token);

            if (tokenData == null)
            {
                this.ModelState.AddModelError("token", "Invalid token");

                return this.BadRequest(ModelState);
            }

            this._tokenRepository.Remove(token);

            return this.NoContent();
        }
    }
}
