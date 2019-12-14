using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Action;
using System.Net;
using System.Net.Mail;
using System.IO;
using WebService.Models;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WebService.Controllers
{
    public class CashController : Controller
    {
        #region CashManagement
        public ActionResult ListNotApply()
        {
            ViewBag.ListNotApply = CashAction.ListNotApply();
            return View();
        }

        public ActionResult Apply(int ID)
        {
            CashAction.Apply(ID);
            return RedirectToAction("ListNotApply");
        }

        public ActionResult RemoveBill(int ID)
        {
            CashAction.RemoveBill(ID);
            return RedirectToAction("ListNotApply");
        }

        public ActionResult ListApply()
        {
            ViewBag.ListApply = CashAction.ListApply();
            return View();
        }

        public ActionResult Paid(int ID)
        {
            CashAction.Paid(ID);
            return RedirectToAction("ListApply");
        }

        public ActionResult ListPaid()
        {
            ViewBag.ListPaid = CashAction.ListPaid();
            return View();
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
            Session["alert"] = "ok";
            content = content.Replace("{{Name}}", "sadsada");
            content = content.Replace("{{product}}", text);
            content = content.Replace("{{Content}}", "allday997@gmail.com");
            content = content.Replace("{{totalcost}}", Convert.ToString(detail.Bill.TotalCost));
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail("allday997@gmail.com", "Đơn Đặt Hàng", content);
            new MailHelper().SendMail(toEmail, "Đơn Đặt Hàng", content);
            return RedirectToAction("ViewCart", "Cart");
        }
        #endregion

        public ActionResult ExportToExcel(int id)
        {
            var products = BillAction.ListBillDetail(id);
            var bill = BillAction.ReBill(id);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:D1"].Value = "Thông Tin Đơn hàng";
            ws.Cells["A1:D1"].Merge = true;
            ws.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //ws.Cells["A1:D1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);





            ws.Cells["A3"].Value = "Người đặt hàng";
            ws.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // ws.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["B3"].Value = bill.Account.UserName;
            ws.Cells["B3:D3"].Merge = true;
            ws.Cells["B3:D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //  ws.Cells["B3:D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["A4"].Value = "Ngày đặt hàng";
            ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // ws.Cells["A4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


            ws.Cells["B4"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", bill.FoundedDate);
            ws.Cells["B4:D4"].Merge = true;
            ws.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //  ws.Cells["B4:D4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["A6"].Value = "STT";
            ws.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["B6"].Value = "Tên sách";
            ws.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["C6"].Value = "Số lượng";
            ws.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["C6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["D6"].Value = "Giá sau giảm";
            ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["D6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            int rowStart = 7;
            int stt = 1;
            foreach (var item in products)
            {

                if (item.Count < 10)
                {
                    //    ws.Cells[string.Format("A{0}:D{1}",rowStart,rowStart)].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //    ws.Cells[string.Format("A{0}:D{1}", rowStart, rowStart)].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = stt.ToString();
                ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("A{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Book.Name;
                ws.Cells[string.Format("B{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("B{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Count;
                ws.Cells[string.Format("C{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("C{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Price;
                ws.Cells[string.Format("D{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("D{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                rowStart++;
                stt++;
            }

            ws.Cells[string.Format("A{0}:C{1}", rowStart, rowStart)].Value = "TỔNG TIỀN THANH TOÁN:";
            ws.Cells[string.Format("A{0}:C{1}", rowStart, rowStart)].Merge = true;
            ws.Cells[string.Format("A{0}:C{1}", rowStart, rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("A{0}:C{1}", rowStart, rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("A{0}:C{1}", rowStart, rowStart)].Style.Font.Bold = true;

            ws.Cells[string.Format("D{0}", rowStart)].Value = bill.TotalCost;
            ws.Cells[string.Format("D{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("D{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("D{0}", rowStart)].Style.Font.Bold = true;
            int rowEnd = rowStart + 2;

            ws.Cells[string.Format("A{0}:D{1}", rowEnd, rowEnd)].Value = "XIN CẢM ƠN QUÝ KHÁCH!";
            ws.Cells[string.Format("A{0}:D{1}", rowEnd, rowEnd)].Merge = true;
            ws.Cells[string.Format("A{0}:D{1}", rowEnd, rowEnd)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("A{0}:D{1}", rowEnd, rowEnd)].Style.Border.Top.Style = ExcelBorderStyle.DashDot;


            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attactment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return View();
        }
    }
}