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
            _buttonEdit.Visibility = Visibility.Hidden;
            _buttonEditOwner.Visibility = Visibility.Hidden;
            _buttonDeleteOwner.Visibility = Visibility.Hidden;
            _buttonAddOwner.Visibility = Visibility.Hidden;

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
                _buttonEdit.Visibility = Visibility.Visible;
                _buttonAddOwner.Visibility = Visibility.Visible;
                _buttonDeleteOwner.Visibility = Visibility.Visible;
                _buttonEditOwner.Visibility = Visibility.Visible;
            }

            // LoadAsync - asynchronous call without blocking the window thread
            await _realtyDatabase.RealEstates.LoadAsync();

            // After the data is loaded call the DbSet<T>.Local property 
            // to use the DbSet<T> as a binding source.
            realEstateViewSource.Source = _realtyDatabase.RealEstates.Local.OrderBy(realEstate => realEstate.Address);

            // LoadAsync - asynchronous call without blocking the window thread
            await _realtyDatabase.Owners.LoadAsync();

        }

        private void RefreshListBox()
        {
            _realtyListBox.ItemsSource = null;
            _realtyListBox.ItemsSource = _realtyDatabase.RealEstates.Local;

            _realtyListBox.SelectedItem = null;
            _realtyListBox.SelectedItem = (RealEstate)_realtyListBox.SelectedItem;

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
            window.RealEstate = new RealEstate();
            if (window.ShowDialog().Value)
            {
                _realtyDatabase.RealEstates.Add(window.RealEstate);
                SaveData();
                _realtyListBox.SelectedItem = window.RealEstate;
                RefreshListBox();

            }
        }

        private void _buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex == -1)
                return;

            var window = new RealEstateWindow(_realtyDatabase);
            window.RealEstate = (RealEstate)_realtyListBox.SelectedItem;
            if (window.ShowDialog().Value)
            {
                SaveData();
                _realtyListBox.SelectedItem = window.RealEstate;
                RefreshListBox();
            }
        }

        private void _buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex != -1)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы действительно хотите удалить \"{_realtyListBox.SelectedItem}\"?",
                    "Внимание!",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _realtyDatabase.RealEstates.Local.RemoveAt(_realtyListBox.SelectedIndex);
                    SaveData();
                    RefreshListBox();
                }
            }
        }

        private void _realtyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RealEstate selectedRealEstate;

            if (_realtyListBox.SelectedIndex == -1)
            {
                selectedRealEstate = new RealEstate { Owner = new Owner() };
            }
            else
            {
                selectedRealEstate = (RealEstate)_realtyListBox.SelectedItem;
            }

            _textBlockTitle.Text = selectedRealEstate.Title;
            _textBlockAddress.Text = selectedRealEstate.Address;
            _textBlockPrice.Text = selectedRealEstate.Price.ToString();
            _textBlockFullName.Text = selectedRealEstate.Owner.FullName;
            _textBlockPhoneNumber.Text = selectedRealEstate.Owner.PhoneNumber;
        }

        private async void SaveData()
        {
            await _realtyDatabase.SaveChangesAsync();
        }

        private void _buttonAddOwner_Click(object sender, RoutedEventArgs e)
        {
            var window = new RealEstateOwnerWindow();
            window.RealEstateOwner = new Owner();
            if (window.ShowDialog().Value)
            {
                _realtyDatabase.Owners.Add(window.RealEstateOwner);
                SaveData();
                RefreshListBox();

            }
        }

        private void _buttonEditOwner_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex == -1)
                return;

            var window = new RealEstateOwnerWindow();
            RealEstate realEstate = (RealEstate)_realtyListBox.SelectedItem;
            window.RealEstateOwner = realEstate.Owner;
            if (window.ShowDialog().Value)
            {
                SaveData();
                RefreshListBox();
            }
        }

        private void _buttonDeleteOwner_Click(object sender, RoutedEventArgs e)
        {

            if (_realtyListBox.SelectedIndex != -1)
            {
                RealEstate realEstate = (RealEstate)_realtyListBox.SelectedItem;

                MessageBoxResult result = MessageBox.Show(
                    $"Вы действительно хотите удалить владельца \"{realEstate.Owner}\"?",
                    "Внимание!",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {

                    if (realEstate.Owner.RealEstates.Any())
                    {
                        MessageBoxResult result2 = MessageBox.Show(
                        $"У выбранного владельца есть объект(ы) недвижимости. Удалить всё?",
                        "Внимание!",
                        MessageBoxButton.YesNo);

                        if (result2 == MessageBoxResult.Yes)
                        {
                            foreach (var r in realEstate.Owner.RealEstates.Reverse())
                            {
                                _realtyDatabase.RealEstates.Local.Remove(r);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                    _realtyDatabase.Owners.Local.Remove(realEstate.Owner);
                    SaveData();
                    RefreshListBox();

                }
            }
        }
    }
}

