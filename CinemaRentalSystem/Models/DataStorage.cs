using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace CinemaRentalSystem.Models
{
    /// <summary>
    /// Класс для хранения всех данных системы
    /// </summary>
    public class DataContainer
    {
        public List<Cinema> Cinemas { get; set; } = new List<Cinema>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public List<Film> Films { get; set; } = new List<Film>();
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }

    /// <summary>
    /// Сервис для сохранения и загрузки данных в текстовом формате JSON
    /// </summary>
    public class DataStorage
    {
        private readonly string _dataFilePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public DataContainer Data { get; private set; }

        public DataStorage(string dataFilePath = "cinema_rental_data.json")
        {
            // Получаем директорию, где находится исполняемый файл приложения
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            // Если передан относительный путь, делаем его абсолютным относительно папки приложения
            if (!Path.IsPathRooted(dataFilePath))
            {
                _dataFilePath = Path.Combine(appDirectory, dataFilePath);
            }
            else
            {
                _dataFilePath = dataFilePath;
            }
            
            // Настройка параметров JSON для красивого форматирования и поддержки кириллицы
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // Форматирование с отступами для удобного редактирования
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Data = new DataContainer();
            LoadData();
        }

        /// <summary>
        /// Загружает данные из JSON файла
        /// </summary>
        public void LoadData()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string json = File.ReadAllText(_dataFilePath);
                    Data = JsonSerializer.Deserialize<DataContainer>(json, _jsonOptions) ?? new DataContainer();
                }
                else
                {
                    Data = new DataContainer();
                    SaveData(); // Создаем файл с пустыми данными
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                Data = new DataContainer();
            }
        }

        /// <summary>
        /// Сохраняет данные в JSON файл
        /// </summary>
        public void SaveData()
        {
            try
            {
                string json = JsonSerializer.Serialize(Data, _jsonOptions);
                File.WriteAllText(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
                throw;
            }
        }

        #region Cinema Operations

        public void AddCinema(Cinema cinema)
        {
            Data.Cinemas.Add(cinema);
            SaveData();
        }

        public void UpdateCinema(Cinema cinema)
        {
            var index = Data.Cinemas.FindIndex(c => c.Id == cinema.Id);
            if (index >= 0)
            {
                Data.Cinemas[index] = cinema;
                SaveData();
            }
        }

        public void DeleteCinema(Guid cinemaId)
        {
            Data.Cinemas.RemoveAll(c => c.Id == cinemaId);
            SaveData();
        }

        public Cinema? GetCinemaById(Guid id)
        {
            return Data.Cinemas.Find(c => c.Id == id);
        }

        #endregion

        #region Supplier Operations

        public void AddSupplier(Supplier supplier)
        {
            Data.Suppliers.Add(supplier);
            SaveData();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            var index = Data.Suppliers.FindIndex(s => s.Id == supplier.Id);
            if (index >= 0)
            {
                Data.Suppliers[index] = supplier;
                SaveData();
            }
        }

        public void DeleteSupplier(Guid supplierId)
        {
            Data.Suppliers.RemoveAll(s => s.Id == supplierId);
            SaveData();
        }

        public Supplier? GetSupplierById(Guid id)
        {
            return Data.Suppliers.Find(s => s.Id == id);
        }

        #endregion

        #region Film Operations

        public void AddFilm(Film film)
        {
            Data.Films.Add(film);
            SaveData();
        }

        public void UpdateFilm(Film film)
        {
            var index = Data.Films.FindIndex(f => f.Id == film.Id);
            if (index >= 0)
            {
                Data.Films[index] = film;
                SaveData();
            }
        }

        public void DeleteFilm(Guid filmId)
        {
            Data.Films.RemoveAll(f => f.Id == filmId);
            SaveData();
        }

        public Film? GetFilmById(Guid id)
        {
            return Data.Films.Find(f => f.Id == id);
        }

        #endregion

        #region Rental Operations

        public void AddRental(Rental rental)
        {
            Data.Rentals.Add(rental);
            SaveData();
        }

        public void UpdateRental(Rental rental)
        {
            var index = Data.Rentals.FindIndex(r => r.Id == rental.Id);
            if (index >= 0)
            {
                Data.Rentals[index] = rental;
                SaveData();
            }
        }

        public void DeleteRental(Guid rentalId)
        {
            Data.Rentals.RemoveAll(r => r.Id == rentalId);
            SaveData();
        }

        public Rental? GetRentalById(Guid id)
        {
            return Data.Rentals.Find(r => r.Id == id);
        }

        #endregion
    }
}

