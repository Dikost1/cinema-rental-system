using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CinemaRentalSystem.Models;

namespace CinemaRentalSystem.ViewModels
{
    /// <summary>
    /// ViewModel для управления арендой фильмов
    /// </summary>
    public class RentalsViewModel : ViewModelBase
    {
        private readonly DataStorage _dataStorage;
        private Rental? _selectedRental;
        private Rental _editingRental;
        private Cinema? _selectedCinema;
        private Film? _selectedFilm;
        private string _errorMessage = string.Empty;
        private string _cinemaError = string.Empty;
        private string _filmError = string.Empty;
        private string _demonstrationStartError = string.Empty;
        private string _demonstrationEndError = string.Empty;
        private string _rentalPaymentError = string.Empty;
        private string _lateFeeError = string.Empty;

        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasCinemaError => !string.IsNullOrWhiteSpace(CinemaError);
        public bool HasFilmError => !string.IsNullOrWhiteSpace(FilmError);
        public bool HasDemonstrationStartError => !string.IsNullOrWhiteSpace(DemonstrationStartError);
        public bool HasDemonstrationEndError => !string.IsNullOrWhiteSpace(DemonstrationEndError);
        public bool HasRentalPaymentError => !string.IsNullOrWhiteSpace(RentalPaymentError);
        public bool HasLateFeeError => !string.IsNullOrWhiteSpace(LateFeeError);

        public ObservableCollection<Rental> Rentals { get; }
        public ObservableCollection<Cinema> Cinemas { get; }
        public ObservableCollection<Film> Films { get; }

        public Rental? SelectedRental
        {
            get => _selectedRental;
            set
            {
                SetProperty(ref _selectedRental, value);
                if (value != null)
                {
                    EditingRental = CloneRental(value);
                    SelectedCinema = Cinemas.FirstOrDefault(c => c.Id == value.CinemaId);
                    SelectedFilm = Films.FirstOrDefault(f => f.Id == value.FilmId);
                }
            }
        }

        public Rental EditingRental
        {
            get => _editingRental;
            set
            {
                if (value == null)
                    return;

                if (_editingRental != null)
                {
                    _editingRental.PropertyChanged -= EditingRental_PropertyChanged;
                }
                
                if (SetProperty(ref _editingRental, value!))
                {
                    value.PropertyChanged += EditingRental_PropertyChanged;
                    ValidateAll();
                }
            }
        }

        public Cinema? SelectedCinema
        {
            get => _selectedCinema;
            set
            {
                if (SetProperty(ref _selectedCinema, value))
                {
                    if (value != null)
                    {
                        EditingRental.CinemaId = value.Id;
                    }
                    ValidateField("Cinema");
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Film? SelectedFilm
        {
            get => _selectedFilm;
            set
            {
                if (SetProperty(ref _selectedFilm, value))
                {
                    if (value != null)
                    {
                        EditingRental.FilmId = value.Id;
                    }
                    ValidateField("Film");
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public string CinemaError
        {
            get => _cinemaError;
            set
            {
                SetProperty(ref _cinemaError, value);
                OnPropertyChanged(nameof(HasCinemaError));
            }
        }

        public string FilmError
        {
            get => _filmError;
            set
            {
                SetProperty(ref _filmError, value);
                OnPropertyChanged(nameof(HasFilmError));
            }
        }

        public string DemonstrationStartError
        {
            get => _demonstrationStartError;
            set
            {
                SetProperty(ref _demonstrationStartError, value);
                OnPropertyChanged(nameof(HasDemonstrationStartError));
            }
        }

        public string DemonstrationEndError
        {
            get => _demonstrationEndError;
            set
            {
                SetProperty(ref _demonstrationEndError, value);
                OnPropertyChanged(nameof(HasDemonstrationEndError));
            }
        }

        public string RentalPaymentError
        {
            get => _rentalPaymentError;
            set
            {
                SetProperty(ref _rentalPaymentError, value);
                OnPropertyChanged(nameof(HasRentalPaymentError));
            }
        }

        public string LateFeeError
        {
            get => _lateFeeError;
            set
            {
                SetProperty(ref _lateFeeError, value);
                OnPropertyChanged(nameof(HasLateFeeError));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public RentalsViewModel(DataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            Rentals = new ObservableCollection<Rental>(_dataStorage.Data.Rentals);
            Cinemas = new ObservableCollection<Cinema>(_dataStorage.Data.Cinemas);
            Films = new ObservableCollection<Film>(_dataStorage.Data.Films);
            _editingRental = new Rental();
            _editingRental.PropertyChanged += EditingRental_PropertyChanged;

            AddCommand = new RelayCommand(Add, CanAdd);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            ClearCommand = new RelayCommand(Clear);
        }

        private void EditingRental_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateField(e.PropertyName);
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
            ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
        }

        private bool CanAdd(object? parameter)
        {
            return SelectedCinema != null && SelectedFilm != null;
        }

        private void Add(object? parameter)
        {
            if (!ValidateRental(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                var newRental = CloneRental(EditingRental);
                newRental.Id = Guid.NewGuid();
                _dataStorage.AddRental(newRental);
                Rentals.Add(newRental);
                Clear(null);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при добавлении аренды: {ex.Message}";
            }
        }

        private bool CanUpdate(object? parameter)
        {
            return SelectedRental != null && SelectedCinema != null && SelectedFilm != null;
        }

        private void Update(object? parameter)
        {
            if (!ValidateRental(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                if (SelectedRental != null)
                {
                    var index = Rentals.IndexOf(SelectedRental);
                    if (index >= 0)
                    {
                        var updatedRental = CloneRental(EditingRental);
                        updatedRental.Id = SelectedRental.Id;
                        _dataStorage.UpdateRental(updatedRental);
                        Rentals[index] = updatedRental;
                        SelectedRental = updatedRental;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при обновлении аренды: {ex.Message}";
            }
        }

        private bool CanDelete(object? parameter)
        {
            return SelectedRental != null;
        }

        private void Delete(object? parameter)
        {
            if (SelectedRental != null)
            {
                _dataStorage.DeleteRental(SelectedRental.Id);
                Rentals.Remove(SelectedRental);
                Clear(null);
            }
        }

        private void Clear(object? parameter)
        {
            EditingRental = new Rental();
            SelectedRental = null;
            SelectedCinema = null;
            SelectedFilm = null;
            ErrorMessage = string.Empty;
            ClearValidationErrors();
        }

        private Rental CloneRental(Rental source)
        {
            return new Rental
            {
                Id = source.Id,
                CinemaId = source.CinemaId,
                FilmId = source.FilmId,
                DemonstrationStartDate = source.DemonstrationStartDate,
                DemonstrationEndDate = source.DemonstrationEndDate,
                RentalPayment = source.RentalPayment,
                LateFee = source.LateFee,
                Notes = source.Notes
            };
        }

        private bool ValidateRental(out string errorMessage)
        {
            errorMessage = string.Empty;
            ValidateAll();

            if (!string.IsNullOrWhiteSpace(CinemaError))
            {
                errorMessage = CinemaError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(FilmError))
            {
                errorMessage = FilmError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(DemonstrationStartError))
            {
                errorMessage = DemonstrationStartError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(DemonstrationEndError))
            {
                errorMessage = DemonstrationEndError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(RentalPaymentError))
            {
                errorMessage = RentalPaymentError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(LateFeeError))
            {
                errorMessage = LateFeeError;
                return false;
            }

            return true;
        }

        private void ValidateAll()
        {
            ValidateField("Cinema");
            ValidateField("Film");
            ValidateField("DemonstrationStartDate");
            ValidateField("DemonstrationEndDate");
            ValidateField("RentalPayment");
            ValidateField("LateFee");
        }

        private void ValidateField(string? propertyName)
        {
            if (propertyName == null || EditingRental == null)
                return;

            switch (propertyName)
            {
                case "Cinema":
                    if (SelectedCinema == null)
                    {
                        CinemaError = "Необходимо выбрать кинотеатр";
                    }
                    else
                    {
                        CinemaError = string.Empty;
                    }
                    break;

                case "Film":
                    if (SelectedFilm == null)
                    {
                        FilmError = "Необходимо выбрать фильм (киноленту)";
                    }
                    else
                    {
                        FilmError = string.Empty;
                    }
                    break;

                case "DemonstrationStartDate":
                    if (EditingRental.DemonstrationStartDate < new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))
                    {
                        DemonstrationStartError = "Дата начала демонстрации должна быть корректной";
                    }
                    else if (EditingRental.DemonstrationStartDate > EditingRental.DemonstrationEndDate)
                    {
                        DemonstrationStartError = "Дата начала не может быть позже даты окончания";
                    }
                    else
                    {
                        DemonstrationStartError = string.Empty;
                        if (EditingRental.DemonstrationEndDate >= EditingRental.DemonstrationStartDate)
                        {
                            DemonstrationEndError = string.Empty;
                        }
                    }
                    break;

                case "DemonstrationEndDate":
                    if (EditingRental.DemonstrationEndDate < EditingRental.DemonstrationStartDate)
                    {
                        DemonstrationEndError = "Дата окончания не может быть раньше даты начала";
                    }
                    else
                    {
                        DemonstrationEndError = string.Empty;
                        if (EditingRental.DemonstrationStartDate <= EditingRental.DemonstrationEndDate)
                        {
                            DemonstrationStartError = string.Empty;
                        }
                    }
                    break;

                case "RentalPayment":
                    if (EditingRental.RentalPayment < 0)
                    {
                        RentalPaymentError = "Сумма оплаты не может быть отрицательной";
                    }
                    else if (EditingRental.RentalPayment == 0)
                    {
                        RentalPaymentError = "Необходимо указать сумму оплаты за аренду";
                    }
                    else
                    {
                        RentalPaymentError = string.Empty;
                    }
                    break;

                case "LateFee":
                    if (EditingRental.LateFee < 0)
                    {
                        LateFeeError = "Пени не могут быть отрицательными";
                    }
                    else
                    {
                        LateFeeError = string.Empty;
                    }
                    break;
            }
        }

        private void ClearValidationErrors()
        {
            CinemaError = string.Empty;
            FilmError = string.Empty;
            DemonstrationStartError = string.Empty;
            DemonstrationEndError = string.Empty;
            RentalPaymentError = string.Empty;
            LateFeeError = string.Empty;
        }

        public string GetCinemaName(Guid cinemaId)
        {
            var cinema = Cinemas.FirstOrDefault(c => c.Id == cinemaId);
            return cinema?.Name ?? "Неизвестный кинотеатр";
        }

        public string GetFilmTitle(Guid filmId)
        {
            var film = Films.FirstOrDefault(f => f.Id == filmId);
            return film?.Title ?? "Неизвестный фильм";
        }
    }
}
