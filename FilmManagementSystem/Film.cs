using System;

namespace FilmManagementSystem
{
    /// <summary>
    /// Класс для представления кинофильма
    /// </summary>
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Screenwriter { get; set; }
        public string Director { get; set; }
        public string ProductionCompany { get; set; }
        public int ReleaseYear { get; set; }
        public int SupplierId { get; set; }
        public decimal PurchaseCost { get; set; }

        public Film()
        {
            Title = string.Empty;
            Category = string.Empty;
            Screenwriter = string.Empty;
            Director = string.Empty;
            ProductionCompany = string.Empty;
        }

        public Film(int id, string title, string category, string screenwriter, 
                   string director, string productionCompany, int releaseYear, 
                   int supplierId, decimal purchaseCost)
        {
            Id = id;
            Title = title;
            Category = category;
            Screenwriter = screenwriter;
            Director = director;
            ProductionCompany = productionCompany;
            ReleaseYear = releaseYear;
            SupplierId = supplierId;
            PurchaseCost = purchaseCost;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Название: {Title}, Категория: {Category}, " +
                   $"Сценарист: {Screenwriter}, Режиссер: {Director}, " +
                   $"Производитель: {ProductionCompany}, Год: {ReleaseYear}, " +
                   $"Поставщик ID: {SupplierId}, Стоимость: {PurchaseCost:C}";
        }

        /// <summary>
        /// Преобразование в строку для сохранения в файл
        /// </summary>
        public string ToFileString()
        {
            return $"{Id}|{Title}|{Category}|{Screenwriter}|{Director}|" +
                   $"{ProductionCompany}|{ReleaseYear}|{SupplierId}|{PurchaseCost}";
        }

        /// <summary>
        /// Создание объекта из строки файла
        /// </summary>
        public static Film FromFileString(string line)
        {
            var parts = line.Split('|');
            if (parts.Length != 9)
                throw new FormatException("Неверный формат данных фильма");

            return new Film
            {
                Id = int.Parse(parts[0]),
                Title = parts[1],
                Category = parts[2],
                Screenwriter = parts[3],
                Director = parts[4],
                ProductionCompany = parts[5],
                ReleaseYear = int.Parse(parts[6]),
                SupplierId = int.Parse(parts[7]),
                PurchaseCost = decimal.Parse(parts[8])
            };
        }
    }
}
