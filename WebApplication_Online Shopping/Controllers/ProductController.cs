using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication_Online_Shopping.Models;

namespace WebApplication_Online_Shopping.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        ProjectContext _db;
        IWebHostEnvironment _environment;
        public ProductController(ProjectContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;

        }

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
       
        

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var rawList = await (from test in _db.Categories select test).ToListAsync();
            ViewBag.ProductList = rawList;
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult> Create (Product product)
        {
            try
            {
                product.ProductPic = UploadFile(product);
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var rawList = await (from test in _db.Categories select test).ToListAsync();
                ViewBag.ProductList = rawList;
                return View();
            }
            
        }

        public string UploadFile(Product product)
        {
            string fileName = null;
            if (product.Picture != null)
            {
                string uploadDir = Path.Combine(_environment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "_" + product.Picture.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.Picture.CopyTo(fileStream);
                }
            }
            return fileName;

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var row = await (from test in _db.Categories
                             select test).ToListAsync();
            ViewBag.ProductList = row;

            var raw = await _db.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();

            if (raw != null)
            {
                ViewBag.Image = raw.ProductPic;
                return View(raw);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            try
            {
                if (product.Picture != null)
                {
                    string imagePath = Path.Combine(_environment.WebRootPath, "Images", product.ProductPic);
                    System.IO.File.Delete(imagePath);
                    product.ProductPic = UploadFile(product);

                }

                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();

            }
            catch (Exception ex) { }
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var row = await _db.Products.Where(a => a.ProductId == id).FirstOrDefaultAsync();
            ViewBag.Image = row.ProductPic;
            return View(row);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var row = await _db.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
                if (row != null)
                {
                    string existFile = Path.Combine(_environment.WebRootPath, "Images", row.ProductPic);
                    System.IO.File.Delete(existFile);
                    _db.Products.Remove(row);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index");


           

        }

       

    }
}
