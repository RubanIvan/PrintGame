using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintGame.Controllers
{
    public class LicenceController : Controller
    {
        // GET: Licence
        public ActionResult Index()
        {
            ViewBag.Title = "Пользовательское соглашение";
            return View();
        }
    }
}