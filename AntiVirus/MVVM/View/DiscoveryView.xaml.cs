using AntiVirus.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AntiVirus.MVVM.View
{
    public enum FileOperation
    {
        Delete
    }

    public class FileOperationEventArgs : EventArgs
    {
        public FlaggedFile File { get; set; }
        public FileOperation Operation { get; set; }

        public FileOperationEventArgs(FlaggedFile file, FileOperation operation)
        {
            File = file;
            Operation = operation;
        }
    }

    public class FileDeletedEventArgs : EventArgs
    {
        public string FilePath { get; set; }

        public FileDeletedEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }

    public partial class DiscoveryView : UserControl
    {
        public event EventHandler<FileOperationEventArgs> FileOperationRequested;
        public event EventHandler<FileDeletedEventArgs> FileDeleted;

        public DiscoveryView()
        {
            InitializeComponent();

            var flaggedFiles = MalwareDatabase.FlaggedFiles;

            flaggedFilesDataGrid.ItemsSource = flaggedFiles;

            DeleteButton.Click += DeleteButton_Click;

            FileDeleted += DiscoveryView_FileDeleted;

            UpdateSecStatus();
        }

        private void UpdateSecStatus()
        {
            int itemCount = MalwareDatabase.FlaggedFiles.Count;

            if (itemCount == 0)
            {
                SecStatusImage.Source = new BitmapImage(new Uri(@"/Images/Good.png", UriKind.RelativeOrAbsolute));
                SecStatus.Text = "Congratulations\nSystem is Safe!";
                SecStatus.Foreground = Brushes.ForestGreen;

            }
            else
            {
                SecStatusImage.Source = new BitmapImage(new Uri(@"/Images/Danger.png", UriKind.RelativeOrAbsolute));
                SecStatus.Text = $"Warning!\n{itemCount} Suspicious item{(itemCount > 1 ? "s" : "")}";
                SecStatus.Foreground = Brushes.Red;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (flaggedFilesDataGrid.SelectedItem is FlaggedFile selectedFile)
            {
                OnFileOperationRequested(new FileOperationEventArgs(selectedFile, FileOperation.Delete));
            }
        }

        protected virtual void OnFileOperationRequested(FileOperationEventArgs e)
        {
            FileOperationRequested?.Invoke(this, e);

            if (e.Operation == FileOperation.Delete)
            {
                string filePath = e.File.FilePath;

                DeleteFile(filePath);

                OnFileDeleted(new FileDeletedEventArgs(filePath));
            }
        }

        private void DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnFileDeleted(FileDeletedEventArgs e)
        {
            FileDeleted?.Invoke(this, e);

            UpdateSecStatus();
        }

        private void DiscoveryView_FileDeleted(object sender, FileDeletedEventArgs e)
        {
            MalwareDatabase.FlaggedFiles.RemoveAll(file => file.FilePath == e.FilePath);

            flaggedFilesDataGrid.Items.Refresh();

            UpdateSecStatus();
        }
    }
}
