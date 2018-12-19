using System.IO;
using System.Threading.Tasks;
using TestApp.Core.Models.Analysis;

namespace TestApp.Core.FileStorage.Interfaces
{
    public interface IDataRepository
    {
        Task<AnalysisResult> GetAnalysisResult(string name);

        Task<string> GetRawData(string name);

        Task<string> SaveAnalysisResult(AnalysisResult result, string name);

        Task<string> SaveComparisonResult(AnalysisResult result, string name);

        bool SaveFile(string name, Stream stream);
    }
}
