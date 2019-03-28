using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;

namespace TestApp.Lambda.Common
{
    public abstract class AnalysisFunctionBase : FunctionBase
    {
        protected IDataRepository FileRepository { get; }
        private readonly int _minSequenceLength;

        protected AnalysisFunctionBase() : base()
        {
            this.FileRepository = (IDataRepository)this.Container.GetService(typeof(IDataRepository));
            this._minSequenceLength = 2;
        }

        /// <summary>
        /// Finds all sequences from <paramref name="source"/> of length <paramref name="length"/>;
        /// </summary>
        protected AnalysisResult FindSequences(string source, int length)
        {
            var result = new AnalysisResult();

            for (int startIndex = 0; startIndex <= source.Length - length; startIndex++)
            {
                var sequence = source.Substring(startIndex, length);

                result.Add(sequence, startIndex);
            }

            return result;
        }

        protected AnalysisResult RunAnalysis(string fileName, int length)
        {
            var result = new AnalysisResult(fileName, length);
            var rawData = this.FileRepository.GetRawData(fileName).Result;

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

    }
}
