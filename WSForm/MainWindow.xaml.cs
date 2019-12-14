using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WSForm.Models;
using WSForm.Services;

namespace WSForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Book> data = null;
        private LoginModel loginModel = null;
        private string selectedID = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainForm_Loaded(object sender, RoutedEventArgs e)
        {
            btnAdd.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDel.IsEnabled = false;
            btnLogin.Visibility = Visibility.Visible;
            btnLogin.IsEnabled = true;
            btnLogout.Visibility = Visibility.Hidden;
            btnLogout.IsEnabled = false;
            dgBooksStatusCol.Visibility = Visibility.Hidden;

            dgBooks.SelectedValuePath = "ID";
            FetchData();
        }

        private void dgBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBooks.SelectedIndex > -1)
            {
                txtSelectedIndex.Content = string.Format("{0}", dgBooks.SelectedValue);
                selectedID = string.Format("{0}", dgBooks.SelectedValue);

                if (loginModel != null && (loginModel.role.Equals("Manager") || loginModel.role.Equals("Admin")))
                {
                    btnEdit.IsEnabled = true;
                    btnDel.IsEnabled = true;
                }
                else
                {
                    btnEdit.IsEnabled = false;
                    btnDel.IsEnabled = false;
                }
            }
            else
            {
                txtSelectedIndex.Content = "";
                selectedID = "";
                btnEdit.IsEnabled = false;
                btnDel.IsEnabled = false;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(loginModel.token);
            if (addWindow.ShowDialog() == true)
            {
                FetchData(loginModel);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var model = data.First(x => x.ID == selectedID);
            EditWindow editWindow = new EditWindow(loginModel.token, model);
            if (editWindow.ShowDialog() == true)
            {
                FetchData(loginModel);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedID) && MessageBox.Show("Bạn có chắc muốn xoá sản phẩm này?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeleteBook(loginModel, selectedID);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchModel searchModel = new SearchModel();
            string searchType = cbSearchType.Text;
            if (searchType == "Mã sách")
            {
                if (int.TryParse(txtSearch.Text, out int searchId))
                    searchModel.Id = searchId;
            }
            else if (searchType == "Tên sách")
            {
                searchModel.Name = txtSearch.Text;
            }
            else if (searchType == "Loại sách")
            {
                searchModel.BookType = txtSearch.Text;
            }
            else if (searchType == "Tác giả")
            {
                searchModel.Author = txtSearch.Text;
            }
            else if (searchType == "Giá")
            {
                var txt = txtSearch.Text.Replace(" ", "");
                if (txt.Contains("-"))
                {
                    var splited = txt.Split('-');
                    if (int.TryParse(splited[0], out int priceFrom) && int.TryParse(splited[1], out int priceTo))
                    {
                        if (priceTo >= priceFrom)
                        {
                            searchModel.PriceFrom = priceFrom;
                            searchModel.PriceTo = priceTo;
                        }
                        else
                        {
                            searchModel.PriceFrom = priceTo;
                            searchModel.PriceTo = priceFrom;
                        }
                    }
                }
                else
                {
                    if (int.TryParse(txt, out int price))
                    {
                        searchModel.PriceFrom = price;
                        searchModel.PriceTo = price;
                    }
                }
            }

            data = DatabaseService.SearchBooks(searchModel, loginModel != null ? loginModel.token : "");
            dgBooks.ItemsSource = null;
            dgBooks.ItemsSource = data;
            dgBooks.SelectedIndex = -1;

            if (loginModel != null && loginModel.role.Equals("Admin"))
            {
                dgBooksStatusCol.Visibility = Visibility.Visible;
            }
            else
            {
                dgBooksStatusCol.Visibility = Visibility.Hidden;
            }
        }

        private void FetchData(LoginModel model = null)
        {
            data = DatabaseService.FetchBooks(model != null ? model.token : "");

            dgBooks.ItemsSource = null;
            dgBooks.ItemsSource = data;
            dgBooks.SelectedIndex = -1;

            if (model != null && model.role.Equals("Admin"))
            {
                dgBooksStatusCol.Visibility = Visibility.Visible;
            }
            else
            {
                dgBooksStatusCol.Visibility = Visibility.Hidden;
            }
        }

        private void LogIn()
        {
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                loginModel = loginWindow.loginModel;

                btnLogin.IsEnabled = false;
                btnLogin.Visibility = Visibility.Hidden;

                btnLogout.IsEnabled = true;
                btnLogout.Visibility = Visibility.Visible;

                if (loginModel.role == "Admin" || loginModel.role == "Manager")
                {
                    btnAdd.IsEnabled = true;
                }

                txtLoginInfo.Content = $"{loginModel.name} - {loginModel.role}";
                FetchData(loginModel);
            }
        }

        private void LogOut()
        {
            loginModel = null;

            btnAdd.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDel.IsEnabled = false;

            btnLogout.IsEnabled = false;
            btnLogout.Visibility = Visibility.Hidden;
            btnLogin.IsEnabled = true;
            btnLogin.Visibility = Visibility.Visible;

            txtLoginInfo.Content = "";
            FetchData();
        }

        private void DeleteBook(LoginModel model, string ID)
        {
            if (DatabaseService.DeleteBook(model.token, ID))
                FetchData(model);
        }

        private void btnStatistic_Click(object sender, RoutedEventArgs e)
        {
            StatisticWindow statisticWindow = new StatisticWindow(data);
            statisticWindow.ShowDialog();
        }
    }
}