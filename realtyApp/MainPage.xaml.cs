using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RealtyApp.Models;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using Microsoft.Win32;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private RealtyDatabaseEntities _realtyDatabase;

        public MainPage(RealtyDatabaseEntities realtyDatabase)
        {
            InitializeComponent();

            _realtyDatabase = realtyDatabase;

        }

        public void SetReadOnly()
        {
            _buttonAdd.Visibility = Visibility.Hidden;
            _buttonDelete.Visibility = Visibility.Hidden;
            _buttonEdit.Visibility = Visibility.Hidden;
            _buttonEditOwner.Visibility = Visibility.Hidden;
            _buttonDeleteOwner.Visibility = Visibility.Hidden;
            _buttonAddOwner.Visibility = Visibility.Hidden;
            _buttonAddImages.Visibility = Visibility.Hidden;
            _buttonDeleteImages.Visibility = Visibility.Hidden;
        }

        public void RefreshListBox()
        {
            var selectedItem = (RealEstate)_realtyListBox.SelectedItem;
            _realtyListBox.ItemsSource = null;
            _realtyListBox.ItemsSource = _realtyDatabase.RealEstates.Local;

            _realtyListBox.SelectedItem = null;
            _realtyListBox.SelectedItem = selectedItem;

        }

        private void Search()
        {
            _realtyListBox.ItemsSource = _realtyDatabase.RealEstates.Local
                .Where(realEstate => realEstate.Address.ToLower()
                            .Contains(_searchTextBox.Text.ToLower()));
        }

        private void Page_SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_searchTextBox.Text.Trim()))
                RefreshListBox();
            else
                Search();
        }

        private void Page_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex != -1)
            {
                RealEstate realEstate = (RealEstate)_realtyListBox.SelectedItem;

                MessageBoxResult result = MessageBox.Show(
                    $"Вы действительно хотите удалить \"{realEstate}\"?",
                    "Внимание!",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    foreach (var p in _realtyDatabase.Pictures.Local.Where(image => image.RealEstateId == realEstate.Id).Reverse())
                    {
                        _realtyDatabase.Pictures.Local.Remove(p);
                    }

                    _realtyDatabase.RealEstates.Local.Remove(realEstate);

                    RefreshListBox();
                }
            }
        }

        private void Page_RealtyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            for (int i = _dockPanelImages.Children.Count - 1; i >= 0; i--)
            {
                _dockPanelImages.Children.RemoveAt(i);
            }

            foreach (var picture in _realtyDatabase.Pictures.Local.Where(image => image.RealEstateId == selectedRealEstate.Id))
            {
                _dockPanelImages.Children.Add(new Image
                {
                    Source = ByteToImage(picture.Content),
                    Width = 150,
                    Height = 150,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5)
                });
            }
        }

        private static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage b = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            b.BeginInit();
            b.StreamSource = ms;
            b.EndInit();

            ImageSource imgSrc = b as ImageSource;

            return imgSrc;
        }

        private void Page_ButtonDeleteOwner_Click(object sender, RoutedEventArgs e)
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

                        if (result2 != MessageBoxResult.Yes)
                        {
                            return;
                        }

                        foreach (var r in _realtyDatabase.RealEstates.Local.Where(r => r.OwnerID == realEstate.Owner.Id).Reverse())
                        {
                            // удаляем сначала все изображения недвижимости
                            foreach (var p in _realtyDatabase.Pictures.Local.Where(image => image.RealEstateId == r.Id).Reverse())
                            {
                                _realtyDatabase.Pictures.Local.Remove(p);
                            }

                            // затем саму недвижимость
                            _realtyDatabase.RealEstates.Local.Remove(r);
                        }
                    }

                    // и только потом владельца
                    _realtyDatabase.Owners.Local.Remove(realEstate.Owner);

                    _realtyDatabase.SaveChangesAsync();

                    RefreshListBox();

                }
            }
        }

        private void Page_ButtonDeleteImages_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex != -1)
            {
                RealEstate realEstate = (RealEstate)_realtyListBox.SelectedItem;

                if (realEstate.Pictures.Any())
                {
                    MessageBoxResult result = MessageBox.Show(
                    $"Вы действительно хотите удалить изображения \"{realEstate}\"?",
                    "Внимание!",
                    MessageBoxButton.YesNo);

                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }

                    // удаляем все изображения недвижимости
                    foreach (var p in _realtyDatabase.Pictures.Local.Where(image => image.RealEstateId == realEstate.Id).Reverse())
                    {
                        _realtyDatabase.Pictures.Local.Remove(p);
                    }

                    _realtyDatabase.SaveChangesAsync();

                    RefreshListBox();

                }
            }
        }

        private void Page_ButtonAddImages_Click(object sender, RoutedEventArgs e)
        {
            RealEstate realEstate = (RealEstate)_realtyListBox.SelectedItem;

            if (realEstate == null)
                return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Images (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    _realtyDatabase.Pictures.Local.Add(new Picture
                    {
                        Name = Path.GetFileName(filename),
                        Content = File.ReadAllBytes(filename),
                        RealEstateId = realEstate.Id
                    });
                }

                RefreshListBox();
            }
        }

        private void Page_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Pages.RealEstatePage.RealEstate = new RealEstate();
            Pages.RealEstatePage.CurrentRegime = PageRegime.Add;
            NavigationService.Navigate(Pages.RealEstatePage);
        }

        private void Page_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex == -1)
                return;

            Pages.RealEstatePage.RealEstate = (RealEstate)_realtyListBox.SelectedItem;
            Pages.RealEstatePage.CurrentRegime = PageRegime.Edit;
            NavigationService.Navigate(Pages.RealEstatePage);

        }

        private void Page_ButtonAddOwner_Click(object sender, RoutedEventArgs e)
        {
            Pages.RealEstateOwnerPage.RealEstateOwner = new Owner();
            Pages.RealEstateOwnerPage.CurrentRegime = PageRegime.Add;
            NavigationService.Navigate(Pages.RealEstateOwnerPage);
        }

        private void Page_ButtonEditOwner_Click(object sender, RoutedEventArgs e)
        {
            if (_realtyListBox.SelectedIndex == -1)
                return;

            RealEstate realEstate = (RealEstate)_realtyListBox.SelectedItem;
            Pages.RealEstateOwnerPage.RealEstateOwner = realEstate.Owner;
            Pages.RealEstateOwnerPage.CurrentRegime = PageRegime.Edit;
            NavigationService.Navigate(Pages.RealEstateOwnerPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;

            RefreshListBox();

            if (Pages.RealEstatePage.CurrentRegime != PageRegime.Cancel)
            {
                Pages.RealEstatePage.CurrentRegime = PageRegime.Cancel;
            }

            if (Pages.RealEstateOwnerPage.CurrentRegime != PageRegime.Cancel)
            {
                Pages.RealEstateOwnerPage.CurrentRegime = PageRegime.Cancel;
            }

        }


    }
}

