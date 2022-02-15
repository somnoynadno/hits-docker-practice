# Node Application

Фотогалерея на NodeJS

## Стек технологий

- NodeJS (веб-фреймворк Express)

- MySQL

## Локальный запуск

*примеры команд ниже указаны для unix-подобных ОС, с виндой разбирайтесь сами*

1. Ставим БД MySQL (любой версии) для своей ОС 
([тык](https://dev.mysql.com/doc/mysql-installation-excerpt/5.7/en/)), 
запускаем и проверяем работоспособность

2. Устанавливаем [npm и nodejs](https://nodejs.org/en/download/) (hard way)

3. ... или ставим триалку WebStorm, которая сама поставит всё необходимое (easy way)

4. Меняем креды в файле `db.js`, чтобы они соответствовали вашим

5. Загоняем в БД скрипт `init.sql` для создания таблиц по умолчанию

6. Ставим зависимости командой ` $ npm install`

7. Запускаем приложение командой ` $ npm start`

## Дополнительно

API доступен по адресу http://localhost:3000

[Документация Postman](https://www.getpostman.com/collections/87776fd4abf2af4bdb6d)
