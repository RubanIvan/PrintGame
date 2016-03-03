using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PrintGame.Models;

namespace PrintGame.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult GetAllUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            return Json(context.Users.ToList());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult CreateUser(string email, string password)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            

            var user = new ApplicationUser { UserName = email, Email = email,TwoFactorEnabled = false};

            IdentityResult result = userManager.Create(user, password);
            
            return Json(result);
        }


    }
}