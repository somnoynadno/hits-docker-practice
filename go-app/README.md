# Go Application

Сервис для работы с заметками на языке Go

## Стек технологий

- Go (веб-фреймворк mux, ORM gorm)

- PostgreSQL

## Локальный запуск

*примеры команд ниже указаны для unix-подобных ОС, с виндой разбирайтесь сами*

1. Ставим БД PostgreSQL, запускаем и проверяем работоспособность

2. Устанавливаем Go для своей ОС (hard way)

3. ... или ставим триалку Goland, которая сама всё подтянет (easy way)

4. Передаем креды для подключения к БД как переменные окружения 
(см. `.env-example`) или хардкодим их в файле `db/db.go`

5. Подгружаем необходимые зависимости командой ` $ go mod tidy`

6. Собираем приложение командой ` $ go build -o main .`

7. Запускаем созданный бинарный файл как ` $ ./main`

## Дополнительно

API доступен по адресу http://localhost:8080

[Документация Postman](https://www.getpostman.com/collections/92e55648801ba4ab0fb5)