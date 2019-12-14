using Microsoft.Win32;
using System;
using System.Windows;
using WSForm.Models;

namespace WSForm
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private Book book = new Book();
        private string filePath = "";
        private string token = "";

        public EditWindow(string token, Book bookModel)
        {
            this.token = token;
            this.book = bookModel;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNgayXB.SelectedDate = DateTime.Now;

            txtName.Text = book.Name;
            txtAuthorName.Text = book.AuthorName;
            txtNXB.Text = book.PublishingCompany;
            txtNgayXB.SelectedDate = DateTime.Parse(book.PublishingDate);
            txtSize.Text = book.Size;
            txtPages.Text = book.NumberOfPages;
            txtCover.Text = book.CoverType;
            txtBookTypeName.Text = book.BookTypeName;
            txtPrice.Text = book.Price;
            txtCount.Text = book.Count;
            txtImgPath.Content = "Không đổi";
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
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
                string.IsNullOrWhiteSpace(txtCount.Text))
            {
                MessageBox.Show("Hãy nhập đủ thông tin");
                return;
            }

            book.Name = txtName.Text;
            book.AuthorName = txtAuthorName.Text;
            book.PublishingCompany = txtNXB.Text;
            book.PublishingDate = txtNgayXB.SelectedDate.Value.ToString("yyyy/MM/dd");
            book.Size = txtSize.Text;
            book.NumberOfPages = txtPages.Text;
            book.CoverType = txtCover.Text;
            book.BookTypeName = txtBookTypeName.Text;
            book.Price = txtPrice.Text;
            book.Count = txtCount.Text;

            var result = Services.DatabaseService.PutBook(token, book.ID, book, filePath);

            if (result == false)
            {
                MessageBox.Show("Sửa thất bại!\nHãy kiểm tra lại thông tin!");
                return;
            }
            else
            {
                MessageBox.Show("Sửa thành công!");
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