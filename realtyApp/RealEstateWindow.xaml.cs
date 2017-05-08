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

        public RealEstateWindow(List<Owner> owners)
        {
            InitializeComponent();

            comboBoxFaculties.ItemsSource = owners;
        }

        RealEstate _newRealEstate;

        public RealEstate NewRealEstate
        {
            get {
                return _newRealEstate;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
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

            if (comboBoxFaculties.SelectedItem == null) {
                MessageBox.Show("Необходимо выбрать факультет");
                comboBoxFaculties.Focus();
                return;
            }
            
            _newLecturer = new Lecturer(textBoxFio.Text,
                rating);
            _newLecturer.Faculty = comboBoxFaculties.SelectedItem as Faculty;
            // Close current window
            DialogResult = true;
        }
    }
}
