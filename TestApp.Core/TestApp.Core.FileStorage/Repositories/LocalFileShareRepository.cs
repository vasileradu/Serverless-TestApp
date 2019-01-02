using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;

namespace TestApp.Core.FileStorage.Repositories
{
    /// <summary>
    /// Local (on-premise) file share wrapper.
    /// </summary>
    public class LocalFileShareRepository : IDataRepository
    {
        private readonly string _uploadsPath;
        private readonly string _reportsPath;

        public LocalFileShareRepository(string uploadsPath, string reportsPath)
        {
            this._uploadsPath = uploadsPath;
            this._reportsPath = reportsPath;
        }

        #region IDataRepository Members

        public Task<AnalysisResult> GetAnalysisResult(string name)
        {
            var result = new AnalysisResult();

            var path = Path.Combine(this._reportsPath, name);

            if (File.Exists(path))
            {
                var readTask = File.ReadAllTextAsync(path);
                Task.WaitAll(readTask);

                result = JsonConvert.DeserializeObject<AnalysisResult>(readTask.Result);
            }

            return Task.FromResult(result);
        }

        public Task<string> GetRawData(string name)
        {
            return File.ReadAllTextAsync(Path.Combine(this._uploadsPath, name));
        }

        public Task<string> SaveAnalysisResult(AnalysisResult result, string name)
        {
            var fileName = $"analysis_{name}_{Guid.NewGuid()}.json";

            Task.WaitAll(this.SaveResult(result, fileName));

            return Task.FromResult(fileName);
        }

        public Task<string> SaveComparisonResult(AnalysisResult result, string name)
        {
            var fileName = $"comparison_{name}_{Guid.NewGuid()}.json";

            Task.WaitAll(this.SaveResult(result, fileName));

            return Task.FromResult(fileName);
        }

        public bool SaveFile(string name, Stream stream)
        {
            string path = Path.Combine(this._uploadsPath, $"upload_{Guid.NewGuid()}_{name}");

            using (stream)
            using (var output = File.Open(path, FileMode.Create))
            {
                Task.WaitAll(stream.CopyToAsync(output));
            }

            return true;
        }

        #endregion

        #region Private Methods

        private Task SaveResult(AnalysisResult result, string fileName)
        {
            var content = JsonConvert.SerializeObject(result);

            return File.WriteAllTextAsync(Path.Combine(this._reportsPath, fileName), content);
        }

        #endregion
    }
}
