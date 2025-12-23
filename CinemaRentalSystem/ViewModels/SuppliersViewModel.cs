using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CinemaRentalSystem.Models;

namespace CinemaRentalSystem.ViewModels
{
    /// <summary>
    /// ViewModel для управления поставщиками
    /// </summary>
    public class SuppliersViewModel : ViewModelBase
    {
        private readonly DataStorage _dataStorage;
        private Supplier? _selectedSupplier;
        private Supplier _editingSupplier;
        private string _errorMessage = string.Empty;
        private string _nameError = string.Empty;
        private string _accountNumberError = string.Empty;
        private string _innError = string.Empty;

        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasNameError => !string.IsNullOrWhiteSpace(NameError);
        public bool HasAccountNumberError => !string.IsNullOrWhiteSpace(AccountNumberError);
        public bool HasINNError => !string.IsNullOrWhiteSpace(INNError);

        public ObservableCollection<Supplier> Suppliers { get; }

        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (SetProperty(ref _selectedSupplier, value))
                {
                    if (value != null)
                    {
                        EditingSupplier = CloneSupplier(value);
                    }
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Supplier EditingSupplier
        {
            get => _editingSupplier;
            set
            {
                if (value == null)
                    return;

                if (_editingSupplier != null)
                {
                    _editingSupplier.PropertyChanged -= EditingSupplier_PropertyChanged;
                }
                
                if (SetProperty(ref _editingSupplier, value!))
                {
                    value.PropertyChanged += EditingSupplier_PropertyChanged;
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

        public SuppliersViewModel(DataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            Suppliers = new ObservableCollection<Supplier>(_dataStorage.Data.Suppliers);
            _editingSupplier = new Supplier();
            _editingSupplier.PropertyChanged += EditingSupplier_PropertyChanged;

            AddCommand = new RelayCommand(Add, CanAdd);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            ClearCommand = new RelayCommand(Clear);
        }

        private void EditingSupplier_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateField(e.PropertyName);
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
            ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
        }

        private bool CanAdd(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(EditingSupplier.Name);
        }

        private void Add(object? parameter)
        {
            if (!ValidateSupplier(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                var newSupplier = CloneSupplier(EditingSupplier);
                newSupplier.Id = Guid.NewGuid();
                _dataStorage.AddSupplier(newSupplier);
                Suppliers.Add(newSupplier);
                Clear(null);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при добавлении поставщика: {ex.Message}";
            }
        }

        private bool CanUpdate(object? parameter)
        {
            return SelectedSupplier != null && !string.IsNullOrWhiteSpace(EditingSupplier.Name);
        }

        private void Update(object? parameter)
        {
            if (!ValidateSupplier(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                if (SelectedSupplier != null)
                {
                    var index = Suppliers.IndexOf(SelectedSupplier);
                    if (index >= 0)
                    {
                        var updatedSupplier = CloneSupplier(EditingSupplier);
                        updatedSupplier.Id = SelectedSupplier.Id;
                        _dataStorage.UpdateSupplier(updatedSupplier);
                        Suppliers[index] = updatedSupplier;
                        SelectedSupplier = updatedSupplier;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при обновлении поставщика: {ex.Message}";
            }
        }

        private bool CanDelete(object? parameter)
        {
            return SelectedSupplier != null;
        }

        private void Delete(object? parameter)
        {
            if (SelectedSupplier != null)
            {
                _dataStorage.DeleteSupplier(SelectedSupplier.Id);
                Suppliers.Remove(SelectedSupplier);
                Clear(null);
            }
        }

        private void Clear(object? parameter)
        {
            EditingSupplier = new Supplier();
            SelectedSupplier = null;
            ErrorMessage = string.Empty;
            ClearValidationErrors();
        }

        private Supplier CloneSupplier(Supplier source)
        {
            return new Supplier
            {
                Id = source.Id,
                Name = source.Name,
                LegalAddress = source.LegalAddress,
                Bank = source.Bank,
                AccountNumber = source.AccountNumber,
                INN = source.INN
            };
        }

        private bool ValidateSupplier(out string errorMessage)
        {
            errorMessage = string.Empty;
            ValidateAll();

            if (!string.IsNullOrWhiteSpace(NameError))
            {
                errorMessage = NameError;
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
            ValidateField("AccountNumber");
            ValidateField("INN");
        }

        private void ValidateField(string? propertyName)
        {
            if (propertyName == null || EditingSupplier == null)
                return;

            switch (propertyName)
            {
                case "Name":
                    if (!Validator.ValidateRequiredString(EditingSupplier.Name))
                    {
                        NameError = Validator.GetRequiredFieldErrorMessage("Название");
                    }
                    else
                    {
                        NameError = string.Empty;
                    }
                    break;

                case "AccountNumber":
                    if (!string.IsNullOrWhiteSpace(EditingSupplier.AccountNumber))
                    {
                        if (!Validator.ValidateNumericString(EditingSupplier.AccountNumber))
                        {
                            AccountNumberError = Validator.GetNumericStringErrorMessage("Номер счета");
                        }
                        else if (!Validator.ValidateAccountNumber(EditingSupplier.AccountNumber))
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
                    if (!string.IsNullOrWhiteSpace(EditingSupplier.INN))
                    {
                        if (!Validator.ValidateNumericString(EditingSupplier.INN))
                        {
                            INNError = Validator.GetNumericStringErrorMessage("ИНН");
                        }
                        else if (!Validator.ValidateINN(EditingSupplier.INN))
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
            AccountNumberError = string.Empty;
            INNError = string.Empty;
        }
    }
}
