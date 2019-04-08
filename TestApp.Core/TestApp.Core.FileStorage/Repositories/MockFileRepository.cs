using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;

namespace TestApp.Core.FileStorage.Repositories
{
    public class MockFileRepository : IDataRepository
    {
        public Task<AnalysisResult> GetAnalysisResult(string name)
        {
            var dummyData = new AnalysisResult(name, 10);

            dummyData.Add(name, 1);

            return Task.FromResult(dummyData);
        }

        public Task<string> GetRawData(string name)
        {
            return Task.FromResult("ABCDEFGHIJKLMN");
        }

        public Task<string> SaveAnalysisResult(AnalysisResult result, string name)
        {
            var content = JsonConvert.SerializeObject(result);

            return Task.FromResult($"analysis_{name}_{Guid.NewGuid()}.json");
        }

        public Task<string> SaveComparisonResult(AnalysisResult result, string name)
        {
            var content = JsonConvert.SerializeObject(result);

            return Task.FromResult($"comparison_{name}_{Guid.NewGuid()}.json");
        }

        public Task<string> SaveFile(string name, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var readTask = reader.ReadToEndAsync();

                Task.WaitAll(readTask);
            }

            return Task.FromResult($"upload_{Guid.NewGuid()}_{name}");
        }
    }
}
