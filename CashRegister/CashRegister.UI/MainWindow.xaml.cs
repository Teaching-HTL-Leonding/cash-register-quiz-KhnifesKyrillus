using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CashRegister.SharedResources;

namespace CashRegister.UI
{
    public record ReceiptLineViewModel(int ProductId, string ProductName, int Amount, decimal TotalPrice);

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("https://localhost:5001"),
            Timeout = TimeSpan.FromSeconds(5)
        };

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            async Task LoadProducts()
            {
                var products = await HttpClient.GetFromJsonAsync<List<Product>>("api/products");
                if (products == null || products.Count == 0) return;
                foreach (var product in products) Products.Add(product);
            }

            Loaded += async (_, __) => await LoadProducts();
        }

        public ObservableCollection<Product> Products { get; } = new();

        public ObservableCollection<ReceiptLineViewModel> Basket { get; } = new();

        public decimal TotalSum => Basket.Sum(rl => rl.TotalPrice);

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnAddProduct(object sender, RoutedEventArgs e)
        {
            if (((Button) sender).DataContext is not Product selectedProduct) return;

            var product = Products.First(p => p.ID == selectedProduct.ID);

            Basket.Add(new ReceiptLineViewModel
                (product.ID, product.ProductName, 1, product.UnitPrice));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalSum)));
        }

        private async void OnCheckout(object sender, RoutedEventArgs e)
        {
            var dto = Basket.Select(b => new ReceiptLineDto
            {
                ProductID = b.ProductId,
                Amount = b.Amount
            }).ToList();

            var response = await HttpClient.PostAsJsonAsync("/api/receipts", dto);

            response.EnsureSuccessStatusCode();

            Basket.Clear();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalSum)));
        }
    }
}