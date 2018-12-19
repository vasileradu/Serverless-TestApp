using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;

namespace TestApp.Core.FileStorage.Repositories
{
    /// <summary>
    /// Local (on-premise) file share wrapper.
    /// </summary>
    public class LocalFileShareRepository : IDataRepository
    {
        private readonly Dictionary<string, object> _analysisStore;
        private readonly Dictionary<string, object> _comparisonStore;
        private readonly string _rootPath;

        public LocalFileShareRepository(string rootPath)
        {
            this._analysisStore = new Dictionary<string, object>();
            this._comparisonStore = new Dictionary<string, object>();
            this._rootPath = rootPath;
        }

        public Task<AnalysisResult> GetAnalysisResult(string name)
        {
            var result = new AnalysisResult();

            if (this._analysisStore.ContainsKey(name))
            {
                result = this._analysisStore[name] as AnalysisResult;
            }

            return Task.FromResult(result);
        }

        public Task<string> GetRawData(string name)
        {
            return File.ReadAllTextAsync(Path.Combine(this._rootPath, name));
        }

        public Task<string> SaveAnalysisResult(AnalysisResult result, string name)
        {
            var fileName = $"analysis_{name}_{result.Sequences.Count}.json";

            if (this._analysisStore.ContainsKey(fileName))
            {
                this._analysisStore[fileName] = result;
            }
            else
            {
                this._analysisStore.Add(fileName, result);
            }

            return Task.FromResult(fileName);
        }

        public Task<string> SaveComparisonResult(AnalysisResult result, string name)
        {
            var fileName = $"comparison_{name}_{result.Sequences.Count}.json";

            if (this._comparisonStore.ContainsKey(fileName))
            {
                this._comparisonStore[fileName] = result;
            }
            else
            {
                this._comparisonStore.Add(fileName, result);
            }

            return Task.FromResult(fileName);
        }
    }
}
