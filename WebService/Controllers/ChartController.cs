using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Models;

namespace WebService.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult ColumnChart()
        {
            if (Session["UserName"] != null && (int)Session["Role"] == 1)
            {
                using (var db = new BookContext())
                {
                    //Pie Chart
                    var temp = db.Bills.ToList();
                    ArrayList header = new ArrayList { "Task Name", "Hours" };

                    ArrayList data = new ArrayList { header };
                    for (int i = 0; i < temp.Count; i++)
                    {
                        data.Add(new ArrayList { temp[i].ID.ToString(), temp[i].TotalCost });
                    }


                    string datastr = JsonConvert.SerializeObject(data, Formatting.None);
                    ViewBag.Data = new HtmlString(datastr);

                    //Area Chart
                    var query = db.Bills.Where(m => m.IsPaid == true && m.IsDeleted == false)
                        .GroupBy(m => m.FoundedDate.Year)
                        .Select(m => new { tong = m.Sum(a => a.TotalCost), Year = m.Key.ToString() })
                        .ToList();
                    ArrayList headerAreaChart = new ArrayList { "Year", "Tổng Tiền" };
                    ArrayList dataAreaChart = new ArrayList { headerAreaChart };

                    for (int i = 0; i < query.Count; i++)
                    {
                        dataAreaChart.Add(new ArrayList { query[i].Year, query[i].tong });
                    }
                    string datastrArea = JsonConvert.SerializeObject(dataAreaChart, Formatting.None);
                    ViewBag.DataArea = new HtmlString(datastrArea);

                    //Column Chart

                    var query1 = db.BillDetails.Where(m => m.IsDeleted == false)
                        .GroupBy(m => m.Book.Name)
                        .Select(m => new
                        {
                            tong1 = m.Sum(a => a.Count).ToString(),
                            Name = m.Key.ToString(),
                            tong2 =
                        m.Sum(a => a.Price)
                        })
                        .ToList();
                    ArrayList headerColumnChart = new ArrayList { "Tên Sách", "Số Lượng", "Tổng Tiền Bán" };
                    ArrayList dataColumnChart = new ArrayList { headerColumnChart };
                    for (int i = 0; i < query1.Count; i++)
                    {
                        dataColumnChart.Add(new ArrayList { query1[i].Name, float.Parse(query1[i].tong1), query1[i].tong2 });
                    }
                    string datastrColumn = JsonConvert.SerializeObject(dataColumnChart, Formatting.None);
                    ViewBag.DataColumn = new HtmlString(datastrColumn);

                    //Donut Chart
                    var query2 = db.Watcheds.GroupBy(m => m.Book.Name)
                        .Select(m => new { solan = m.Count(a => a.id_book == a.id_book).ToString(), name1 = m.Key.ToString() })
                        .ToList();
                    ArrayList headerDonutChart = new ArrayList { "Tên Sách", "Số lượt xem" };
                    ArrayList dataDonutChart = new ArrayList { headerDonutChart };
                    for (int i = 0; i < query2.Count; i++)
                    {
                        dataDonutChart.Add(new ArrayList { query2[i].name1, int.Parse(query2[i].solan) });
                    }
                    string datastrDonut = JsonConvert.SerializeObject(dataDonutChart, Formatting.None);
                    ViewBag.DataDonut = new HtmlString(datastrDonut);
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult VisualizeBillsResults()
        {
            return Json(Result(),JsonRequestBehavior.AllowGet);
        }

        public List<Bill> Result()
        {
            List<Bill> list = new List<Bill>();
            using (var db = new BookContext())
            {
                list = db.Bills.ToList();
            }
            return list;
        }



    }
}