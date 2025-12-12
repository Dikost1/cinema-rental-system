using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaRentalSystem.Models
{
    /// <summary>
    /// Класс для представления аренды фильма
    /// </summary>
    public class Rental : INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _cinemaId;
        private Guid _filmId;
        private DateTimeOffset _demonstrationStartDate;
        private DateTimeOffset _demonstrationEndDate;
        private decimal _rentalPayment;
        private decimal _lateFee;
        private string _notes = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Уникальный идентификатор аренды
        /// </summary>
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        
        /// <summary>
        /// ID кинотеатра
        /// </summary>
        public Guid CinemaId
        {
            get => _cinemaId;
            set => SetProperty(ref _cinemaId, value);
        }
        
        /// <summary>
        /// ID фильма (киноленты)
        /// </summary>
        public Guid FilmId
        {
            get => _filmId;
            set => SetProperty(ref _filmId, value);
        }
        
        /// <summary>
        /// Дата начала демонстрации фильма
        /// </summary>
        public DateTimeOffset DemonstrationStartDate
        {
            get => _demonstrationStartDate;
            set => SetProperty(ref _demonstrationStartDate, value);
        }
        
        /// <summary>
        /// Дата окончания демонстрации фильма
        /// </summary>
        public DateTimeOffset DemonstrationEndDate
        {
            get => _demonstrationEndDate;
            set => SetProperty(ref _demonstrationEndDate, value);
        }
        
        /// <summary>
        /// Сумма оплаты за аренду ленты
        /// </summary>
        public decimal RentalPayment
        {
            get => _rentalPayment;
            set => SetProperty(ref _rentalPayment, value);
        }
        
        /// <summary>
        /// Пени за несвоевременный возврат
        /// </summary>
        public decimal LateFee
        {
            get => _lateFee;
            set => SetProperty(ref _lateFee, value);
        }
        
        /// <summary>
        /// Примечания
        /// </summary>
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public Rental()
        {
            _id = Guid.NewGuid();
            _demonstrationStartDate = DateTimeOffset.Now.Date;
            _demonstrationEndDate = DateTimeOffset.Now.Date.AddDays(30);
            _lateFee = 0;
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
    }
}
