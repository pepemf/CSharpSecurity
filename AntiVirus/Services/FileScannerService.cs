using System.IO;
using System.Security.Cryptography;

namespace AntiVirus.Services
{
    // Serviço para escanear arquivos em um diretório em busca de malware
    class FileScannerService : IFileScannerService
    {
        // Varre um diretório em busca de arquivos e verifica o hash deles em relação a um banco de dados de malware
        // Também fornece informações de progresso por meio de um retorno de chamada
        public IEnumerable<FileScanResult> ScanDirectoryWithProgress(string directoryPath, IEnumerable<string> database, Action<double> progressCallback)
        {
            // Verifica se o diretório especificado existe
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("O diretório especificado não existe.");
            }

            // Inicializa uma lista para armazenar os resultados da varredura
            var results = new List<FileScanResult>();

            // Obtém todos os arquivos no diretório especificado e em seus subdiretórios
            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

            // Armazena o número total de arquivos para o cálculo de progresso
            var totalFiles = files.Length;

            // Inicializa um contador para os arquivos processados
            var processedFiles = 0;

            // Itera por cada arquivo no diretório
            foreach (var filePath in files)
            {
                // Calcula o hash do arquivo atual
                string hash = CalculateHash(filePath);

                // Verifica se o hash calculado está presente no banco de dados de malware
                if (database.Contains(hash))
                {
                    // Se o hash for encontrado no banco de dados, marca o arquivo como malware
                    results.Add(new FileScanResult { FilePath = filePath, Hash = hash, IsMalware = true });
                }

                // Incrementa o contador para os arquivos processados
                processedFiles++;

                // Calcula e invoca o progresso usando o retorno de chamada
                var progress = (double)processedFiles / totalFiles * 100;
                progressCallback?.Invoke(progress);
            }

            // Retorna a lista de resultados da varredura
            return results;
        }

        // Calcula o hash SHA256 de um determinado arquivo
        private static string CalculateHash(string filePath)
        {
            // Abre o arquivo em um fluxo tampão para otimizar a leitura
            using (var stream = new BufferedStream(File.OpenRead(filePath)))
            {
                // Inicializa o algoritmo de hash SHA256
                using SHA256Managed sha = new SHA256Managed();

                // Calcula o hash e o converte para uma representação de string
                byte[] hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
