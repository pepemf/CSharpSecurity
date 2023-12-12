using AntiVirus.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AntiVirus.MVVM.View
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            UpdateUI();
        }

        private void OpenLinkedinLink(object sender, MouseButtonEventArgs e)
        {
            string externalLink = "https://www.linkedin.com/in/pepemf/";
            OpenLinkInBrowser(externalLink);
        }

        private void OpenGitHubLink(object sender, MouseButtonEventArgs e)
        {
            string externalLink = "https://github.com/pepemf";
            OpenLinkInBrowser(externalLink);
        }

        private void OpenPortifolioLink(object sender, MouseButtonEventArgs e)
        {
            string externalLink = "https://pepemf.github.io/Portifolio/";
            OpenLinkInBrowser(externalLink);
        }


        private void OpenLinkInBrowser(string link)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = link,
                UseShellExecute = true
            });
        }

        private void UpdateUI()
        {
            int numberOfFlaggedItems = MalwareDatabase.FlaggedFiles.Count;

            UpdateAlertNumber(numberOfFlaggedItems);

            UpdateSafeOrNotText(numberOfFlaggedItems);
        }

        private void UpdateAlertNumber(int numberOfFlaggedItems)
        {
            alertNum.Text = $"{numberOfFlaggedItems} {(numberOfFlaggedItems == 1 ? "Alert" : "Alerts")}";

            alertNum.Foreground = numberOfFlaggedItems > 0 ? Brushes.Red : Brushes.Green;
        }

        private void UpdateSafeOrNotText(int numberOfFlaggedItems)
        {
            SolidColorBrush textColor = numberOfFlaggedItems > 0 ? Brushes.Red : Brushes.Green;
            safeOrNot.Foreground = textColor;
            safeOrNot.Text = numberOfFlaggedItems > 0 ? "⚠️ Not Safe" : "✅ Safe";
        }

        private void DatabaseUpdated()
        {
            UpdateUI();
        }
    }
}
