using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PrintGame.Models;

namespace PrintGame.Controllers
{
    public class SubscribeController : Controller
    {
        // GET: Subscribe
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {

            PrintGameDataEntities enties = new PrintGameDataEntities();
            var FullTag = from t in enties.Tag
                          orderby t.TagName
                          select t;

            SubscribeModel model = new SubscribeModel();
            model.FullTags = FullTag.ToList();

            var SubscribeTag = from s in enties.Subscribe
                               where s.Email == User.Identity.Name
                               select s;

            if (SubscribeTag.Any())
            {
                model.SubscribeTag = JsonConvert.DeserializeObject<List<Tag>>(SubscribeTag.First().TagList);
            }


            return View(model);
        }
    }
}