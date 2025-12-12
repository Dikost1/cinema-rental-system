using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaRentalSystem.Models
{
    /// <summary>
    /// Класс для представления кинофильма
    /// </summary>
    public class Film : INotifyPropertyChanged
    {
        private Guid _id;
        private string _title = string.Empty;
        private string _category = string.Empty;
        private string _screenwriter = string.Empty;
        private string _director = string.Empty;
        private string _productionCompany = string.Empty;
        private int _releaseYear;
        private Guid _supplierId;
        private decimal _purchaseCost;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Уникальный идентификатор фильма
        /// </summary>
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        
        /// <summary>
        /// Название кинофильма
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        
        /// <summary>
        /// Категория фильма (боевик, триллер, комедия и др.)
        /// </summary>
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }
        
        /// <summary>
        /// Автор сценария
        /// </summary>
        public string Screenwriter
        {
            get => _screenwriter;
            set => SetProperty(ref _screenwriter, value);
        }
        
        /// <summary>
        /// Режиссер-постановщик
        /// </summary>
        public string Director
        {
            get => _director;
            set => SetProperty(ref _director, value);
        }
        
        /// <summary>
        /// Компания-производитель
        /// </summary>
        public string ProductionCompany
        {
            get => _productionCompany;
            set => SetProperty(ref _productionCompany, value);
        }
        
        /// <summary>
        /// Год выхода на экран
        /// </summary>
        public int ReleaseYear
        {
            get => _releaseYear;
            set => SetProperty(ref _releaseYear, value);
        }
        
        /// <summary>
        /// ID поставщика киноленты
        /// </summary>
        public Guid SupplierId
        {
            get => _supplierId;
            set => SetProperty(ref _supplierId, value);
        }
        
        /// <summary>
        /// Стоимость приобретения
        /// </summary>
        public decimal PurchaseCost
        {
            get => _purchaseCost;
            set => SetProperty(ref _purchaseCost, value);
        }

        public Film()
        {
            _id = Guid.NewGuid();
            _releaseYear = DateTime.Now.Year;
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
            return Title;
        }
    }
}
