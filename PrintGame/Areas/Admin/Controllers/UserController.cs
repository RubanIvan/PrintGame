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

        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult CreateUser(string email, string password)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = new ApplicationUser { UserName = email, Email = email, TwoFactorEnabled = false,EmailConfirmed = true};

            IdentityResult result = userManager.Create(user, password);

            return Json(result);
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult DleteUser(string email)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            ApplicationUser user = userManager.FindByName(email);

            IdentityResult result;
            try
            {
                result = userManager.Delete(user);
            }
            catch (Exception e)
            {
                result = new IdentityResult(e.Message);
            }

            return Json(result);
        }


    }
}