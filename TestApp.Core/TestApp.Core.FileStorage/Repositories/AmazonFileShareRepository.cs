using Amazon.S3;
using Amazon.S3.Transfer;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TestApp.Core.FileStorage.Interfaces;
using TestApp.Core.Models.Analysis;

namespace TestApp.Core.FileStorage.Repositories
{
    public class AmazonFileShareRepository : IDataRepository
    {
        private readonly IAmazonS3 _s3client;
        private string _bucketName;
        private string _uploadsPath;
        private string _reportsPath;

        public AmazonFileShareRepository(
            string accessKey,
            string secretKey,
            string bucketName,
            string uploads,
            string reports)
        {
            this._s3client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.EUCentral1);

            this._bucketName = bucketName;
            this._uploadsPath = uploads;
            this._reportsPath = reports;
        }

        #region IDataRepository Members

        public Task<AnalysisResult> GetAnalysisResult(string name)
        {
            var result = new AnalysisResult();

            using (var transferUtility = new TransferUtility(this._s3client))
            using (var reader = new StreamReader(
                transferUtility.OpenStream(this._bucketName, this.FormatReportsPath(name))))
            {
                var readTask = reader.ReadToEndAsync();
                Task.WaitAll(readTask);

                result = JsonConvert.DeserializeObject<AnalysisResult>(readTask.Result);
            }

            return Task.FromResult(result);
        }

        public Task<string> GetRawData(string name)
        {
            using (var transferUtility = new TransferUtility(this._s3client))
            using (var reader = new StreamReader(
                transferUtility.OpenStream(this._bucketName, this.FormatUploadsPath(name))))
            {
                return Task.FromResult(reader.ReadToEnd());
            }
        }

        public Task<string> SaveAnalysisResult(AnalysisResult result, string name)
        {
            return this.SaveResult(result, $"analysis_{name}_{Guid.NewGuid()}.json");
        }

        public Task<string> SaveComparisonResult(AnalysisResult result, string name)
        {
            return this.SaveResult(result, $"comparison_{name}_{Guid.NewGuid()}.json");
        }

        public Task<string> SaveFile(string name, Stream stream)
        {
            string fileName = $"upload_{Guid.NewGuid()}_{name}";

            using (var transferUtility = new TransferUtility(this._s3client))
            using (stream)
            {
                Task.WaitAll(transferUtility.UploadAsync(
                    stream,
                    this._bucketName,
                    this.FormatUploadsPath(fileName)));
            }

            return Task.FromResult(fileName);
        }

        #endregion

        #region Private Members

        private string FormatUploadsPath(string fileName)
        {
            return $"{this._uploadsPath}/{fileName}";
        }

        private string FormatReportsPath(string fileName)
        {
            return $"{this._reportsPath}/{fileName}";
        }

        private Task<string> SaveResult(AnalysisResult result, string fileName)
        {
            var content = JsonConvert.SerializeObject(result);

            using (var transferUtility = new TransferUtility(this._s3client))
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content)))
            {
                Task.WaitAll(transferUtility.UploadAsync(
                    stream,
                    this._bucketName,
                    this.FormatReportsPath(fileName)));
            }

            return Task.FromResult(fileName);
        }

        #endregion
    }
}
