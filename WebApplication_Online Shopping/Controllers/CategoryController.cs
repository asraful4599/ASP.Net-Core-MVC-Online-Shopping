using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication_Online_Shopping.Models;

namespace WebApplication_Online_Shopping.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        ProjectContext _db;
        public CategoryController(ProjectContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var rawList = await _db.Categories.ToListAsync();   
            return View(rawList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult>Create(Category cat)
        {
            try
            {
                await _db.Categories.AddAsync(cat);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View();
            }
           

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var row = await(from test in _db.Categories where test.CategoryId == id select test).FirstOrDefaultAsync();
            return View(row);   
        }
        [HttpPost]  
        public async Task<IActionResult>Edit(int id, Category cat)
        {
            try
            {
                _db.Entry(cat).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult>Details(int id)
        {
            var row = await(from test in _db.Categories where test.CategoryId==id select test).FirstOrDefaultAsync();
            return View(row);   
        }

        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                var row_del = await (from test in _db.Categories where test.CategoryId == id select test).FirstOrDefaultAsync();
                if (row_del != null)
                {
                    _db.Categories.Remove(row_del);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex) 
            {
                return View();  
            }
        }

      

    }
}
