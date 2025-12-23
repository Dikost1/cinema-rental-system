using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CinemaRentalSystem.Models;

namespace CinemaRentalSystem.ViewModels
{
    /// <summary>
    /// ViewModel для управления фильмами
    /// </summary>
    public class FilmsViewModel : ViewModelBase
    {
        private readonly DataStorage _dataStorage;
        private Film? _selectedFilm;
        private Film _editingFilm;
        private Supplier? _selectedSupplier;
        private string _errorMessage = string.Empty;
        private string _titleError = string.Empty;
        private string _releaseYearError = string.Empty;
        private string _purchaseCostError = string.Empty;
        private string _supplierError = string.Empty;

        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasTitleError => !string.IsNullOrWhiteSpace(TitleError);
        public bool HasReleaseYearError => !string.IsNullOrWhiteSpace(ReleaseYearError);
        public bool HasPurchaseCostError => !string.IsNullOrWhiteSpace(PurchaseCostError);
        public bool HasSupplierError => !string.IsNullOrWhiteSpace(SupplierError);

        public ObservableCollection<Film> Films { get; }
        public ObservableCollection<Supplier> Suppliers { get; }

        public Film? SelectedFilm
        {
            get => _selectedFilm;
            set
            {
                if (SetProperty(ref _selectedFilm, value))
                {
                    if (value != null)
                    {
                        EditingFilm = CloneFilm(value);
                        SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == value.SupplierId);
                    }
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Film EditingFilm
        {
            get => _editingFilm;
            set
            {
                if (value == null)
                    return;

                if (_editingFilm != null)
                {
                    _editingFilm.PropertyChanged -= EditingFilm_PropertyChanged;
                }
                
                if (SetProperty(ref _editingFilm, value!))
                {
                    value.PropertyChanged += EditingFilm_PropertyChanged;
                    ValidateAll();
                }
            }
        }

        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (SetProperty(ref _selectedSupplier, value))
                {
                    if (value != null)
                    {
                        EditingFilm.SupplierId = value.Id;
                    }
                    ValidateField("Supplier");
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

        public string TitleError
        {
            get => _titleError;
            set
            {
                SetProperty(ref _titleError, value);
                OnPropertyChanged(nameof(HasTitleError));
            }
        }

        public string ReleaseYearError
        {
            get => _releaseYearError;
            set
            {
                SetProperty(ref _releaseYearError, value);
                OnPropertyChanged(nameof(HasReleaseYearError));
            }
        }

        public string PurchaseCostError
        {
            get => _purchaseCostError;
            set
            {
                SetProperty(ref _purchaseCostError, value);
                OnPropertyChanged(nameof(HasPurchaseCostError));
            }
        }

        public string SupplierError
        {
            get => _supplierError;
            set
            {
                SetProperty(ref _supplierError, value);
                OnPropertyChanged(nameof(HasSupplierError));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public FilmsViewModel(DataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            Films = new ObservableCollection<Film>(_dataStorage.Data.Films);
            Suppliers = new ObservableCollection<Supplier>(_dataStorage.Data.Suppliers);
            _editingFilm = new Film();
            _editingFilm.PropertyChanged += EditingFilm_PropertyChanged;

            AddCommand = new RelayCommand(Add, CanAdd);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            ClearCommand = new RelayCommand(Clear);
        }

        private void EditingFilm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateField(e.PropertyName);
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
            ((RelayCommand)UpdateCommand).RaiseCanExecuteChanged();
        }

        private bool CanAdd(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(EditingFilm.Title) && SelectedSupplier != null;
        }

        private void Add(object? parameter)
        {
            if (!ValidateFilm(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                var newFilm = CloneFilm(EditingFilm);
                newFilm.Id = Guid.NewGuid();
                _dataStorage.AddFilm(newFilm);
                Films.Add(newFilm);
                Clear(null);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при добавлении фильма: {ex.Message}";
            }
        }

        private bool CanUpdate(object? parameter)
        {
            return SelectedFilm != null && !string.IsNullOrWhiteSpace(EditingFilm.Title) && SelectedSupplier != null;
        }

        private void Update(object? parameter)
        {
            if (!ValidateFilm(out string errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            ErrorMessage = string.Empty;
            try
            {
                if (SelectedFilm != null)
                {
                    var index = Films.IndexOf(SelectedFilm);
                    if (index >= 0)
                    {
                        var updatedFilm = CloneFilm(EditingFilm);
                        updatedFilm.Id = SelectedFilm.Id;
                        _dataStorage.UpdateFilm(updatedFilm);
                        Films[index] = updatedFilm;
                        SelectedFilm = updatedFilm;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при обновлении фильма: {ex.Message}";
            }
        }

        private bool CanDelete(object? parameter)
        {
            return SelectedFilm != null;
        }

        private void Delete(object? parameter)
        {
            if (SelectedFilm != null)
            {
                _dataStorage.DeleteFilm(SelectedFilm.Id);
                Films.Remove(SelectedFilm);
                Clear(null);
            }
        }

        private void Clear(object? parameter)
        {
            EditingFilm = new Film();
            SelectedFilm = null;
            SelectedSupplier = null;
            ErrorMessage = string.Empty;
            ClearValidationErrors();
        }

        private Film CloneFilm(Film source)
        {
            return new Film
            {
                Id = source.Id,
                Title = source.Title,
                Category = source.Category,
                Screenwriter = source.Screenwriter,
                Director = source.Director,
                ProductionCompany = source.ProductionCompany,
                ReleaseYear = source.ReleaseYear,
                SupplierId = source.SupplierId,
                PurchaseCost = source.PurchaseCost
            };
        }

        private bool ValidateFilm(out string errorMessage)
        {
            errorMessage = string.Empty;
            ValidateAll();

            if (!string.IsNullOrWhiteSpace(TitleError))
            {
                errorMessage = TitleError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(ReleaseYearError))
            {
                errorMessage = ReleaseYearError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PurchaseCostError))
            {
                errorMessage = PurchaseCostError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(SupplierError))
            {
                errorMessage = SupplierError;
                return false;
            }

            return true;
        }

        private void ValidateAll()
        {
            ValidateField("Title");
            ValidateField("ReleaseYear");
            ValidateField("PurchaseCost");
            ValidateField("Supplier");
        }

        private void ValidateField(string? propertyName)
        {
            if (propertyName == null || EditingFilm == null)
                return;

            switch (propertyName)
            {
                case "Title":
                    if (!Validator.ValidateRequiredString(EditingFilm.Title))
                    {
                        TitleError = Validator.GetRequiredFieldErrorMessage("Название фильма");
                    }
                    else
                    {
                        TitleError = string.Empty;
                    }
                    break;

                case "ReleaseYear":
                    if (EditingFilm.ReleaseYear < 1800 || EditingFilm.ReleaseYear > DateTime.Now.Year + 5)
                    {
                        ReleaseYearError = $"Год выхода должен быть от 1800 до {DateTime.Now.Year + 5}";
                    }
                    else
                    {
                        ReleaseYearError = string.Empty;
                    }
                    break;

                case "PurchaseCost":
                    if (EditingFilm.PurchaseCost < 0)
                    {
                        PurchaseCostError = "Стоимость не может быть отрицательной";
                    }
                    else
                    {
                        PurchaseCostError = string.Empty;
                    }
                    break;

                case "Supplier":
                    if (SelectedSupplier == null)
                    {
                        SupplierError = "Необходимо выбрать поставщика";
                    }
                    else
                    {
                        SupplierError = string.Empty;
                    }
                    break;
            }
        }

        private void ClearValidationErrors()
        {
            TitleError = string.Empty;
            ReleaseYearError = string.Empty;
            PurchaseCostError = string.Empty;
            SupplierError = string.Empty;
        }

        public string GetSupplierName(Guid supplierId)
        {
            var supplier = Suppliers.FirstOrDefault(s => s.Id == supplierId);
            return supplier?.Name ?? "Неизвестный поставщик";
        }
    }
}
