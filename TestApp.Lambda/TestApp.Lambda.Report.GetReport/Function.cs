using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Lambda.Common;
using TestApp.Lambda.Common.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TestApp.Lambda.Report.GetReport
{
    public class Function : FunctionBase
    {
        private readonly IDataRepository _dataRepository;

        public Function() : base()
        {
            this._dataRepository = (IDataRepository)this.Container.GetService(typeof(IDataRepository));
        }
        
        public async Task<FunctionResult> FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            Console.WriteLine("GetReport function processed a request.");

            string name = input.Name;

            var result = await this._dataRepository.GetAnalysisResult(name);

            if (result.IsEmpty)
            {
                return this.NotFound(name);
            }

            return this.Ok(result);
        }
    }

    public class FunctionInput
    {
        public string Name { get; set; }
    }
}
