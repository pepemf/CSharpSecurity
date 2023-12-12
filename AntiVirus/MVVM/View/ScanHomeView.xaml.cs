using System.Windows;
using System.Windows.Forms;


namespace AntiVirus.MVVM.View
{
    /// <summary>
    /// Interação lógica para ScanHomeView.xam
    /// </summary>
    public partial class ScanHomeView : System.Windows.Controls.UserControl
    {
        public ScanHomeView()
        {
            InitializeComponent();
        }

        private void ChangeContentButton_Click(object sender, RoutedEventArgs e)
        {
            SetContent(new ScanLoadingView(directoryCustomText.Text, databaseComboBox.Text));
        }
        private void SetContent(System.Windows.Controls.UserControl content)
        {
            MainContentControl.Content = content;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    directoryCustomText.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

    }
}
