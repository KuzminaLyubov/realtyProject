using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using RealtyApp.Models;
using System.Data.Entity;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public enum LoginRegime
        {
            Exit,
            ReadOnly,
            Admin
        }

        public LoginRegime Regime { get; set; } = LoginRegime.Exit;

        private RealtyDatabaseEntities _realtyDatabase;

        public LoginWindow(RealtyDatabaseEntities realtyDatabase)
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

        private async void RealtyLogin_Loaded(object sender, RoutedEventArgs e)
        {
            await _realtyDatabase.Users.LoadAsync();
        }

        private bool AdminLoginSuccessful()
        {
            // Хэш зарегистрированного пользователя должен браться из хранилища
            // данных программы
            var hashFromDb = _realtyDatabase.Users.Local
                .Where(user => user.Login == _textBoxLogin.Text)
                .Select(user => Convert.ToBase64String(user.Password))
                .SingleOrDefault();

            if (hashFromDb == null)
                return false;

            return CalculateHash(_passwordBox.Password) == hashFromDb;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {

            if (AdminLoginSuccessful())
            {
                Regime = LoginRegime.Admin;
                _labelError.Visibility = Visibility.Hidden;
                Close();
            }
            else
            {
                Regime = LoginRegime.Exit;
                _labelError.Visibility = Visibility.Visible;
                _passwordBox.Clear();
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Using keyboard handling on the page level
            if (e.Key == Key.Enter)
                buttonLogin_Click(null, null);
        }

        private void _buttonReadonly_Click(object sender, RoutedEventArgs e)
        {
            Regime = LoginRegime.ReadOnly;
            Close();
        }

    }
}
