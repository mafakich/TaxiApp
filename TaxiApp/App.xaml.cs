using System.Windows;
using TaxiApp.Models;

namespace TaxiApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация базы данных
            using (var db = new TaxiDbContext())
            {
                db.Database.EnsureCreated();

                // Добавление тестовых данных если база пустая
                if (db.Clients.Count() == 0)
                {
                    InitializeTestData(db);
                }
            }
        }

        private void InitializeTestData(TaxiDbContext db)
        {
            // Тестовые клиенты
            var clients = new[]
            {
                new Client { Name = "Иванов Иван", Phone = "+7 (999) 123-45-67", Email = "ivanov@mail.ru" },
                new Client { Name = "Петрова Мария", Phone = "+7 (999) 234-56-78", Email = "petrova@mail.ru" },
                new Client { Name = "Сидоров Алексей", Phone = "+7 (999) 345-67-89", Email = "sidorov@mail.ru" }
            };
            db.Clients.AddRange(clients);

            // Тестовые автомобили
            var vehicles = new[]
            {
                new Vehicle { Model = "Toyota Camry", LicensePlate = "А123БВ777", Color = "Жёлтый" },
                new Vehicle { Model = "Hyundai Solaris", LicensePlate = "В456ГД777", Color = "Жёлтый" },
                new Vehicle { Model = "Kia Rio", LicensePlate = "Е789ЖЗ777", Color = "Белый" }
            };
            db.Vehicles.AddRange(vehicles);
            db.SaveChanges();

            // Тестовые водители
            var drivers = new[]
            {
                new Driver { FullName = "Козлов Дмитрий", Phone = "+7 (999) 111-22-33", LicenseNumber = "77АА123456", VehicleId = 1 },
                new Driver { FullName = "Новиков Сергей", Phone = "+7 (999) 222-33-44", LicenseNumber = "77ББ234567", VehicleId = 2 },
                new Driver { FullName = "Морозов Андрей", Phone = "+7 (999) 333-44-55", LicenseNumber = "77ВВ345678", VehicleId = 3 }
            };
            db.Drivers.AddRange(drivers);
            db.SaveChanges();

            // Тестовые заказы
            var orders = new[]
            {
                new Order {
                    OrderDate = DateTime.Now.AddDays(-2),
                    PickupAddress = "ул. Ленина, 10",
                    DropoffAddress = "пр. Мира, 25",
                    Price = 350,
                    Status = "Завершён",
                    ClientId = 1,
                    DriverId = 1
                },
                new Order {
                    OrderDate = DateTime.Now.AddDays(-1),
                    PickupAddress = "ул. Гагарина, 5",
                    DropoffAddress = "ул. Пушкина, 12",
                    Price = 280,
                    Status = "Завершён",
                    ClientId = 2,
                    DriverId = 2
                },
                new Order {
                    OrderDate = DateTime.Now,
                    PickupAddress = "пл. Победы, 1",
                    DropoffAddress = "ул. Советская, 45",
                    Price = 420,
                    Status = "В ожидании",
                    ClientId = 3,
                    DriverId = null
                }
            };
            db.Orders.AddRange(orders);
            db.SaveChanges();
        }
    }
}