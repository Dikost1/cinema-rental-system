using System;
using System.Linq;

namespace FilmManagementSystem
{
    class Program
    {
        private static DataManager dataManager;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Система учета кинофильмов ===\n");

            dataManager = new DataManager();

            bool running = true;
            while (running)
            {
                ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageSuppliers();
                        break;
                    case "2":
                        ManageFilms();
                        break;
                    case "3":
                        ShowFilmsBySupplier();
                        break;
                    case "4":
                        dataManager.SaveData();
                        break;
                    case "5":
                        dataManager.SaveData();
                        running = false;
                        Console.WriteLine("\nДо свидания!");
                        break;
                    default:
                        Console.WriteLine("\nНеверный выбор! Попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Управление поставщиками");
            Console.WriteLine("2. Управление фильмами");
            Console.WriteLine("3. Показать фильмы по поставщику");
            Console.WriteLine("4. Сохранить данные");
            Console.WriteLine("5. Сохранить и выйти");
            Console.Write("\nВыберите пункт меню: ");
        }

        static void ManageSuppliers()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== УПРАВЛЕНИЕ ПОСТАВЩИКАМИ ===");
                Console.WriteLine("1. Показать всех поставщиков");
                Console.WriteLine("2. Добавить поставщика");
                Console.WriteLine("3. Удалить поставщика");
                Console.WriteLine("4. Назад");
                Console.Write("\nВыберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllSuppliers();
                        break;
                    case "2":
                        AddSupplier();
                        break;
                    case "3":
                        DeleteSupplier();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\nНеверный выбор!");
                        break;
                }
            }
        }

        static void ShowAllSuppliers()
        {
            var suppliers = dataManager.GetAllSuppliers();
            Console.WriteLine("\n=== СПИСОК ПОСТАВЩИКОВ ===");
            if (suppliers.Count == 0)
            {
                Console.WriteLine("Поставщики не найдены.");
                return;
            }

            foreach (var supplier in suppliers)
            {
                Console.WriteLine($"\n{supplier}");
            }
        }

        static void AddSupplier()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ПОСТАВЩИКА ===");
            
            Console.Write("Название поставщика: ");
            string name = Console.ReadLine();

            Console.Write("Юридический адрес: ");
            string address = Console.ReadLine();

            Console.Write("Название банка: ");
            string bank = Console.ReadLine();

            Console.Write("Номер счета: ");
            string account = Console.ReadLine();

            Console.Write("ИНН: ");
            string inn = Console.ReadLine();

            var supplier = new Supplier(0, name, address, bank, account, inn);
            dataManager.AddSupplier(supplier);
        }

        static void DeleteSupplier()
        {
            Console.Write("\nВведите ID поставщика для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                dataManager.DeleteSupplier(id);
            }
            else
            {
                Console.WriteLine("Неверный ID!");
            }
        }

        static void ManageFilms()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== УПРАВЛЕНИЕ ФИЛЬМАМИ ===");
                Console.WriteLine("1. Показать все фильмы");
                Console.WriteLine("2. Добавить фильм");
                Console.WriteLine("3. Удалить фильм");
                Console.WriteLine("4. Назад");
                Console.Write("\nВыберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllFilms();
                        break;
                    case "2":
                        AddFilm();
                        break;
                    case "3":
                        DeleteFilm();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\nНеверный выбор!");
                        break;
                }
            }
        }

        static void ShowAllFilms()
        {
            var films = dataManager.GetAllFilms();
            Console.WriteLine("\n=== СПИСОК ФИЛЬМОВ ===");
            if (films.Count == 0)
            {
                Console.WriteLine("Фильмы не найдены.");
                return;
            }

            foreach (var film in films)
            {
                var supplier = dataManager.GetSupplierById(film.SupplierId);
                Console.WriteLine($"\n{film}");
                if (supplier != null)
                {
                    Console.WriteLine($"   Поставщик: {supplier.Name}");
                }
            }
        }

        static void AddFilm()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ФИЛЬМА ===");

            // Сначала показываем доступных поставщиков
            var suppliers = dataManager.GetAllSuppliers();
            if (suppliers.Count == 0)
            {
                Console.WriteLine("Сначала необходимо добавить поставщика!");
                return;
            }

            Console.WriteLine("\nДоступные поставщики:");
            foreach (var s in suppliers)
            {
                Console.WriteLine($"ID {s.Id}: {s.Name}");
            }

            Console.Write("\nНазвание фильма: ");
            string title = Console.ReadLine();

            Console.Write("Категория (боевик, триллер, комедия и др.): ");
            string category = Console.ReadLine();

            Console.Write("Автор сценария: ");
            string screenwriter = Console.ReadLine();

            Console.Write("Режиссер-постановщик: ");
            string director = Console.ReadLine();

            Console.Write("Компания-производитель: ");
            string company = Console.ReadLine();

            Console.Write("Год выхода на экран: ");
            int year;
            while (!int.TryParse(Console.ReadLine(), out year) || year < 1800 || year > DateTime.Now.Year + 5)
            {
                Console.Write("Неверный год! Введите корректный год: ");
            }

            Console.Write("ID поставщика: ");
            int supplierId;
            while (!int.TryParse(Console.ReadLine(), out supplierId) || 
                   dataManager.GetSupplierById(supplierId) == null)
            {
                Console.Write("Неверный ID поставщика! Введите существующий ID: ");
            }

            Console.Write("Стоимость приобретения (руб.): ");
            decimal cost;
            while (!decimal.TryParse(Console.ReadLine(), out cost) || cost < 0)
            {
                Console.Write("Неверная стоимость! Введите положительное число: ");
            }

            var film = new Film(0, title, category, screenwriter, director, 
                               company, year, supplierId, cost);
            dataManager.AddFilm(film);
        }

        static void DeleteFilm()
        {
            Console.Write("\nВведите ID фильма для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                dataManager.DeleteFilm(id);
            }
            else
            {
                Console.WriteLine("Неверный ID!");
            }
        }

        static void ShowFilmsBySupplier()
        {
            Console.WriteLine("\n=== ФИЛЬМЫ ПО ПОСТАВЩИКУ ===");
            
            var suppliers = dataManager.GetAllSuppliers();
            if (suppliers.Count == 0)
            {
                Console.WriteLine("Поставщики не найдены.");
                return;
            }

            Console.WriteLine("\nДоступные поставщики:");
            foreach (var s in suppliers)
            {
                Console.WriteLine($"ID {s.Id}: {s.Name}");
            }

            Console.Write("\nВведите ID поставщика: ");
            if (int.TryParse(Console.ReadLine(), out int supplierId))
            {
                var supplier = dataManager.GetSupplierById(supplierId);
                if (supplier != null)
                {
                    Console.WriteLine($"\nПоставщик: {supplier.Name}");
                    var films = dataManager.GetFilmsBySupplier(supplierId);
                    
                    if (films.Count == 0)
                    {
                        Console.WriteLine("У этого поставщика нет фильмов.");
                    }
                    else
                    {
                        Console.WriteLine($"\nФильмов от поставщика: {films.Count}");
                        foreach (var film in films)
                        {
                            Console.WriteLine($"\n{film}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Поставщик не найден!");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID!");
            }
        }
    }
}
