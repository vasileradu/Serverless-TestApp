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

        [FunctionName(nameof(CreateAnalysis))]
        public async Task<IActionResult> CreateAnalysis(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "analysis/create")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(CreateAnalysis)} function processed a request.");

            string name = req.Query["name"];
            int length = int.Parse(req.Query["length"]);

            var analysis = this.RunAnalysis(name, length);

            if (analysis.IsEmpty)
            {
                return NotFound(name);
            }

            var result = await this._fileRepository.SaveAnalysisResult(analysis, name);

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
