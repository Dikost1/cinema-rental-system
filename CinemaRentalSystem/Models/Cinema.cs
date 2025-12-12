using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaRentalSystem.Models
{
    /// <summary>
    /// Класс для представления кинотеатра
    /// </summary>
    public class Cinema : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name = string.Empty;
        private string _address = string.Empty;
        private string _phone = string.Empty;
        private int _seatsCount;
        private string _director = string.Empty;
        private string _owner = string.Empty;
        private string _bank = string.Empty;
        private string _accountNumber = string.Empty;
        private string _inn = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Уникальный идентификатор кинотеатра
        /// </summary>
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        
        /// <summary>
        /// Название кинотеатра
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        /// <summary>
        /// Адрес кинотеатра
        /// </summary>
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        
        /// <summary>
        /// Телефон кинотеатра
        /// </summary>
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        
        /// <summary>
        /// Число посадочных мест
        /// </summary>
        public int SeatsCount
        {
            get => _seatsCount;
            set => SetProperty(ref _seatsCount, value);
        }
        
        /// <summary>
        /// Директор кинотеатра
        /// </summary>
        public string Director
        {
            get => _director;
            set => SetProperty(ref _director, value);
        }
        
        /// <summary>
        /// Владелец кинотеатра
        /// </summary>
        public string Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }
        
        /// <summary>
        /// Банк кинотеатра
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
        /// ИНН кинотеатра
        /// </summary>
        public string INN
        {
            get => _inn;
            set => SetProperty(ref _inn, value);
        }

        public Cinema()
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
            return $"{Name} ({Address})";
        }
    }
}
