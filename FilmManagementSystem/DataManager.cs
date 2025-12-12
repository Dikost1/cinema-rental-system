using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilmManagementSystem
{
    /// <summary>
    /// Класс для управления данными фильмов и поставщиков
    /// </summary>
    public class DataManager
    {
        private const string SuppliersFileName = "suppliers.txt";
        private const string FilmsFileName = "films.txt";

        private List<Supplier> suppliers;
        private List<Film> films;

        public DataManager()
        {
            suppliers = new List<Supplier>();
            films = new List<Film>();
            LoadData();
        }

        /// <summary>
        /// Загрузка данных из файлов
        /// </summary>
        public void LoadData()
        {
            // Загрузка поставщиков
            if (File.Exists(SuppliersFileName))
            {
                try
                {
                    var lines = File.ReadAllLines(SuppliersFileName);
                    suppliers.Clear();
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            suppliers.Add(Supplier.FromFileString(line));
                        }
                    }
                    Console.WriteLine($"Загружено поставщиков: {suppliers.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке поставщиков: {ex.Message}");
                }
            }

            // Загрузка фильмов
            if (File.Exists(FilmsFileName))
            {
                try
                {
                    var lines = File.ReadAllLines(FilmsFileName);
                    films.Clear();
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            films.Add(Film.FromFileString(line));
                        }
                    }
                    Console.WriteLine($"Загружено фильмов: {films.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке фильмов: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Сохранение данных в файлы
        /// </summary>
        public void SaveData()
        {
            try
            {
                // Сохранение поставщиков
                var supplierLines = suppliers.Select(s => s.ToFileString()).ToArray();
                File.WriteAllLines(SuppliersFileName, supplierLines);
                Console.WriteLine($"Сохранено поставщиков: {suppliers.Count}");

                // Сохранение фильмов
                var filmLines = films.Select(f => f.ToFileString()).ToArray();
                File.WriteAllLines(FilmsFileName, filmLines);
                Console.WriteLine($"Сохранено фильмов: {films.Count}");

                Console.WriteLine("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавление нового поставщика
        /// </summary>
        public void AddSupplier(Supplier supplier)
        {
            supplier.Id = suppliers.Count > 0 ? suppliers.Max(s => s.Id) + 1 : 1;
            suppliers.Add(supplier);
            Console.WriteLine($"Поставщик добавлен с ID: {supplier.Id}");
        }

        /// <summary>
        /// Добавление нового фильма
        /// </summary>
        public void AddFilm(Film film)
        {
            film.Id = films.Count > 0 ? films.Max(f => f.Id) + 1 : 1;
            films.Add(film);
            Console.WriteLine($"Фильм добавлен с ID: {film.Id}");
        }

        /// <summary>
        /// Получение всех поставщиков
        /// </summary>
        public List<Supplier> GetAllSuppliers()
        {
            return new List<Supplier>(suppliers);
        }

        /// <summary>
        /// Получение всех фильмов
        /// </summary>
        public List<Film> GetAllFilms()
        {
            return new List<Film>(films);
        }

        /// <summary>
        /// Поиск поставщика по ID
        /// </summary>
        public Supplier GetSupplierById(int id)
        {
            return suppliers.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Поиск фильма по ID
        /// </summary>
        public Film GetFilmById(int id)
        {
            return films.FirstOrDefault(f => f.Id == id);
        }

        /// <summary>
        /// Удаление поставщика
        /// </summary>
        public bool DeleteSupplier(int id)
        {
            var supplier = GetSupplierById(id);
            if (supplier != null)
            {
                // Проверка, есть ли фильмы от этого поставщика
                if (films.Any(f => f.SupplierId == id))
                {
                    Console.WriteLine("Невозможно удалить поставщика, так как есть фильмы от него!");
                    return false;
                }
                suppliers.Remove(supplier);
                Console.WriteLine("Поставщик удален!");
                return true;
            }
            Console.WriteLine("Поставщик не найден!");
            return false;
        }

        /// <summary>
        /// Удаление фильма
        /// </summary>
        public bool DeleteFilm(int id)
        {
            var film = GetFilmById(id);
            if (film != null)
            {
                films.Remove(film);
                Console.WriteLine("Фильм удален!");
                return true;
            }
            Console.WriteLine("Фильм не найден!");
            return false;
        }

        /// <summary>
        /// Получение фильмов по поставщику
        /// </summary>
        public List<Film> GetFilmsBySupplier(int supplierId)
        {
            return films.Where(f => f.SupplierId == supplierId).ToList();
        }
    }
}
