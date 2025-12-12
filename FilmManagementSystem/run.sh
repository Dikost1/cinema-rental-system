#!/bin/bash

# Скрипт для запуска системы учета кинофильмов

echo "=== Запуск системы учета кинофильмов ==="
echo ""

# Проверка наличия .NET
if ! command -v dotnet &> /dev/null
then
    echo "❌ .NET не установлен!"
    echo "Установите .NET 6.0 или выше с https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✅ .NET найден: $(dotnet --version)"
echo ""

# Проверка наличия примеров данных
if [ ! -f "suppliers.txt" ] && [ -f "suppliers_example.txt" ]; then
    echo "📋 Копирование примеров данных..."
    cp suppliers_example.txt suppliers.txt
    cp films_example.txt films.txt
    echo "✅ Примеры данных скопированы"
    echo ""
fi

# Сборка и запуск
echo "🔨 Сборка проекта..."
dotnet build --verbosity quiet

if [ $? -eq 0 ]; then
    echo "✅ Сборка завершена успешно"
    echo ""
    echo "▶️  Запуск приложения..."
    echo ""
    dotnet run
else
    echo "❌ Ошибка при сборке проекта"
    exit 1
fi
