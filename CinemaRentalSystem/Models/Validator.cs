using System;
using System.Text.RegularExpressions;

namespace CinemaRentalSystem.Models
{
    /// <summary>
    /// Класс для валидации данных
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Валидация ИНН (10 или 12 цифр)
        /// </summary>
        public static bool ValidateINN(string inn)
        {
            if (string.IsNullOrWhiteSpace(inn))
                return false;

            return Regex.IsMatch(inn, @"^\d{10}$|^\d{12}$");
        }

        /// <summary>
        /// Валидация номера телефона
        /// </summary>
        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // Телефон не обязателен

            // Разрешаем различные форматы телефонов
            return Regex.IsMatch(phone, @"^[\d\s\-\+\(\)]+$");
        }

        /// <summary>
        /// Валидация номера банковского счета (20 цифр)
        /// </summary>
        public static bool ValidateAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;

            return Regex.IsMatch(accountNumber, @"^\d{20}$");
        }

        /// <summary>
        /// Валидация года выпуска фильма
        /// </summary>
        public static bool ValidateReleaseYear(int year)
        {
            return year >= 1895 && year <= DateTime.Now.Year + 2;
        }

        /// <summary>
        /// Валидация числа посадочных мест
        /// </summary>
        public static bool ValidateSeatsCount(int seatsCount)
        {
            return seatsCount > 0 && seatsCount <= 10000;
        }

        /// <summary>
        /// Валидация денежной суммы
        /// </summary>
        public static bool ValidateAmount(decimal amount)
        {
            return amount >= 0;
        }

        /// <summary>
        /// Валидация дат аренды
        /// </summary>
        public static bool ValidateRentalDates(DateTime startDate, DateTime endDate)
        {
            return endDate >= startDate;
        }

        /// <summary>
        /// Валидация строки (не пустая)
        /// </summary>
        public static bool ValidateRequiredString(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации ИНН
        /// </summary>
        public static string GetINNErrorMessage()
        {
            return "ИНН должен содержать 10 или 12 цифр";
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации телефона
        /// </summary>
        public static string GetPhoneErrorMessage()
        {
            return "Некорректный формат номера телефона";
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации номера счета
        /// </summary>
        public static string GetAccountNumberErrorMessage()
        {
            return "Номер счета должен содержать 20 цифр";
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации года
        /// </summary>
        public static string GetReleaseYearErrorMessage()
        {
            return $"Год выпуска должен быть от 1895 до {DateTime.Now.Year + 2}";
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации количества мест
        /// </summary>
        public static string GetSeatsCountErrorMessage()
        {
            return "Количество мест должно быть от 1 до 10000";
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации денежной суммы
        /// </summary>
        public static string GetAmountErrorMessage()
        {
            return "Сумма должна быть положительной";
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации дат
        /// </summary>
        public static string GetRentalDatesErrorMessage()
        {
            return "Дата окончания должна быть позже или равна дате начала";
        }

        /// <summary>
        /// Получить сообщение об ошибке обязательного поля
        /// </summary>
        public static string GetRequiredFieldErrorMessage(string fieldName)
        {
            return $"Поле \"{fieldName}\" обязательно для заполнения";
        }

        /// <summary>
        /// Валидация числовой строки (только цифры)
        /// </summary>
        public static bool ValidateNumericString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true; // Пустая строка допустима для необязательных полей
            
            return Regex.IsMatch(value, @"^\d+$");
        }

        /// <summary>
        /// Получить сообщение об ошибке валидации числовой строки
        /// </summary>
        public static string GetNumericStringErrorMessage(string fieldName)
        {
            return $"Поле \"{fieldName}\" должно содержать только цифры";
        }
    }
}

