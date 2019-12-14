
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Action;
using WebService.Models;

namespace WebService.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult ViewCart()
        {
            ViewBag.BookTop = BookAction.AddSachTop5ToDb();
            if (TempData["Alert"] != null)
            {
                ViewBag.Alert = TempData["Alert"].ToString();
            }
            if(Session["UserName"] != null && (int)Session["Role"] == 2)
            {
                int AccountId = (int)Session["UserID"];
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var listCart = BillAction.ListCartDetail(AccountId);

                if (listCart != null)
                {
                    ViewBag.ListCart = listCart;
                    ViewBag.Detail = listCart.FirstOrDefault();

                }
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AddSingle(int ID)
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 2)
            {
                int AccountId = (int)Session["UserID"];
                BillAction.AddSingle(AccountId,ID);
                return RedirectToAction("ShopGrid", "Book");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult AddCart(int BookId,int Count)
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 2)
            {
                int AccountId = (int)Session["UserID"];
                ViewBag.Message = BillAction.AddCart(AccountId, BookId, Count);
                return RedirectToAction("Detail", "Book", new { ID = BookId });
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Order(int BillId)
        {

            BillAction.AddCartToBill(BillId);
            return RedirectToAction("ViewCart");
        }
        public ActionResult DeleteCart(int Id)
        {

            BillAction.DeleteCartDetail(Id);
            return RedirectToAction("ViewCart");
        }
        [HttpPost]
        public ActionResult UpdateCart(int ID, int Count)
        {
            BillAction.UpdateCart(ID, Count);
            return RedirectToAction("ViewCart");
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 2)
            {
                int AccountId = (int)Session["UserID"];
                ViewBag.Bill = BillAction.FindBill(AccountId);
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        public ActionResult ManageInfo()
        {
            if (Session["UserID"] == null && (int)Session["Role"] == 2)
            {
                //ViewBag.ListInfo = AccountAction.ListInfo();
                return View();
            }
            return RedirectToAction("Index", "Book");

        }

        [HttpPost]
        public ActionResult OrderBill()
        {
            int a = (int)Session["UserID"];
            var listcash = BillAction.ListCartDetail(a);
            var detail = listcash.FirstOrDefault();
            string text = "";
            foreach (var item in listcash)
            {
                text += item.Book.Name + "<br/>" + item.Price + "\n" + item.Count;
            }
            CashAction.Order(a);
            string content = System.IO.File.ReadAllText(Server.MapPath("~/client/template/neworder.html"));
            TempData["Alert"] = "ok";
            content = content.Replace("{{Name}}", "sadsada");
            content = content.Replace("{{product}}", text);
            content = content.Replace("{{Content}}", "allday997@gmail.com");
            content = content.Replace("{{totalcost}}", Convert.ToString(detail.Bill.TotalCost));
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail("allday997@gmail.com", "Đơn Đặt Hàng", content);
            new MailHelper().SendMail(toEmail, "Đơn Đặt Hàng", content);
            return RedirectToAction("ViewCart");
        }
    }
}