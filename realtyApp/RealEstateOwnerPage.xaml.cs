using System.Collections.Generic;
using System.Windows;
using RealtyApp.Models;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for RealEstateOwnerWindow.xaml
    /// </summary>
    public partial class RealEstateOwnerWindow : Window
    {
        private Owner _owner;

        public RealEstateOwnerWindow()
        {
            InitializeComponent();

            _labelError.Visibility = Visibility.Hidden;
        }

        public Owner RealEstateOwner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        private void _buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_textBoxFullName.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите фамилию владельца недвижимости";
                _textBoxFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_textBoxPhoneNumber.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите мобильный телефон владельца недвижимости";
                _textBoxPhoneNumber.Focus();
                return;
            }


           _owner.FullName = _textBoxFullName.Text;
           _owner.PhoneNumber = _textBoxPhoneNumber.Text;
            
            DialogResult = true;
        }

        private void RealEstateOwnerWindowName_Loaded(object sender, RoutedEventArgs e)
        {
            _textBoxFullName.Text = _owner.FullName;
            _textBoxPhoneNumber.Text = _owner.PhoneNumber;
        }
    }
}
