Описание проекта: 
HotelBookingApi — это REST API-сервис для бронирования номеров в отеле.
Стек: C#, ASP.NET Core 8, PostgreSQL, Docker


Инструкция по запуску: 
Клонируем проект из репозитория 
git clone https://github.com/Dimmentor/HotelBookingApi.git

Переходим в каталог проекта
cd HotelBookingApi

Собираем и запускаем контейнер, миграция при этом будет применена автоматически
docker-compose up --build

Можно предварительно ознакомиться с эндпоинтами в Swagger UI
http://localhost:8080/swagger/



Предлагаю следующий гайд по проверке эндпоинтов:
1) Создание номеров (поочередно)

POST /api/rooms
{
  "number": "101",
  "type": "Standart"
}

{
  "number": "201",
  "type": "Vip"
}

{
  "number": "301",
  "type": "President"
}

2) Получение всех номеров(выдаст все созданные нами)

GET /api/rooms

3) Создание бронирования

POST /api/bookings

{
  "roomId": 3,
  "customerName": "Donald Trump",
  "startDate": "2025-06-04T00:00:00Z",
  "endDate": "2025-06-06T00:00:00Z"
}

4) Получение свободных номеров(проверяем, что забронированный номер не доступен)

GET /api/bookings/available?from=2025-06-04T00:00:00Z&to=2025-06-06T00:00:00Z

5) Получение всех бронирований

GET /api/bookings

6) Изменение существующего бронирования(так как, президенский номер только один, в него заедет более важная персона) 

PUT /api/bookings/1

{
  "id": 1,
  "roomId": 3,
  "customerName": "Vladimir Putin",
  "startDate": "2025-06-04T00:00:00Z",
  "endDate": "2025-06-07T00:00:00Z"
}
(После этого можно проверить /api/bookings, изменено ли бронирование)

7) Удаление бронирования

DELETE /api/bookings/1
(После этого можно снова проверить /api/bookings, удалено ли бронирование)

