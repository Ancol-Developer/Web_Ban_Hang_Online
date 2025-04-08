using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class StatisticalController : Controller
    {
        private readonly WebStoreDbContext db;

        public StatisticalController(WebStoreDbContext dbContext)
        {
            this.db = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStatistical(string? fromDate, string? toDate)
        {
            
            var query = from o in db.Order
                        join od in db.OrderDetail
                        on o.Id equals od.OrderId
                        join p in db.Product
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreateDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => x.CreatedDate.Date).Select(x => new
            {
                Date = x.Key,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            
            return Json(new { Data = result });
        }

    }
}
