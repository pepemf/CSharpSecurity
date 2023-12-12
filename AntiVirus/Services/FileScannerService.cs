using System.IO;
using System.Security.Cryptography;

namespace AntiVirus.Services
{
    class FileScannerService : IFileScannerService
    {
        public IEnumerable<FileScanResult> ScanDirectoryWithProgress(string directoryPath, IEnumerable<string> database, Action<double> progressCallback)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("O diretório especificado não existe.");
            }

            var results = new List<FileScanResult>();
            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
            var totalFiles = files.Length;
            var processedFiles = 0;

            foreach (var filePath in files)
            {
                string hash = CalculateHash(filePath);

                if (database.Contains(hash))
                {
                    results.Add(new FileScanResult { FilePath = filePath, Hash = hash, IsMalware = true });
                }


                processedFiles++;
                var progress = (double)processedFiles / totalFiles * 100;
                progressCallback?.Invoke(progress);

            }
            return results;
        }

        private static string CalculateHash(string filePath)
        {
            using (var stream = new BufferedStream(File.OpenRead(filePath)))
            {
                using SHA256Managed sha = new();
                byte[] hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
