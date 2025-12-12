using System;
using System.Collections.Generic;

namespace FilmManagementSystem
{
    /// <summary>
    /// Класс для представления поставщика кинолент
    /// </summary>
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LegalAddress { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string INN { get; set; }

        public Supplier()
        {
            Name = string.Empty;
            LegalAddress = string.Empty;
            BankName = string.Empty;
            BankAccountNumber = string.Empty;
            INN = string.Empty;
        }

        public Supplier(int id, string name, string legalAddress, string bankName, 
                       string bankAccountNumber, string inn)
        {
            Id = id;
            Name = name;
            LegalAddress = legalAddress;
            BankName = bankName;
            BankAccountNumber = bankAccountNumber;
            INN = inn;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Поставщик: {Name}, Адрес: {LegalAddress}, " +
                   $"Банк: {BankName}, Счет: {BankAccountNumber}, ИНН: {INN}";
        }

        /// <summary>
        /// Преобразование в строку для сохранения в файл
        /// </summary>
        public string ToFileString()
        {
            return $"{Id}|{Name}|{LegalAddress}|{BankName}|{BankAccountNumber}|{INN}";
        }

        /// <summary>
        /// Создание объекта из строки файла
        /// </summary>
        public static Supplier FromFileString(string line)
        {
            var parts = line.Split('|');
            if (parts.Length != 6)
                throw new FormatException("Неверный формат данных поставщика");

            return new Supplier
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                LegalAddress = parts[2],
                BankName = parts[3],
                BankAccountNumber = parts[4],
                INN = parts[5]
            };
        }
    }
}
