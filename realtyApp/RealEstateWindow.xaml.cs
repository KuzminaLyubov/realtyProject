using System.Collections.Generic;
using System.Windows;
using RealtyApp.Models;
using System.Linq;

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

            if (!decimal.TryParse(_textBoxPrice.Text, out price))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите стоимость объекта недвижимости";
                _textBoxPrice.Focus();
                return;
            }

            if (price <= 100000)
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Стоимость недвижимости от 100000 руб";
                _textBoxPrice.Focus();
                return;
            }

            if (_comboBoxOwner.SelectedItem == null)
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Необходимо выбрать владельца недвижимости";
                _comboBoxOwner.Focus();
                return;
            }

            _realEstate.Title = _textBoxTitle.Text;
            _realEstate.Address = _textBoxAddress.Text;
            _realEstate.Price = price;
            _realEstate.Owner = _comboBoxOwner.SelectedItem as Owner;

            // Close current window
            DialogResult = true;
        }

        private void RealEstateWindowName_Loaded(object sender, RoutedEventArgs e)
        {
            _comboBoxOwner.ItemsSource = _realtyDatabase.Owners.Local.OrderBy(owner => owner.FullName);
            _comboBoxOwner.SelectedItem = _realEstate.Owner;
            _textBoxTitle.Text = _realEstate.Title;
            _textBoxAddress.Text = _realEstate.Address;
            _textBoxPrice.Text = _realEstate.Price.ToString();
        }

    }
}
