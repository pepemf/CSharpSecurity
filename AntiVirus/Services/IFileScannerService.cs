namespace AntiVirus.Services
{
    // Interface para o serviço de varredura de arquivos
    public interface IFileScannerService
    {
        // Método para varrer um diretório em busca de malware com feedback de progresso
        IEnumerable<FileScanResult> ScanDirectoryWithProgress(string directoryPath, IEnumerable<string> database, Action<double> progressCallback);
    }

    // Classe que representa o resultado da varredura de um arquivo
    public class FileScanResult
    {
        // Caminho do arquivo escaneado
        public string FilePath { get; set; }

        // Hash do arquivo escaneado
        public string Hash { get; set; }

        // Indica se o arquivo é considerado malware (interno para configuração)
        public bool IsMalware { get; internal set; }
    }
}
