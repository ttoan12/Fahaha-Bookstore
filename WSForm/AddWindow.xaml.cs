using Microsoft.Win32;
using System;
using System.Windows;
using WSForm.Models;

namespace WSForm
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private Book book = new Book();
        private string filePath = "";
        private string token = "";

        public AddWindow(string token)
        {
            this.token = token;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNgayXB.SelectedDate = DateTime.Now;
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image Files|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                txtImgPath.Content = filePath;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAuthorName.Text) ||
                string.IsNullOrWhiteSpace(txtNXB.Text) ||
                string.IsNullOrWhiteSpace(txtNgayXB.SelectedDate.Value.ToString("yyyy/MM/dd")) ||
                string.IsNullOrWhiteSpace(txtSize.Text) ||
                string.IsNullOrWhiteSpace(txtPages.Text) ||
                string.IsNullOrWhiteSpace(txtCover.Text) ||
                string.IsNullOrWhiteSpace(txtBookTypeName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtCount.Text) ||
                string.IsNullOrWhiteSpace(filePath))
            {
                MessageBox.Show("Hãy nhập đủ thông tin");
                return;
            }

            book.Name = txtName.Text;
            book.AuthorName = txtAuthorName.Text;
            book.PublishingCompany = txtNXB.Text;
            book.PublishingDate = txtNgayXB.Text;
            book.Size = txtSize.Text;
            book.NumberOfPages = txtPages.Text;
            book.CoverType = txtCover.Text;
            book.BookTypeName = txtBookTypeName.Text;
            book.Price = txtPrice.Text;
            book.Count = txtCount.Text;

            var result = Services.DatabaseService.PostBook(token, book, filePath);

            if (result == false)
            {
                MessageBox.Show("Thêm thất bại!\nHãy kiểm tra lại thông tin!");
                return;
            }
            else
            {
                MessageBox.Show("Thêm thành công!");
            }

            DialogResult = result;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}