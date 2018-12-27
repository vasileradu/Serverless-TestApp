using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;

namespace TestApp.Service.Analysis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IDataRepository _fileRepository;
        private readonly int _minSequenceLength;

        public AnalysisController(IDataRepository repository)
        {
            this._fileRepository = repository;
            this._minSequenceLength = 2;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery]string name, [FromQuery]int length)
        {
            var analysis = this.RunAnalysis(name, length);

            if (analysis.IsEmpty)
            {
                return NotFound(name);
            }

            var result = await this._fileRepository.SaveAnalysisResult(analysis, name);

            return this.Ok(new { filename = result });
        }

        [Route("compare")]
        [HttpPost]
        public async Task<IActionResult> Compare([FromBody]JObject data)
        {
            var files = data["files"].ToObject<List<string>>();
            var length = data["length"].ToObject<int>();
            var name = data["name"].ToObject<string>();

            if (files == null || !files.Any() || length <= 1)
            {
                return this.BadRequest();
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