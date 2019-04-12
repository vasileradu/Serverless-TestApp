using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;
using System.Collections.Generic;
using System.Linq;
using TestApp.Functions.Common;

namespace TestApp.Functions.Analysis.Compare
{
    public class Function1 : FunctionBase
    {
        private readonly IDataRepository _fileRepository;
        private readonly int _minSequenceLength;

        public Function1() : base()
        {
            this._fileRepository = (IDataRepository)this.Container.GetService(typeof(IDataRepository));
            this._minSequenceLength = 2;
        }

        [FunctionName(nameof(CompareAnalysis))]
        public async Task<IActionResult> CompareAnalysis(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "analysis/compare")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(CompareAnalysis)} function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            List<string> files = data["files"].ToObject<List<string>>();
            int length = data["length"].ToObject<int>();
            string name = data["name"].ToObject<string>();

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

            var result = await this._fileRepository.SaveComparisonResult(
                tasks.Aggregate(seed, (accumulator, t) => accumulator.Intersect(t.Result, name, length)),
                name);

            return this.Ok(new { filename = result });
        }

        #region Private Methods

        /// <summary>
        /// Finds all sequences from <paramref name="source"/> of length <paramref name="length"/>;
        /// </summary>
        private AnalysisResult FindSequences(string source, int length)
        {
            var result = new AnalysisResult();

            for (int startIndex = 0; startIndex <= source.Length - length; startIndex++)
            {
                var sequence = source.Substring(startIndex, length);

                result.Add(sequence, startIndex);
            }

            return result;
        }

        private AnalysisResult RunAnalysis(string fileName, int length)
        {
            var result = new AnalysisResult(fileName, length);
            var rawData = this._fileRepository.GetRawData(fileName).Result;

            if (!string.IsNullOrWhiteSpace(rawData))
            {
                var tasks = new List<Task<AnalysisResult>>();

                // Find sequences for all possible seq-lengths.
                for (int seqLength = _minSequenceLength; seqLength <= length; seqLength++)
                {
                    var seqLengthCopy = seqLength;
                    tasks.Add(Task.Run(() => this.FindSequences(rawData, seqLengthCopy)));
                }

                Task.WaitAll(tasks.ToArray());

                result = tasks.Aggregate(new AnalysisResult(fileName, length), (accumulator, t) => accumulator.Append(t.Result));
            }

            return result;
        }

        #endregion
    }
}
