using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using TestApp.Core.Auth.Interfaces;
using TestApp.Lambda.Common;
using TestApp.Lambda.Common.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TestApp.Lambda.Auth.RemoveToken
{
    public class Function : FunctionBase
    {
        private readonly ITokenRepository _tokenRepository;

        public Function() : base()
        {
            this._tokenRepository = (ITokenRepository)this.Container.GetService(typeof(ITokenRepository));
        }

        public FunctionResult FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            Console.WriteLine("RemoveToken function processed a request.");

            string token = input.Token;

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

        public class FunctionInput
        {
            public string Token { get; set; }            
        }
    }
}
