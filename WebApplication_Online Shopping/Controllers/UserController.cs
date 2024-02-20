using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication_Online_Shopping.Models;

namespace WebApplication_Online_Shopping.Controllers
{
    public class UserController : Controller
    {
        ProjectContext _db;
        public UserController(ProjectContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var rawList = await (from test in _db.Products select test).ToListAsync();  
            var rawList = await (from t1 in _db.Products
                                 join
                                 t2 in _db.Categories
                                 on t1.CategoryId equals t2.CategoryId
                                 select new Product
                                 {
                                     ProductId = t1.ProductId,
                                     ProductName = t1.ProductName,
                                     ProductDescription = t1.ProductDescription,
                                     ProductPrice = t1.ProductPrice,
                                     ProductPic = t1.ProductPic,
                                     CategoryId = t1.CategoryId,
                                     CategoryName = t2.CategoryName
                                 }).ToListAsync();


            return View(rawList);
        }
        [HttpPost]
        //public IActionResult Index(String Search)
        //{

        //    var ProductList = _db.Products.ToList();
        //    if (Search != null)
        //    {
        //        ProductList = _db.Products.Where(x => x.ProductName.Contains(Search)).ToList();
        //        return View(ProductList);
        //    }
        //    else
        //    {
        //        TempData["Message"] = "Sorry! Not Matched Found....";
        //    }
        //    return View(ProductList);

        //}

         public async Task<IActionResult> Index(String Search)
        {

            IEnumerable<Product> ProductList =await _db.Products.ToListAsync();
            if (Search != null)
            {
                ProductList = await (from e1 in _db.Products 
                               where e1.ProductName.Contains(Search)  
                               || e1.ProductDescription.Contains(Search) 
                               select e1).ToListAsync();
                return View(ProductList);
            }
            else
            {
                ViewBag.Message = "Sorry! Not Matched Found....";
            }
            return View(ProductList);

        }
       

        public async Task<IActionResult> Details(int id)
        {
            var row = await _db.Products.Where(a => a.ProductId == id).FirstOrDefaultAsync();
            ViewBag.Image = row.ProductPic;
            return View(row);
        }

        public IActionResult AddCart(int id)
        {
            List<Product> oldcart = null;
            var oldcart_string = HttpContext.Session.GetString("cart");
            if (oldcart_string != null)
            {
                oldcart = JsonConvert.DeserializeObject<List<Product>>(oldcart_string);
            }
            if (oldcart == null) 
            {
                List<Product>productCart = new List<Product>();
                productCart.Add(_db.Products.Where(a => a.ProductId == id).FirstOrDefault()); 
                
               // productCart.Add(_db.Products.Where(a=>a.ProductId == id).FirstOrDefault());
                string string_productCart=JsonConvert.SerializeObject(productCart);
                HttpContext.Session.SetString("cart", string_productCart);
            }
            else
            {
                oldcart.Add(_db.Products.Where(a => a.ProductId == id).FirstOrDefault());
                string string_productCart = JsonConvert.SerializeObject(oldcart);
                HttpContext.Session.SetString("cart", string_productCart);

            }
            return RedirectToAction("Index");   
        }
        public IActionResult Cart()
        {
            List<Product> oldcart = new List<Product>();
            var oldest_string = HttpContext.Session.GetString("cart");
            if(oldest_string != null) 
            {
                oldcart = JsonConvert.DeserializeObject<List<Product>>(oldest_string);
            }
            return View(oldcart);
        }

        public IActionResult Delete_cart(int id)
        {
            List<Product> oldcart = null;
            var oldcart_string = HttpContext.Session.GetString("cart");//fetching the old cart if exist

            if (oldcart_string != null)
            {
                oldcart = JsonConvert.DeserializeObject<List<Product>>(oldcart_string);//populate the oldcart List
            }

            foreach (var item in oldcart)
            {
                if (item.ProductId == id)
                {
                    oldcart.Remove(item);
                    break;
                }
            }
            string string_productCart = JsonConvert.SerializeObject(oldcart);//convert the cart list to string
            HttpContext.Session.SetString("cart", string_productCart);//save in session
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Buy(int id)
        {
            List<Product> productCart = new List<Product>();
            if (id != 0)
            {
                var row = await _db.Products.Where(a => a.ProductId == id).FirstOrDefaultAsync();
                productCart.Add(row);
                string string_productCart = JsonConvert.SerializeObject(productCart);
                HttpContext.Session.SetString("cart", string_productCart);
            }
            
           

            return RedirectToAction("Create", "Order");
        }
    }
}
