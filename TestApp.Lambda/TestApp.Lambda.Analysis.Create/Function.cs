using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using TestApp.Lambda.Common;
using TestApp.Lambda.Common.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TestApp.Lambda.Analysis.Create
{
    public class Function : AnalysisFunctionBase
    {
        public Function() : base()
        {
        }
        
        public async Task<FunctionResult> FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            Console.WriteLine("CreateAnalysis function processed a request.");

            string name = input.Name;
            int length = input.Length;

            var analysis = this.RunAnalysis(name, length);

            if (analysis.IsEmpty)
            {
                return NotFound(name);
            }

            var result = await this.FileRepository.SaveAnalysisResult(analysis, name);

            return this.Ok(new { filename = result });
        }
    }

    public class FunctionInput
    {
        public string Name { get; set; }
        public int Length { get; set; }
    }
}
