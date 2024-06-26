# MusicStoreInfo
Этот проект представляет собой Web Api "Торговая площадка музыкальных альбомов", написаное в рамках курсовой работы за 3 курс по предмету "Программирование систем с серверами баз данных". 

## Техническое задание
В информационной системе, содержащей сведения о музыкальных магазинах города нужны данные о магазинах (название магазина, район города, тип собственности (государственная, частная, ЗАО, ООО, …), год открытия, телефон) и музыкальных альбомах (название альбома, тип среды (CD, DVD, кассета,…), какой фирмой выпущен (название фирмы, город, телефон), дата выпуска, время звучания (в минутах), количество песен, в каких магазинах продается (дата поступления в магазин, количество экземпляров, цена за альбом). 

## Особенности
- 3 роли:клиент, менеджер, администратор (разграничения доступа к данным)
- Кастомная система регистрации и авторизации
- CRUD операции над всеми данными, а также сортировка, фильтрация и поиск.
- Админ-панель
- Возможность добавления товара в корзину и оформление заказа
- Возможность давать оценку товару и оставлять отзыв
- Модуль с быстрой генерацией записей к БД
- Вывод графика с доходами магазина за определённые временные промежутки

## Используемые технологии и подходы
- Clean Architecture и MVC
- JWT
- DI
- MSSQL
- Razor Pages
- Bootstrap
- EF Core 8
- ASP.NET Core MVC

## Хранение данных
В качестве БД была выбрана MSSQL, для ускорение запросов были использованы индексы.

## Как запустить
### Для запуска приложения выполните следующие шаги:

1. Клонируйте репозиторий.
2. Откройте файл решения в Visual Studio.
3. Постройте решение.
4. Запустите приложение.
