using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TaxiApp.Models;

namespace TaxiApp
{
    public partial class DriversWindow : Window
    {
        private readonly TaxiDbContext _context;

        public DriversWindow(TaxiDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadDrivers();
        }

        private void LoadDrivers()
        {
            var drivers = _context.Drivers
                .Include(d => d.Vehicle)
                .ToList();
            dgDrivers.ItemsSource = drivers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция добавления водителя (упрощённая версия)",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrivers.SelectedItem == null)
            {
                MessageBox.Show("Выберите водителя для редактирования!",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Функция редактирования водителя (упрощённая версия)",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}