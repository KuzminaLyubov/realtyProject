using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RealtyApp.Models;
using System.Data.Entity;

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
            _buttonAdd.Visibility = Visibility.Hidden;
            _buttonDelete.Visibility = Visibility.Hidden;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _realtyDatabase.Dispose();
        }



        private async void Realty_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionViewSource realEstateViewSource = ((CollectionViewSource)(this.FindResource("realEstateViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            //realEstateViewSource.Source = [generic data source]
            //https://msdn.microsoft.com/en-us/library/jj574514(v=vs.113).aspx

            await _realtyDatabase.Users.LoadAsync();

            LoginWindow loginWindow = new LoginWindow(_realtyDatabase);
            loginWindow.ShowDialog();
            if (loginWindow.Regime == LoginWindow.LoginRegime.Exit)
            {
                Close();
                return;
            }

            if (loginWindow.Regime == LoginWindow.LoginRegime.Admin)
            {
                _buttonDelete.Visibility = Visibility.Visible;
                _buttonAdd.Visibility = Visibility.Visible;
            }

            // LoadAsync - asynchronous call without blocking the window thread
            await _realtyDatabase.RealEstates.LoadAsync();

            // After the data is loaded call the DbSet<T>.Local property 
            // to use the DbSet<T> as a binding source.
            realEstateViewSource.Source = _realtyDatabase.RealEstates.Local;

            await _realtyDatabase.Owners.LoadAsync();

        }

        private void RefreshListBox()
        {
            _realtyListBox.ItemsSource = null;
            _realtyListBox.ItemsSource = _realtyDatabase.RealEstates.Local;
        }

        private void Search()
        {
            _realtyListBox.ItemsSource = _realtyDatabase.RealEstates.Local
                .Where(realEstate => realEstate.Address.ToLower()
                            .Contains(_searchTextBox.Text.ToLower()));
        }

        private void _searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_searchTextBox.Text.Trim()))
                RefreshListBox();
            else
                Search();
        }

        private void _buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new RealEstateWindow(_realtyDatabase);
            if (window.ShowDialog().Value)
            {
                _realtyDatabase.RealEstates.Add(window.RealEstate);
                SaveData();
                RefreshListBox();
            }
        }

        private void _buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex != -1)
            {
                _realtyDatabase.RealEstates.Local.RemoveAt(_realtyListBox.SelectedIndex);
                SaveData();
                RefreshListBox();
            }
        }

        private void _realtyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index = -1, we set IsEnabled to false
            _buttonDelete.IsEnabled = _realtyListBox.SelectedIndex != -1;
        }

        private void SaveData()
        {
            _realtyDatabase.SaveChangesAsync();
        }

    }
}

