using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using TaxiApp.Models;

namespace TaxiApp
{
    public partial class MainWindow : Window
    {
        private TaxiDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new TaxiDbContext();
            LoadOrders();
        }

        private void LoadOrders()
        {
            var orders = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Driver)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            dgOrders.ItemsSource = orders;
            UpdateStatistics(orders);
        }

        private void UpdateStatistics(List<Order> orders)
        {
            txtTotalOrders.Text = $"Всего заказов: {orders.Count}";
            txtPendingOrders.Text = $"В ожидании: {orders.Count(o => o.Status == "В ожидании")}";
            txtCompletedOrders.Text = $"Завершено: {orders.Count(o => o.Status == "Завершён")}";
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewOrderWindow(_context);
            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }
        }

        private void btnDrivers_Click(object sender, RoutedEventArgs e)
        {
            var window = new DriversWindow(_context);
            window.ShowDialog();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderHistoryWindow(_context);
            window.ShowDialog();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверка на инициализацию
            if (cmbStatus == null || dgOrders == null)
                return;

            var selected = (cmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var query = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Driver)
                .AsQueryable();

            if (!string.IsNullOrEmpty(selected) && selected != "Все заказы")
            {
                query = query.Where(o => o.Status == selected);
            }

            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var search = txtSearch.Text.ToLower();
                query = query.Where(o =>
                    (o.Client != null && o.Client.Name.ToLower().Contains(search)) ||
                    o.PickupAddress.ToLower().Contains(search) ||
                    o.DropoffAddress.ToLower().Contains(search));
            }

            dgOrders.ItemsSource = query.OrderByDescending(o => o.OrderDate).ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbStatus_SelectionChanged(null, null);
        }
    }
}