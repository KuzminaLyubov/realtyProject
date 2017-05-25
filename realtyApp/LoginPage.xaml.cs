using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using RealtyApp.Models;
using System.Windows.Controls;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        private RealtyDatabaseEntities _realtyDatabase;

        public LoginPage(RealtyDatabaseEntities realtyDatabase)
        {
            InitializeComponent();
            // При загрузке страницы передаем фокус первому текстбоксу, чтобы
            // сразу можно было вводить имя пользователя
            _textBoxLogin.Focus();
            _labelError.Visibility = Visibility.Hidden;
            _realtyDatabase = realtyDatabase;
        }

        private string CalculateHash(string password)
        {
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private bool AdminLoginSuccessful()
        {
            // Хэш зарегистрированного пользователя должен браться из базы данных программы
            var hashFromDb = _realtyDatabase.Users.Local
                .Where(user => user.Login == _textBoxLogin.Text)
                .Select(user => Convert.ToBase64String(user.HashedPassword))
                .SingleOrDefault();

            if (hashFromDb == null)
                return false;

            return CalculateHash(_passwordBox.Password) == hashFromDb;
        }
        private void Page_ButtonReadonly_Click(object sender, RoutedEventArgs e)
        {
            Pages.MainPage.SetReadOnly();
            NavigationService.Navigate(Pages.MainPage);
        }

        private void Page_ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (AdminLoginSuccessful())
            {
                _labelError.Visibility = Visibility.Hidden;
                NavigationService.Navigate(Pages.MainPage);
            }
            else
            {
                _labelError.Visibility = Visibility.Visible;
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Using keyboard handling on the page level
            if (e.Key == Key.Enter)
                Page_ButtonLogin_Click(null, null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;
        }
    }
}
