using System.Windows;
using RealtyApp.Models;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for RealEstateOwnerPage.xaml
    /// </summary>
    public partial class RealEstateOwnerPage : Page
    {
        private RealtyDatabaseEntities _realtyDatabase;
        private Owner _owner;

        public PageRegime CurrentRegime { get; set; }

        public RealEstateOwnerPage(RealtyDatabaseEntities realtyDatabase)
        {
            InitializeComponent();

            _realtyDatabase = realtyDatabase;

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

        private void Page_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_textBoxFullName.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите фамилию владельца недвижимости";
                _textBoxFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_textBoxPhoneNumber.Text) || !Regex.Match(_textBoxPhoneNumber.Text, @"^(\+[0-9]{11})$").Success)
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Введите корректный мобильный телефон владельца недвижимости";
                _textBoxPhoneNumber.Focus();
                return;
            }

           _owner.FullName = _textBoxFullName.Text;
           _owner.PhoneNumber = _textBoxPhoneNumber.Text;

            if (CurrentRegime == PageRegime.Add)
                _realtyDatabase.Owners.Local.Add(_owner);

            _realtyDatabase.SaveChanges();
            _realtyDatabase.Owners.Load();

            NavigationService.Navigate(Pages.MainPage);
        }

        private void Page_ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            CurrentRegime = PageRegime.Cancel;
            NavigationService.Navigate(Pages.MainPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;
            _textBoxFullName.Text = _owner.FullName;
            _textBoxPhoneNumber.Text = _owner.PhoneNumber;
        }

    }
}
