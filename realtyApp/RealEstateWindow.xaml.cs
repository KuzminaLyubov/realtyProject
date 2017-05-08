using System.Collections.Generic;
using System.Windows;
using RealtyApp.Models;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for NewRealEstateWindow.xaml
    /// </summary>
    public partial class RealEstateWindow : Window
    {
        private RealtyDatabaseEntities _realtyDatabase;

        public RealEstateWindow(RealtyDatabaseEntities realtyDatabase)
        {
            InitializeComponent();

            _realtyDatabase = realtyDatabase;

            _labelError.Visibility = Visibility.Hidden;
        }

        private RealEstate _realEstate;

        public RealEstate RealEstate
        {
            get
            {
                return _realEstate;
            }
            set
            {
                _realEstate = value;
            }
        }

        private void _buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_textBoxTitle.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите название объекта недвижимости";
                _textBoxTitle.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_textBoxAddress.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите адрес объекта недвижимости";
                _textBoxAddress.Focus();
                return;
            }

            decimal price;

            if (!decimal.TryParse(_textBoxPrice.Text, out price)) {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите стоимость объекта недвижимости";
                _textBoxPrice.Focus();
                return;
            }
            if (price <= 100000) {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Стоимость недвижимости от 100000 руб";
                _textBoxPrice.Focus();
                return;
            }

            if (_comboBoxOwner.SelectedItem == null) {
                MessageBox.Show("Необходимо выбрать факультет");
                _comboBoxOwner.Focus();
                return;
            }

            _realEstate = new RealEstate {
                Title = _textBoxTitle.Text,
                Address = _textBoxAddress.Text,
                Price = price,
                Owner = _comboBoxOwner.SelectedItem as Owner
            };

            // Close current window
            DialogResult = true;
        }

        private void RealEstateWindowName_Loaded(object sender, RoutedEventArgs e)
        {
            _comboBoxOwner.ItemsSource = _realtyDatabase.Owners.Local;
        }

    }
}
