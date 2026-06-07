using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using TaxiApp.Models;

namespace TaxiApp
{
    public partial class NewOrderWindow : Window
    {
        private readonly TaxiDbContext _context;

        public NewOrderWindow(TaxiDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка клиентов
            var clients = _context.Clients.ToList();
            foreach (var client in clients)
            {
                cmbClient.Items.Add(new ComboBoxItem
                {
                    Content = $"{client.Name} ({client.Phone})",
                    Tag = client.Id
                });
            }

            // Загрузка водителей
            var drivers = _context.Drivers.Include(d => d.Vehicle).ToList();
            foreach (var driver in drivers)
            {
                var vehicleInfo = driver.Vehicle != null ?
                    $" - {driver.Vehicle.Model} ({driver.Vehicle.LicensePlate})" : "";
                cmbDriver.Items.Add(new ComboBoxItem
                {
                    Content = $"{driver.FullName}{vehicleInfo}",
                    Tag = driver.Id
                });
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка обязательных полей
                if (string.IsNullOrWhiteSpace(txtPickup.Text) ||
                    string.IsNullOrWhiteSpace(txtDropoff.Text))
                {
                    MessageBox.Show("Пожалуйста, укажите адрес подачи и назначения!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                decimal price = 0;
                if (!decimal.TryParse(txtPrice.Text, out price))
                {
                    MessageBox.Show("Некорректная стоимость!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Определение клиента
                int? clientId = null;
                var selectedClient = cmbClient.SelectedItem as ComboBoxItem;
                if (selectedClient != null && selectedClient.Tag != null)
                {
                    clientId = (int)selectedClient.Tag;
                }
                else if (!string.IsNullOrWhiteSpace(txtNewClientName.Text))
                {
                    // Создание нового клиента
                    var newClient = new Client
                    {
                        Name = txtNewClientName.Text,
                        Phone = txtPhone.Text,
                        Email = ""
                    };
                    _context.Clients.Add(newClient);
                    _context.SaveChanges();
                    clientId = newClient.Id;
                }
                else
                {
                    MessageBox.Show("Выберите клиента или введите данные нового!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Определение водителя
                int? driverId = null;
                var selectedDriver = cmbDriver.SelectedItem as ComboBoxItem;
                if (selectedDriver != null && selectedDriver.Tag != null)
                {
                    driverId = (int)selectedDriver.Tag;
                }

                // Создание заказа
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    PickupAddress = txtPickup.Text,
                    DropoffAddress = txtDropoff.Text,
                    Price = price,
                    Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "В ожидании",
                    ClientId = clientId,
                    DriverId = driverId
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                MessageBox.Show("Заказ успешно создан!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заказа: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}