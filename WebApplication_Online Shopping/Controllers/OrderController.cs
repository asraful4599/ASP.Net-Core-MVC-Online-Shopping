using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication_Online_Shopping.Models;

namespace WebApplication_Online_Shopping.Controllers
{
    public class OrderController : Controller
    {
        ProjectContext _db;
        //ctor press tab
        public OrderController(ProjectContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var OrderList = await (from test in _db.Orders select test).ToListAsync();
            return View(OrderList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]//when submit button is click
        public async Task<ActionResult> Create(Order order)
        {
            try
            {
                List<Product> oldcart = null;
                order.OrderDate = DateTime.Now;
                var oldcart_string = HttpContext.Session.GetString("cart");
                if (oldcart_string != null)
                {
                    oldcart = JsonConvert.DeserializeObject<List<Product>>(oldcart_string);
                }
                //order.ProductLIST = oldcart;
                foreach(var item in oldcart)
                {
                    if (order.ProductNames != null)
                    {
                        order.ProductNames = order.ProductNames + "," + item.ProductName;
                        order.TotalAmount += item.ProductPrice;
                    }
                    else
                    {
                        order.ProductNames = item.ProductName;
                        order.TotalAmount += item.ProductPrice;
                    }
                }
                await _db.Orders.AddAsync(order);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index","User");

            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
