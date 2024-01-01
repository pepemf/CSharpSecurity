using AntiVirus.Core;

namespace AntiVirus.MVVM.ViewModel
{
    // ViewModel principal que gerencia as visualizações na aplicação
    internal class MainViewModel : ObservableObject
    {
        // Comandos para navegar entre as diferentes visualizações
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand ScanViewCommand { get; set; }

        // ViewModels associados a cada visualização
        public HomeViewModel HomeVm { get; set; }
        public DiscoveryViewModel DiscoveryVm { get; set; }
        public ScanViewModel ScanVm { get; set; }

        // Propriedade que representa a visualização atual na interface do usuário
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                // Define a visualização atual e notifica as alterações aos objetos vinculados
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Construtor da classe MainViewModel
        public MainViewModel()
        {
            // Inicializa as instâncias das ViewModels associadas a cada visualização
            HomeVm = new HomeViewModel();
            DiscoveryVm = new DiscoveryViewModel();
            ScanVm = new ScanViewModel();

            // Define a visualização inicial como a Home
            CurrentView = HomeVm;

            // Inicializa os comandos de navegação entre visualizações
            HomeViewCommand = new RelayCommand(o =>
            {
                // Altera a visualização atual para a Home
                CurrentView = HomeVm;
            });

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                // Altera a visualização atual para a Discovery
                CurrentView = DiscoveryVm;
            });

            ScanViewCommand = new RelayCommand(o =>
            {
                // Altera a visualização atual para a Scan
                CurrentView = ScanVm;
            });
        }
    }
}
