using System.Windows;
using WSForm.Models;

namespace WSForm
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginModel loginModel = null;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            loginModel = Services.DatabaseService.Login(username, password);
            if (loginModel != null)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công!\nVui lòng kiểm tra lại tài khoản và mật khẩu.", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}