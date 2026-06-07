using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TaxiApp.Models;

namespace TaxiApp
{
    public partial class OrderHistoryWindow : Window
    {
        private readonly TaxiDbContext _context;

        public OrderHistoryWindow(TaxiDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadHistory();
        }

        private void LoadHistory()
        {
            var history = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Driver)
                .Where(o => o.Status == "Завершён" || o.Status == "Отменён")
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            dgHistory.ItemsSource = history;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var query = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Driver)
                .Where(o => o.Status == "Завершён" || o.Status == "Отменён")
                .AsQueryable();

            if (dpFrom.SelectedDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= dpFrom.SelectedDate.Value.Date);
            }

            if (dpTo.SelectedDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= dpTo.SelectedDate.Value.Date.AddDays(1));
            }

            dgHistory.ItemsSource = query.OrderByDescending(o => o.OrderDate).ToList();
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            var allOrders = _context.Orders
                .Where(o => o.Status == "Завершён" || o.Status == "Отменён")
                .ToList();

            var completed = allOrders.Count(o => o.Status == "Завершён");
            var cancelled = allOrders.Count(o => o.Status == "Отменён");
            var totalRevenue = allOrders.Where(o => o.Status == "Завершён").Sum(o => o.Price);

            MessageBox.Show(
                $"📊 Статистика:\n\n" +
                $"✅ Выполнено: {completed}\n" +
                $"❌ Отменено: {cancelled}\n" +
                $"💰 Общая выручка: {totalRevenue:N2} ₽",
                "Статистика",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}