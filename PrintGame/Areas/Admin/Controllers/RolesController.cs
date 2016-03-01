using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PrintGame.Controllers;
using PrintGame.Models;

namespace PrintGame.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        //отображаем доступные роли

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }

        #region создать роль
        // GET: /Roles/Create
        // Создать новую роль 
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            return View();
        }


        // POST: /Roles/Create
        // Создать новую роль 
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion


        #region изменить имя роли
        // редактировать 
        // GET: /Roles/Edit/
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        // редактировать 
        // POST: /Roles/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        // удалить роль
        // GET: /Roles/Delete/
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        #region назначение ролей
        //вывести список назначенных ролей
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageUserRoles()
        {
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        // назначить пользователю роль
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            List<string> roles;
            List<string> users;
            using (var context1 = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context1);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var userStore = new UserStore<ApplicationUser>(context1);
                var userManager = new UserManager<ApplicationUser>(userStore);

                users = (from u in userManager.Users select u.UserName).ToList();

                var user = userManager.FindByName(UserName);
                if (user == null)
                    throw new Exception("User not found!");

                var role = roleManager.FindByName(RoleName);
                if (role == null)
                    throw new Exception("Role not found!");

                if (userManager.IsInRole(user.Id, role.Name))
                {
                    ViewBag.ResultMessage = "This user already has the role specified !";
                }
                else
                {
                    userManager.AddToRole(user.Id, role.Name);
                    context.SaveChanges();

                    ViewBag.ResultMessage = "Username added to the role succesfully !";
                }

                roles = (from r in roleManager.Roles select r.Name).ToList();
            }


            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                List<string> userRoles;
                List<string> roles;
                List<string> users;
                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    roles = (from r in roleManager.Roles select r.Name).ToList();

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    users = (from u in userManager.Users select u.UserName).ToList();

                    var user = userManager.FindByName(userName);
                    if (user == null)
                        throw new Exception("User not found!");

                    var userRoleIds = (from r in user.Roles select r.RoleId);
                    userRoles = (from id in userRoleIds
                                 let r = roleManager.FindById(id)
                                 select r.Name).ToList();
                }

                ViewBag.RolesForThisUser = userRoles;
                ViewBag.Roles = new SelectList(roles);
                //ViewBag.Users = new SelectList(users);
                //ViewBag.RolesForThisUser = userRoles;
            }

            return View("ManageUserRoles");
        }

        //удалить роль
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string userName, string roleName)
        {
            List<string> userRoles;
            List<string> roles;
            List<string> users;
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roles = (from r in roleManager.Roles select r.Name).ToList();

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                users = (from u in userManager.Users select u.UserName).ToList();

                var user = userManager.FindByName(userName);
                if (user == null)
                    throw new Exception("User not found!");

                if (userManager.IsInRole(user.Id, roleName))
                {
                    userManager.RemoveFromRole(user.Id, roleName);
                    context.SaveChanges();

                    ViewBag.ResultMessage = "Role removed from this user successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }

                var userRoleIds = (from r in user.Roles select r.RoleId);
                userRoles = (from id in userRoleIds
                             let r = roleManager.FindById(id)
                             select r.Name).ToList();
            }
            ViewBag.RolesForThisUser = userRoles;
            ViewBag.Roles = new SelectList(roles);

            ViewBag.Users = new SelectList(users);
            return View("ManageUserRoles");
        }

        #endregion
    }
}