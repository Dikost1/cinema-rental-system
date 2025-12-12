using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaRentalSystem.Models
{
    /// <summary>
    /// Класс для представления поставщика кинолент
    /// </summary>
    public class Supplier : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name = string.Empty;
        private string _legalAddress = string.Empty;
        private string _bank = string.Empty;
        private string _accountNumber = string.Empty;
        private string _inn = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Уникальный идентификатор поставщика
        /// </summary>
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        
        /// <summary>
        /// Название поставщика
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        /// <summary>
        /// Юридический адрес поставщика
        /// </summary>
        public string LegalAddress
        {
            get => _legalAddress;
            set => SetProperty(ref _legalAddress, value);
        }
        
        /// <summary>
        /// Банк поставщика
        /// </summary>
        public string Bank
        {
            get => _bank;
            set => SetProperty(ref _bank, value);
        }
        
        /// <summary>
        /// Номер счета в банке
        /// </summary>
        public string AccountNumber
        {
            get => _accountNumber;
            set => SetProperty(ref _accountNumber, value);
        }
        
        /// <summary>
        /// ИНН поставщика
        /// </summary>
        public string INN
        {
            get => _inn;
            set => SetProperty(ref _inn, value);
        }

        public Supplier()
        {
            _id = Guid.NewGuid();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
