using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Lambda.Common;
using TestApp.Lambda.Common.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TestApp.Lambda.Files.Upload
{
    public class Function : FunctionBase
    {
        private readonly IDataRepository _fileRepository;

        public Function() : base()
        {
            this._fileRepository = (IDataRepository)this.Container.GetService(typeof(IDataRepository));
        }

        public async Task<FunctionResult> FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            Console.WriteLine("Upload function processed a request.");

            using(var stream = new MemoryStream(Encoding.ASCII.GetBytes(input.Body)))
            {
                var result = await this._fileRepository.SaveFile("awsLambda", stream);

                return this.Ok(new { filename = result });
            }            
        }
    }

    public class FunctionInput
    {
        public string Body { get; set; }
    }
}
