using System.Linq;
using System.Web.Mvc;
using WebCuaHangSach.Action;
using WebCuaHangSach.Models;

namespace WebCuaHangSach.Controllers
{
    public class AdminMaxController : Controller
    {
        // GET: AdminMax
        public ActionResult Add()
        {
            ViewBag.ListAccount = AccountAction.ListAccount();
            //AccountAction.
            return View();
        }

        [HttpPost]
        public ActionResult Add(string UserName, string Password, string FirstName, string LastName,
            string Email, string PhoneNumber, int RoleID)
        {
            try
            {
                ViewBag.ListAccount = AccountAction.ListAccount();
                AccountAction.AdminAddAccount(UserName, Password, FirstName, LastName, PhoneNumber, Email, RoleID);
                return RedirectToAction("Add");
            }
            catch
            {
                ViewBag.Message = "Thêm thất bại";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ListAccount()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 3)
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
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 3)
            {
                ViewBag.Account = AccountAction.FindAccount(ID);
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(int ID, string Email, string PhoneNumber, string FirstName, string LastName)
        {
            Account acc = AccountAction.FindAccount(ID);
            ViewBag.Message = "Nhập thông tin cần cập nhật";
            if (acc.Email != Email || acc.PhoneNumber != PhoneNumber || acc.FirstName != FirstName || acc.LastName != LastName)
            {
                if (AccountAction.Edit(ID, FirstName, LastName, Email, PhoneNumber))
                {
                    ViewBag.Message = "Cập nhật thành công";
                }
            }
            ViewBag.Account = AccountAction.FindAccount(ID);
            return View();
        }

        [HttpGet]
        public ActionResult Lock(int ID)
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 3)
            {
                AccountAction.Lock(ID);
                return RedirectToAction("ListAccount", "AdminMax");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult UnLock(int ID)
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 3)
            {
                AccountAction.UnLock(ID);
                return RedirectToAction("ListAccount", "AdminMax");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult ChangeRole()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 3)
            {
                ViewBag.ListAccount = AccountAction.ListAccount();
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult AdminEditRole(int ID)
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 3)
            {
                ViewBag.Account = AccountAction.FindAccount(ID);
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Change(int ID, int RoleID)
        {
            ViewBag.Change = AccountAction.EditRole(ID, RoleID);
            return RedirectToAction("ChangeRole", "AdminMax");
        }
    }
}