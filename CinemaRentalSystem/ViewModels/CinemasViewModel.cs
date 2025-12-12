using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CinemaRentalSystem.Models;

namespace CinemaRentalSystem.ViewModels
{
    /// <summary>
    /// ViewModel для управления кинотеатрами
    /// </summary>
    public class CinemasViewModel : ViewModelBase
    {
        private readonly DataStorage _dataStorage;
        private Cinema? _selectedCinema;
        private Cinema _editingCinema;
        private string _errorMessage = string.Empty;
        private string _nameError = string.Empty;
        private string _phoneError = string.Empty;
        private string _seatsCountError = string.Empty;
        private string _accountNumberError = string.Empty;
        private string _innError = string.Empty;

        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasNameError => !string.IsNullOrWhiteSpace(NameError);
        public bool HasPhoneError => !string.IsNullOrWhiteSpace(PhoneError);
        public bool HasSeatsCountError => !string.IsNullOrWhiteSpace(SeatsCountError);
        public bool HasAccountNumberError => !string.IsNullOrWhiteSpace(AccountNumberError);
        public bool HasINNError => !string.IsNullOrWhiteSpace(INNError);

        public ObservableCollection<Cinema> Cinemas { get; }

        public Cinema? SelectedCinema
        {
            get => _selectedCinema;
            set
            {
                SetProperty(ref _selectedCinema, value);
                if (value != null)
                {
                    EditingCinema = CloneCinema(value);
                }
            }
        }

        public Cinema EditingCinema
        {
            get => _editingCinema;
            set
            {
                if (value == null)
                    return;

                if (_editingCinema != null)
                {
                    _editingCinema.PropertyChanged -= EditingCinema_PropertyChanged;
                }
                
                if (SetProperty(ref _editingCinema, value!))
                {
                    value.PropertyChanged += EditingCinema_PropertyChanged;
                    ValidateAll();
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

        public string NameError
        {
            get => _nameError;
            set
            {
                SetProperty(ref _nameError, value);
                OnPropertyChanged(nameof(HasNameError));
            }
        }

        public string PhoneError
        {
            get => _phoneError;
            set
            {
                SetProperty(ref _phoneError, value);
                OnPropertyChanged(nameof(HasPhoneError));
            }
        }

        public string SeatsCountError
        {
            get => _seatsCountError;
            set
            {
                SetProperty(ref _seatsCountError, value);
                OnPropertyChanged(nameof(HasSeatsCountError));
            }
        }

        public string AccountNumberError
        {
            get => _accountNumberError;
            set
            {
                SetProperty(ref _accountNumberError, value);
                OnPropertyChanged(nameof(HasAccountNumberError));
            }
        }

        public string INNError
        {
            get => _innError;
            set
            {
                SetProperty(ref _innError, value);
                OnPropertyChanged(nameof(HasINNError));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public CinemasViewModel(DataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            Cinemas = new ObservableCollection<Cinema>(_dataStorage.Data.Cinemas);
            _editingCinema = new Cinema();
            _editingCinema.PropertyChanged += EditingCinema_PropertyChanged;

            AddCommand = new RelayCommand(Add, CanAdd);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            ClearCommand = new RelayCommand(Clear);
        }

        private void EditingCinema_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateField(e.PropertyName);
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
            ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
        }

        private bool CanAdd(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(EditingCinema.Name);
        }

        private void Add(object? parameter)
        {
            if (!ValidateCinema(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                var newCinema = CloneCinema(EditingCinema);
                newCinema.Id = Guid.NewGuid();
                _dataStorage.AddCinema(newCinema);
                Cinemas.Add(newCinema);
                Clear(null);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при добавлении кинотеатра: {ex.Message}";
            }
        }

        private bool CanUpdate(object? parameter)
        {
            return SelectedCinema != null && !string.IsNullOrWhiteSpace(EditingCinema.Name);
        }

        private void Update(object? parameter)
        {
            if (!ValidateCinema(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                if (SelectedCinema != null)
                {
                    var index = Cinemas.IndexOf(SelectedCinema);
                    if (index >= 0)
                    {
                        var updatedCinema = CloneCinema(EditingCinema);
                        updatedCinema.Id = SelectedCinema.Id;
                        _dataStorage.UpdateCinema(updatedCinema);
                        Cinemas[index] = updatedCinema;
                        SelectedCinema = updatedCinema;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при обновлении кинотеатра: {ex.Message}";
            }
        }

        private bool CanDelete(object? parameter)
        {
            return SelectedCinema != null;
        }

        private void Delete(object? parameter)
        {
            if (SelectedCinema != null)
            {
                _dataStorage.DeleteCinema(SelectedCinema.Id);
                Cinemas.Remove(SelectedCinema);
                Clear(null);
            }
        }

        private void Clear(object? parameter)
        {
            EditingCinema = new Cinema();
            SelectedCinema = null;
            ErrorMessage = string.Empty;
            ClearValidationErrors();
        }

        private Cinema CloneCinema(Cinema source)
        {
            return new Cinema
            {
                Id = source.Id,
                Name = source.Name,
                Address = source.Address,
                Phone = source.Phone,
                SeatsCount = source.SeatsCount,
                Director = source.Director,
                Owner = source.Owner,
                Bank = source.Bank,
                AccountNumber = source.AccountNumber,
                INN = source.INN
            };
        }

        private bool ValidateCinema(out string errorMessage)
        {
            errorMessage = string.Empty;
            ValidateAll();

            if (!string.IsNullOrWhiteSpace(NameError))
            {
                errorMessage = NameError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PhoneError))
            {
                errorMessage = PhoneError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(SeatsCountError))
            {
                errorMessage = SeatsCountError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(AccountNumberError))
            {
                errorMessage = AccountNumberError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(INNError))
            {
                errorMessage = INNError;
                return false;
            }

            return true;
        }

        private void ValidateAll()
        {
            ValidateField("Name");
            ValidateField("Phone");
            ValidateField("SeatsCount");
            ValidateField("AccountNumber");
            ValidateField("INN");
        }

        private void ValidateField(string? propertyName)
        {
            if (propertyName == null || EditingCinema == null)
                return;

            switch (propertyName)
            {
                case "Name":
                    if (!Validator.ValidateRequiredString(EditingCinema.Name))
                    {
                        NameError = Validator.GetRequiredFieldErrorMessage("Название");
                    }
                    else
                    {
                        NameError = string.Empty;
                    }
                    break;

                case "Phone":
                    if (!string.IsNullOrWhiteSpace(EditingCinema.Phone) && !Validator.ValidatePhone(EditingCinema.Phone))
                    {
                        PhoneError = Validator.GetPhoneErrorMessage();
                    }
                    else
                    {
                        PhoneError = string.Empty;
                    }
                    break;

                case "SeatsCount":
                    if (!Validator.ValidateSeatsCount(EditingCinema.SeatsCount))
                    {
                        SeatsCountError = Validator.GetSeatsCountErrorMessage();
                    }
                    else
                    {
                        SeatsCountError = string.Empty;
                    }
                    break;

                case "AccountNumber":
                    if (!string.IsNullOrWhiteSpace(EditingCinema.AccountNumber))
                    {
                        if (!Validator.ValidateNumericString(EditingCinema.AccountNumber))
                        {
                            AccountNumberError = Validator.GetNumericStringErrorMessage("Номер счета");
                        }
                        else if (!Validator.ValidateAccountNumber(EditingCinema.AccountNumber))
                        {
                            AccountNumberError = Validator.GetAccountNumberErrorMessage();
                        }
                        else
                        {
                            AccountNumberError = string.Empty;
                        }
                    }
                    else
                    {
                        AccountNumberError = string.Empty;
                    }
                    break;

                case "INN":
                    if (!string.IsNullOrWhiteSpace(EditingCinema.INN))
                    {
                        if (!Validator.ValidateNumericString(EditingCinema.INN))
                        {
                            INNError = Validator.GetNumericStringErrorMessage("ИНН");
                        }
                        else if (!Validator.ValidateINN(EditingCinema.INN))
                        {
                            INNError = Validator.GetINNErrorMessage();
                        }
                        else
                        {
                            INNError = string.Empty;
                        }
                    }
                    else
                    {
                        INNError = string.Empty;
                    }
                    break;
            }
        }

        private void ClearValidationErrors()
        {
            NameError = string.Empty;
            PhoneError = string.Empty;
            SeatsCountError = string.Empty;
            AccountNumberError = string.Empty;
            INNError = string.Empty;
        }
    }
}
