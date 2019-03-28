using System;

using Amazon.Lambda.Core;
using TestApp.Core.Auth.Interfaces;
using TestApp.Lambda.Common;
using TestApp.Lambda.Common.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TestApp.Lambda.Auth.GetToken
{
    public class Function : FunctionBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public Function() : base()
        {
            this._userRepository = (IUserRepository)this.Container.GetService(typeof(IUserRepository));
            this._tokenRepository = (ITokenRepository)this.Container.GetService(typeof(ITokenRepository));
        }
        
        public FunctionResult FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            Console.WriteLine("GetToken function processed a request.");

            string username = input?.Username;
            string password = input?.Password;

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

    public class FunctionInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
