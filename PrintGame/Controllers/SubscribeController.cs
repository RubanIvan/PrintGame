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
                               orderby s.TagList
                               select s;

            if (SubscribeTag.Any())
            {
                model.SubscribeTag = JsonConvert.DeserializeObject<string[]>(SubscribeTag.First().TagList);
            }


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveTag(string[] Tags)
        {
            List<string> NewTag = new List<string>();

            //если есть тэги делаем список
            if (Tags!=null)
            {
                NewTag = Tags.ToList();
            }


            PrintGameDataEntities enties = new PrintGameDataEntities();
            var SubscribeTag = from s in enties.Subscribe
                               where s.Email == User.Identity.Name
                               orderby s.TagList
                               select s;



            #region если пришел пустой список удаляем все теги подписки
            if (Tags == null)
            {
                //находим строку в таблице
                var del = SubscribeTag.FirstOrDefault();
                //если она есть удаляем ее
                if (del != null)
                {
                    enties.Subscribe.Remove(del);
                    enties.SaveChanges();
                }

            }

            #endregion

            #region если список не пустой обновляем его
            else
            {
                Subscribe sub;
                List<string> t = new List<string>();
                

                //если строки нет создаем ее
                if (!SubscribeTag.Any())
                {
                    sub = new Subscribe();
                    sub.Email = User.Identity.Name;
                }
                //строка есть получаем список тегов
                else
                {
                    sub = SubscribeTag.First();
                    
                }

                foreach (string newTag in NewTag)
                {
                    t.Add(enties.Tag.First(et => et.TagName == newTag).TagName);
                }

                sub.TagList = JsonConvert.SerializeObject(t.ToArray());

                if (!SubscribeTag.Any())
                {
                    enties.Subscribe.Add(sub);
                    enties.SaveChanges();
                }
                else
                {
                    enties.SaveChanges();
                }

            }

            #endregion

            //получаем результат
            enties = new PrintGameDataEntities();
            var ResTag = from s in enties.Subscribe
                               where s.Email == User.Identity.Name
                               orderby s.TagList
                               select s;

            if (ResTag.Any())
            {
                return Json(JsonConvert.DeserializeObject<string[]>(SubscribeTag.First().TagList));
            }

            return null;
        }

    }
}