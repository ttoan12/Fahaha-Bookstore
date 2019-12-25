using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCuaHangSach.Models
{
    public class BookModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string PublishingCompany { get; set; }
        public string PublishingDate { get; set; }
        public string Size { get; set; }
        public string NumberOfPages { get; set; }
        public string CoverType { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Discount { get; set; }
        public string Count { get; set; }
        public string AuthorName { get; set; }
        public string BookTypeName { get; set; }

        public static BookModel Import(Book book)
        {
            var bookModel = new BookModel
            {
                ID = book.ID.ToString(),
                Name = book.Name,
                PublishingCompany = book.PublishingCompany,
                AuthorName = book.Author.Name,
                BookTypeName = book.BookType.Name,
                Count = book.Count.ToString(),
                CoverType = book.CoverType,
                Discount = book.Discount.ToString(),
                Image = book.Image,
                Status = book.IsDeleted ? "Đã xoá" : "",
                NumberOfPages = book.NumberOfPages.ToString(),
                Price = book.Price.ToString(),
                PublishingDate = book.PublishingDate.ToString("dd/MM/yyyy"),
                Size = book.Size
            };

            return bookModel;
        }

        public static List<BookModel> Import(List<Book> books)
        {
            List<BookModel> models = new List<BookModel>();
            books.ForEach(book =>
            {
                var bookModel = new BookModel
                {
                    ID = book.ID.ToString(),
                    Name = book.Name,
                    PublishingCompany = book.PublishingCompany,
                    AuthorName = book.Author.Name,
                    BookTypeName = book.BookType.Name,
                    Count = book.Count.ToString(),
                    CoverType = book.CoverType,
                    Discount = book.Discount.ToString(),
                    Image = book.Image,
                    Status = book.IsDeleted ? "Đã xoá" : "",
                    NumberOfPages = book.NumberOfPages.ToString(),
                    Price = book.Price.ToString(),
                    PublishingDate = book.PublishingDate.ToString("dd/MM/yyyy"),
                    Size = book.Size
                };
                models.Add(bookModel);
            });

            return models;
        }

        public Book ToBook()
        {
            BookContext db = new BookContext();

            int authorID;
            var author = db.Authors.FirstOrDefault(x => x.Name == AuthorName);
            if (author != null) authorID = author.ID;
            else
            {
                var newAuthor = new Author { Name = AuthorName };
                db.Authors.Add(newAuthor);
                db.SaveChanges();
                authorID = newAuthor.ID;
            }

            int bookTypeID;
            var bookType = db.BookTypes.FirstOrDefault(x => x.Name == BookTypeName);
            if (bookType != null) bookTypeID = bookType.ID;
            else
            {
                var newBookType = new BookType { Name = BookTypeName };
                db.BookTypes.Add(newBookType);
                db.SaveChanges();
                bookTypeID = newBookType.ID;
            }

            Book book = new Book
            {
                ID = int.TryParse(ID, out int intID) ? intID : 0,
                Name = Name,
                AuthorID = authorID,
                BookTypeID = bookTypeID,
                Count = int.TryParse(Count, out int intCount) ? intCount : 0,
                CoverType = CoverType,
                Discount = int.TryParse(Discount, out int intDiscount) ? intDiscount : 0,
                Image = Image,
                NumberOfPages = int.TryParse(NumberOfPages, out int intNumberOfPages) ? intNumberOfPages : 0,
                Price = double.TryParse(Price, out double doublePrice) ? doublePrice : 0,
                PublishingCompany = PublishingCompany,
                PublishingDate = DateTime.Parse(PublishingDate),
                Size = Size,
                ReducePrice = doublePrice - intDiscount
            };
            return book;
        }
    }
}