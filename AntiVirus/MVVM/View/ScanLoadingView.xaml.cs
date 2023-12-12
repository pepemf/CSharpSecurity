using AntiVirus.Data;
using AntiVirus.Services;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AntiVirus.MVVM.View
{
    /// <summary>
    /// Interação lógica para ScanLoadingView.xam
    /// </summary>
    public partial class ScanLoadingView : UserControl
    {
        private readonly string directoryPath;
        private readonly string databaseType;
        private readonly FileScannerService fileScannerService;
        public ScanLoadingView(string @directoryPath, string databaseType)
        {
            InitializeComponent();
            this.directoryPath = string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                : directoryPath;
            this.databaseType = databaseType;
            this.fileScannerService = new FileScannerService();
            Loaded += ScanLoadingView_Loaded;

        }

        private async void ScanLoadingView_Loaded(object sender, RoutedEventArgs e)
        {
            var results = await Task.Run(() => ScanDirectoryAsync(directoryPath, databaseType));

            foreach (var result in results)
            {
                if (result.IsMalware)
                {
                    MalwareDatabase.FlaggedFiles.Add(new FlaggedFile
                    {
                        FilePath = result.FilePath,
                        FileName = System.IO.Path.GetFileName(result.FilePath),
                        FlaggedDateTime = DateTime.Now
                    });
                }
            }

            SetContent(new ScanEndedView());
        }

        private void SetContent(System.Windows.Controls.UserControl content)
        {
            MainContentControl.Content = content;
        }


        private IEnumerable<FileScanResult> ScanDirectoryAsync(string directoryPath, string databaseType)
        {
            void ProgressCallback(double progress)
            {
                Dispatcher.Invoke(() => progessBar.Value = progress);
            }

            IEnumerable<string> selectedDatabase = databaseType switch
            {
                "Known Malwares" => MalwareDatabase.KnownMalwareHashes,
                "Project Hashes" => MalwareDatabase.ProjetoHashes,
                _ => throw new NotSupportedException($"Database type '{databaseType}' is not supported."),
            };

            return fileScannerService.ScanDirectoryWithProgress(directoryPath, selectedDatabase, ProgressCallback);
        }

    }
}
