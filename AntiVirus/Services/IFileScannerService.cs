namespace AntiVirus.Services
{
    public interface IFileScannerService
    {
        IEnumerable<FileScanResult> ScanDirectoryWithProgress(string directoryPath, IEnumerable<string> database, Action<double> progressCallback);
    }

    public class FileScanResult
    {
        public string FilePath { get; set; }
        public string Hash { get; set; }
        public bool IsMalware { get; internal set; }
    }
}