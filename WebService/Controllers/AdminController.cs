using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Action;
using WebService.Models;

namespace WebService.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("ListBook", "Admin");
        }
        [HttpGet]
        public ActionResult ListAccount()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 1)
            {
                ViewBag.ListAccount = AccountAction.ListAccount();
                Account account = new Account();
                using (var db = new BookContext())
                {
                    account.RoleCollection = db.Roles.ToList();
                }
                //AccountAction.
                return View(account);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult ListAccount(string SearchString)
        {

            ViewBag.ListAccount = AccountAction.Search(SearchString);
            return View();
        }

        [HttpGet]
        public ActionResult ListBook()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 1)
            {
                ViewBag.ListBook = BookAction.ListBook();
                //AccountAction.
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult ListBook(string SearchString)
        {
            ViewBag.ListBook = BookAction.SearchBook(SearchString);
            return View();

        }

        [HttpGet]
        public ActionResult ListLog()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 1)
            {
                ViewBag.ListLog = LogAction.ListLog();

                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult ListLog(string SearchString)
        {
            ViewBag.ListLog = LogAction.Search(SearchString);
            //AccountAction.
            return View();
        }

        [HttpGet]
        public ActionResult ListAuthor()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 1)
            {
                ViewBag.ListAuthor = AuthorAction.ListAuthor();

                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult ListBookType()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 1)
            {
                ViewBag.ListBookType = BookTypesAction.ListBookType();

                return View();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}