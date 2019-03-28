using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using TestApp.Core.Models.Analysis;
using TestApp.Lambda.Common;
using TestApp.Lambda.Common.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TestApp.Lambda.Analysis.Compare
{
    public class Function : AnalysisFunctionBase
    {
        public Function() : base()
        {
        }

        public async Task<FunctionResult> FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            Console.WriteLine("CompareAnalysis function processed a request.");

            List<string> files = input.Files;
            int length = input.Length;
            string name = input.Name;

            if (files == null || !files.Any() || length <= 1)
            {
                return this.BadRequest(nameof(files));
            }

            var tasks = new List<Task<AnalysisResult>>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => this.RunAnalysis(file, length)));
            }

            Task.WaitAll(tasks.ToArray());

            var notFoundFiles = tasks.Where(t => t.Result.IsEmpty).Select(t => t.Result.Name);

            if (notFoundFiles.Any())
            {
                return this.NotFound(notFoundFiles);
            }

            // Aggregate results.
            var seed = tasks.First().Result;

            var result = await this.FileRepository.SaveComparisonResult(
                tasks.Aggregate(seed, (accumulator, t) => accumulator.Intersect(t.Result, name, length)),
                name);

            return this.Ok(new { filename = result });
        }
    }

    public class FunctionInput
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public List<string> Files { get; set; }
    }
}
