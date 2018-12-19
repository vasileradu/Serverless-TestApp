using System.Collections.Generic;
using System.Linq;

namespace TestApp.Core.Models.Analysis
{
    public class AnalysisResult
    {
        public string Name { get; }
        public int Length { get; }
        public bool IsEmpty => this.Sequences.Count == 0;
        public Dictionary<string, List<long>> Sequences { get; }
        
        public AnalysisResult()
        {
            this.Sequences = new Dictionary<string, List<long>>();
            this.Name = string.Empty;
            this.Length = 0;
        }

        public AnalysisResult(string name, int length)
        : this()
        {
            this.Name = name;
            this.Length = length;
        }

        public void Add(string sequence, long position)
        {
            if (!this.Sequences.ContainsKey(sequence))
            {
                this.Sequences.Add(sequence, new List<long>());
            }

            this.Sequences[sequence].Add(position);
        }

        /// <summary>
        /// Adds <paramref name="result"/> to current instance, without checking for duplicates (key overlaps).
        /// </summary>
        public AnalysisResult Append(AnalysisResult result)
        {
            foreach (var resultSequence in result.Sequences)
            {
                this.Sequences.Add(resultSequence.Key, resultSequence.Value);
            }

            return this;
        }

        /// <summary>
        /// Creates a new <see cref="AnalysisResult"/> with the common sequences and their position identical overlaps.
        /// </summary>
        public AnalysisResult Intersect(AnalysisResult otherResult)
        {
            var result = new AnalysisResult();

            foreach (var sequence in this.Sequences)
            {
                if (otherResult.Sequences.ContainsKey(sequence.Key))
                {
                    var overlaps = sequence.Value.Intersect(otherResult.Sequences[sequence.Key]).ToList();

                    result.Sequences.Add(sequence.Key, overlaps);
                }
            }

            return result;
        }
    }
}
