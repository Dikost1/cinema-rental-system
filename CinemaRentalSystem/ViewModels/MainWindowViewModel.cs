using System;
using System.Windows.Input;
using CinemaRentalSystem.Models;

namespace CinemaRentalSystem.ViewModels
{
    /// <summary>
    /// Главная ViewModel приложения
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DataStorage _dataStorage;
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public CinemasViewModel CinemasViewModel { get; }
        public SuppliersViewModel SuppliersViewModel { get; }
        public FilmsViewModel FilmsViewModel { get; }
        public RentalsViewModel RentalsViewModel { get; }

        public ICommand ShowCinemasCommand { get; }
        public ICommand ShowSuppliersCommand { get; }
        public ICommand ShowFilmsCommand { get; }
        public ICommand ShowRentalsCommand { get; }

        public MainWindowViewModel()
        {
            _dataStorage = new DataStorage();

            CinemasViewModel = new CinemasViewModel(_dataStorage);
            SuppliersViewModel = new SuppliersViewModel(_dataStorage);
            FilmsViewModel = new FilmsViewModel(_dataStorage);
            RentalsViewModel = new RentalsViewModel(_dataStorage);

            ShowCinemasCommand = new RelayCommand(_ => 
            {
                CurrentView = CinemasViewModel;
            });
            ShowSuppliersCommand = new RelayCommand(_ => 
            {
                CurrentView = SuppliersViewModel;
            });
            ShowFilmsCommand = new RelayCommand(_ => 
            {
                CurrentView = FilmsViewModel;
            });
            ShowRentalsCommand = new RelayCommand(_ => 
            {
                CurrentView = RentalsViewModel;
            });

            _currentView = CinemasViewModel;
        }
    }
}
