using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using WebApplication_Online_Shopping.Models;
using WebApplication_Online_Shopping.Migrations;

namespace WebApplication_Online_Shopping.Controllers
{
    public class AdminController : Controller
    {
        ProjectContext _db;
        public AdminController(ProjectContext db)
        {
            _db = db;
        }


        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public async Task<IActionResult> Login(Admin admin)
        //{
        //    var validUser = await _db.Admins.Where(a => a.UserName == admin.UserName && a.Password == admin.Password).FirstOrDefaultAsync();    
        //    if (validUser == null)
        //    {
        //        ViewBag.Message = "Invalid User";
        //        return View();
        //    }
        //    else
        //    {
        //        HttpContext.Session.SetString("UserName", admin.UserName);
        //        return RedirectToAction("Index", "Product", "Category");
        //    }
        //}

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            var validUser = await _db.Admins.Where(a => a.UserName == admin.UserName && a.Password == admin.Password).FirstOrDefaultAsync();
            if (validUser != null)
            {

                //bool isValid = (validUser.Email == user.Email && validUser.Password == user.Password);
                //if (isValid)

                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, admin.UserName) },
                   CookieAuthenticationDefaults.AuthenticationScheme);
                var Principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
                HttpContext.Session.SetString("User", "admin.UserName");
                return RedirectToAction("Index", "Product");
            }

            else
            {

                TempData["errorMessage"] = "Invalid Password";
                return View(admin);

            }
            //ViewBag.Massege = "Invalid User";
            //return View();


            //    //assain the Email to a Session variable and redirect to Welcome page
            //    HttpContext.Session.SetString("UserEmail", user.Email);
            //    return RedirectToAction("Index", "Admin");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Index", "Product");
        }
    }






   


}

