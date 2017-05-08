using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        public enum LoginRegime
        {
          Exit,
          ReadOnly,
          Admin
        }

        public LoginRegime Regime { get; set; } = LoginRegime.Exit;

        public LoginWindow()
        {
            InitializeComponent();
            // При загрузке страницы передаем фокус первому текстбоксу, чтобы
            // сразу можно было вводить имя пользователя
            _textBoxLogin.Focus();
            _labelError.Visibility = Visibility.Hidden;
        }

        private string CalculateHash(string password)
        {
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e) {

            // Хэш зарегистрированного пользователя должен браться из хранилища
            // данных программы
            var hash = CalculateHash("qwerty");

            if (_textBoxLogin.Text == "lyubov" && CalculateHash(_passwordBox.Password) == hash)
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
