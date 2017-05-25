using System.Windows;
using RealtyApp.Models;
using System.Linq;
using System.Windows.Controls;
using System.Data.Entity;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for RealEstatePage.xaml
    /// </summary>
    public partial class RealEstatePage : Page
    {
        private RealtyDatabaseEntities _realtyDatabase;

        public PageRegime CurrentRegime { get; set; }

        public RealEstatePage(RealtyDatabaseEntities realtyDatabase)
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

        private void Page_ButtonSave_Click(object sender, RoutedEventArgs e)
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

            if (CurrentRegime == PageRegime.Add)
                _realtyDatabase.RealEstates.Local.Add(_realEstate);

            _realtyDatabase.SaveChanges();
            _realtyDatabase.RealEstates.Load();

            NavigationService.Navigate(Pages.MainPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;

            _comboBoxOwner.ItemsSource = _realtyDatabase.Owners.Local.OrderBy(owner => owner.FullName);
            _comboBoxOwner.SelectedItem = _realEstate.Owner;
            _textBoxTitle.Text = _realEstate.Title;
            _textBoxAddress.Text = _realEstate.Address;
            _textBoxPrice.Text = _realEstate.Price.ToString();
        }

        private void Page_ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            CurrentRegime = PageRegime.Cancel;
            NavigationService.Navigate(Pages.MainPage);
        }
    }
}
