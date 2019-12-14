using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Models;
using WebService.Action;
using System.IO;
using PagedList;

namespace WebService.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            List<BookType> a = new List<BookType>();
            a = BookAction.ListBookType();
            return PartialView(a);
        }

        public ActionResult ListTrinhTham(int ID)
        {
            ViewBag.ListTT = BookAction.ListTrinhTham(ID);
            return View();
        }


        public ActionResult Edit(int ID)
        {
            
            ViewBag.book = BookAction.FindBook(ID);
            Book book = new Book();
            using (var db = new BookContext())
            {
                book.AuthorCollection = db.Authors.ToList();
                book.BookTypeCollection = db.BookTypes.ToList();
            }
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(int ID, string Name, string PublishingCompany,
            DateTime PublishingDate, string Size, int NumberOfPages, string CoverType,
            int BookTypeID, int AuthorID, double Price, HttpPostedFileBase file, int Discount)
        {
            Book book = new Book
            {
                AuthorCollection = AuthorAction.ListAuthor(),
                BookTypeCollection = BookAction.ListBookType()
            };
            ViewBag.book = BookAction.FindBook(ID);
            try
            {
               
                string _path = "";
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    BookAction.EditFullBook(ID, Name, PublishingCompany, PublishingDate, Size,
                    NumberOfPages, CoverType, BookTypeID, AuthorID, _FileName, Price, Discount);
                }
                ViewBag.Message = "Updated Successfully";
                return View(book);
            }
            catch
            { 
                ViewBag.Message = "Updated Fail!";
                return View(book);
            }
             
        }
        
        public ActionResult ShopGrid()
        {
            ViewBag.ListBook = BookAction.ListBook();
            return View();
        }
        public ActionResult HomePage()
        {
            ViewBag.ListBook = BookAction.ListBook();
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            Book book = new Book
            {
                AuthorCollection = AuthorAction.ListAuthor(),
                BookTypeCollection = BookAction.ListBookType()
            };
            ViewBag.ListBook = BookAction.ListBook();
            return View(book);
        }
        [HttpPost]
        public ActionResult Add(string Name, string PublishingCompany,
            DateTime PublishingDate, string Size, int NumberOfPages, string CoverType,
            int BookTypeID, int AuthorID, double Price, HttpPostedFileBase file, int Discount)
        {
            
            try
            {
                string _path = "";
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    BookAction.AddBook(Name, PublishingCompany, PublishingDate, Size,
                    NumberOfPages, CoverType, BookTypeID, AuthorID, _FileName, Price, Discount);
                }
                return RedirectToAction("Add");
            }
            catch
            {
                return RedirectToAction("Add");
            }
            
        }

        [HttpGet]
        public ActionResult AddCount(int ID)
        {
            ViewBag.Book = BookAction.FindBook(ID);
            return View();
        }
        [HttpPost]
        public ActionResult AddCount(int ID, int Count)
        {
            ViewBag.Book = BookAction.FindBook(ID);
            if (BookAction.AddCount(ID,Count))
            {
                ViewBag.Book = BookAction.FindBook(ID);
                return RedirectToAction("AddCount", new { ID });
            }
            return RedirectToAction("AddCount", new { ID });
        }
        public ActionResult Detail (int ID)
        {
                Book book = BookAction.FindBook(ID);

            if (Session["UserName"] != null)
            {
                int id_account = (int)Session["UserID"];
                BookAction.Watched(id_account, ID);
            }
            else
            {
                BookAction.Watched(null, ID);
            }
            ViewBag.Book = book;
            ViewBag.ListBook = BookAction.ListBook();
            return View();
        }

        public ActionResult Delete(int ID)
        {
            BookAction.DeleteBook(ID);
            return RedirectToAction("ListBook","Admin");
        }

        
        #region Search
        public ActionResult Search(int? page, string Name)
        {
            
            List<Book> a = BookAction.SearchBook(Name);
            ViewBag.l = a;
            return View();
        }
        #endregion
    }
    
}