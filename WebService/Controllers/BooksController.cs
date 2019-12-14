using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebService.Models;

namespace WebService.Controllers
{
    public class BooksController : ApiController
    {
        private BookContext db = new BookContext();

        // GET: api/Book
        public IHttpActionResult GetBooks()
        {
            List<Book> books;
            List<BookModel> models;
            var hostAddr = string.Format("{0}://{1}:{2}", Request.RequestUri.Scheme, Request.RequestUri.Host, Request.RequestUri.Port);
            string imgAddr = hostAddr + "/Content/UploadedFiles/";

            if (Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens))
            {
                string token = tokens.FirstOrDefault();
                string username = TokenManager.ValidateToken(token);
                if (username != null)
                {
                    var acc = db.Accounts.Include(x => x.Role).FirstOrDefault(x => x.UserName == username);
                    if (acc != null && acc.Role.Name == "Admin")
                    {
                        books = db.Books.Include(x => x.Author).Include(x => x.BookType).ToList();
                        books.ForEach(x => x.Image = x.Image != null ? imgAddr + x.Image : x.Image);
                        models = BookModel.Import(books);
                        return Ok(models);
                    }
                }
            }

            books = db.Books.Where(x => x.IsDeleted == false).Include(x => x.Author).Include(x => x.BookType).ToList();
            books.ForEach(x => x.Image = x.Image != null ? imgAddr + x.Image : x.Image);

            // Convert to model
            models = BookModel.Import(books);
            return Ok(models);
        }

        [HttpPost]
        [Route("api/Book/Search")]
        public IHttpActionResult Search(SearchModel model)
        {
            List<Book> books;
            List<BookModel> models;
            var hostAddr = string.Format("{0}://{1}:{2}", Request.RequestUri.Scheme, Request.RequestUri.Host, Request.RequestUri.Port);
            string imgAddr = hostAddr + "/Content/UploadedFiles/";

            if (Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens))
            {
                string token = tokens.FirstOrDefault();
                string username = TokenManager.ValidateToken(token);
                if (username != null)
                {
                    var acc = db.Accounts.Include(x => x.Role).FirstOrDefault(x => x.UserName == username);
                    if (acc != null && acc.Role.Name == "Admin")
                    {
                        books = db.Books.Include(x => x.Author).Include(x => x.BookType).ToList();
                        books.ForEach(x => x.Image = x.Image != null ? imgAddr + x.Image : x.Image);
                        books = BookHelper.Search(books, model);
                        models = BookModel.Import(books);
                        return Ok(models);
                    }
                }
            }

            books = db.Books.Where(x => x.IsDeleted == false).Include(x => x.Author).Include(x => x.BookType).ToList();
            books.ForEach(x => x.Image = x.Image != null ? imgAddr + x.Image : x.Image);
            books = BookHelper.Search(books, model);
            models = BookModel.Import(books);
            return Ok(models);
        }

        // GET: api/Book/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult GetBook(int id)
        {
            Book book = db.Books.Include(x => x.Author).Include(x => x.BookType).FirstOrDefault(x => x.ID == id);
            BookModel model;
            var hostAddr = string.Format("{0}://{1}:{2}", Request.RequestUri.Scheme, Request.RequestUri.Host, Request.RequestUri.Port);
            string imgAddr = hostAddr + "/Content/UploadedFiles/";

            if (book == null)
            {
                return NotFound();
            }
            else if (!book.IsDeleted)
            {
                book.Image = book.Image != null ? imgAddr + book.Image : book.Image;
                model = BookModel.Import(book);
                return Ok(model);
            }
            else
            {
                if (Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens))
                {
                    string token = tokens.FirstOrDefault();
                    string username = TokenManager.ValidateToken(token);
                    if (username != null)
                    {
                        var acc = db.Accounts.Include(x => x.Role).FirstOrDefault(x => x.UserName == username);
                        if (acc != null && acc.Role.Name == "Admin")
                        {
                            book.Image = book.Image != null ? imgAddr + book.Image : book.Image;
                            model = BookModel.Import(book);
                            return Ok(model);
                        }
                    }
                }
            }

            return NotFound();
        }

        // PUT: api/Book/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, BookModel bookModel)
        {
            bool authorized = false;
            if (Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens))
            {
                string token = tokens.FirstOrDefault();
                string username = TokenManager.ValidateToken(token);
                if (username != null)
                {
                    var acc = db.Accounts.Include(x => x.Role).FirstOrDefault(x => x.UserName == username);
                    if (acc != null && (acc.Role.Name == "Admin" || acc.Role.Name == "Manager"))
                    {
                        authorized = true;
                    }
                }
            }
            if (!authorized)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var form = HttpContext.Current.Request.Form;
            if (!ModelState.IsValid && form.Count <= 0)
            {
                return BadRequest(ModelState);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return BadRequest();
            }

            bookModel = new BookModel()
            {
                ID = id.ToString(),
                Name = form.Get("Name"),
                AuthorName = form.Get("AuthorName"),
                PublishingCompany = form.Get("PublishingCompany"),
                PublishingDate = form.Get("PublishingDate"),
                Size = form.Get("Size"),
                NumberOfPages = form.Get("NumberOfPages"),
                CoverType = form.Get("CoverType"),
                Price = form.Get("Price"),
                Discount = form.Get("Discount"),
                Count = form.Get("Count"),
                BookTypeName = form.Get("BookTypeName")
            };

            book.Import(bookModel.ToBook());

            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                var fileName = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/Content/UploadedFiles"),
                    fileName
                );

                file.SaveAs(path);
                book.Image = fileName;
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Book
        [ResponseType(typeof(void))]
        public IHttpActionResult PostBook(BookModel bookModel)
        {
            bool authorized = false;
            if (Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens))
            {
                string token = tokens.FirstOrDefault();
                string username = TokenManager.ValidateToken(token);
                if (username != null)
                {
                    var acc = db.Accounts.Include(x => x.Role).FirstOrDefault(x => x.UserName == username);
                    if (acc != null && (acc.Role.Name == "Admin" || acc.Role.Name == "Manager"))
                    {
                        authorized = true;
                    }
                }
            }
            if (!authorized)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var form = HttpContext.Current.Request.Form;
            if (!ModelState.IsValid && form.Count <= 0)
            {
                return BadRequest(ModelState);
            }

            bookModel = new BookModel()
            {
                Name = form.Get("Name"),
                AuthorName = form.Get("AuthorName"),
                PublishingCompany = form.Get("PublishingCompany"),
                PublishingDate = form.Get("PublishingDate"),
                Size = form.Get("Size"),
                NumberOfPages = form.Get("NumberOfPages"),
                CoverType = form.Get("CoverType"),
                Price = form.Get("Price"),
                Discount = form.Get("Discount"),
                Count = form.Get("Count"),
                BookTypeName = form.Get("BookTypeName")
            };

            Book book = bookModel.ToBook();

            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                var fileName = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/Content/UploadedFiles"),
                    fileName
                );

                file.SaveAs(path);
                book.Image = fileName;
            }

            db.Books.Add(book);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Book/5
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteBook(int id)
        {
            bool authorized = false;
            if (Request.Headers.TryGetValues("Token", out IEnumerable<string> tokens))
            {
                string token = tokens.FirstOrDefault();
                string username = TokenManager.ValidateToken(token);
                if (username != null)
                {
                    var acc = db.Accounts.Include(x => x.Role).FirstOrDefault(x => x.UserName == username);
                    if (acc != null && (acc.Role.Name == "Admin" || acc.Role.Name == "Manager"))
                    {
                        authorized = true;
                    }
                }
            }
            if (!authorized)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            book.IsDeleted = true;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.ID == id) > 0;
        }
    }
}