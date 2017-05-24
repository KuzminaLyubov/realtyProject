using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RealtyApp.Models;
using System.Data.Entity;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RealtyDatabaseEntities _realtyDatabase = new RealtyDatabaseEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _realtyDatabase.Dispose();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await _realtyDatabase.Users.LoadAsync();

            // LoadAsync - asynchronous call without blocking the window thread
            await _realtyDatabase.RealEstates.LoadAsync();

            // LoadAsync - asynchronous call without blocking the window thread
            await _realtyDatabase.Owners.LoadAsync();

            // LoadAsync - asynchronous call without blocking the window thread
            await _realtyDatabase.Pictures.LoadAsync();

            Pages.LoginPage = new LoginPage(_realtyDatabase);
            Pages.MainPage = new MainPage(_realtyDatabase);

            frameMain.Navigate(Pages.LoginPage);

        }
    }
}

