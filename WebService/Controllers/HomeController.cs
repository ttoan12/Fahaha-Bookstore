using System.Collections.Generic;
using System.Web.Mvc;
using WebCuaHangSach.Action;
using WebCuaHangSach.Models;

namespace WebCuaHangSach.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult ListViewCart()
        {
            if ((int?)Session["UserID"] != null)
            {
                List<BillDetail> a = new List<BillDetail>();
                a = BillAction.ListCartDetail((int)Session["UserID"]);
                ViewBag.CountItem = BillAction.CountBill((int)Session["UserID"]);
                ViewBag.Total = BillAction.ReBill((int)Session["UserID"]);
                if (a != null)
                {
                    return PartialView(a);
                }
                else { return PartialView(); }
            }
            else if (BillAction.CountBill((int)Session["UserID"]) == 0)
            {
                return RedirectToAction("HomePage", "Home");
            }
            else
            { return RedirectToAction("Login", "Account"); }
        }

        public ActionResult CountListCart()
        {
            if ((int?)Session["UserID"] != null)
            {
                ViewBag.CountItem = BillAction.CountBill((int)Session["UserID"]);
                return PartialView();
            }
            else
            { return RedirectToAction("Login", "Account"); }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Hompage()
        {
            return View();
        }
    }
}