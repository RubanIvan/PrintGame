using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintGame.Models;
using PrintGame.Proc;

namespace PrintGame.Controllers
{
    public class StrategicController : Controller
    {
        public ActionResult Index(int? PageID)
        {
            ViewBag.Title = "Распечатай и играй. Сканы настольных игр";

            //если id ошибочный возвращаем 404 
            if (PageID < 1) return HttpNotFound();

            //если на естановлен устанавливаем
            if (PageID == null) PageID = 1;


            List<PageModel> Page = new List<PageModel>();

            PrintGameDataEntities entities = new PrintGameDataEntities();

            var query = (from g in entities.Game
                         join gt in entities.GameTag on g.GameID equals gt.GameID
                         join t in entities.Tag on gt.TagID equals t.TagID
                         //where t.TagName == "стратегия"
                         where t.TagID == 2
                         select g).Distinct();


            foreach (Game pageResult in query.OrderByDescending(g => g.GameID).Skip((int)(PageID - 1) * Constants.GamePerPage).Take(Constants.GamePerPage))
            {
                PageModel p = new PageModel();
                p.GameID = pageResult.GameID;
                p.TitleRu = pageResult.TitleRu;
                p.TitleEn = pageResult.TitleEn;
                p.Rating = pageResult.Rating * 10;
                p.YearPublishing = pageResult.YearPublishing;
                //сокращаем до 1 абзаца описание
                int s_begin = pageResult.Descript.IndexOf("<p>") + 3;
                if (s_begin != 0) pageResult.Descript = pageResult.Descript.Remove(0, s_begin);
                int s_end = pageResult.Descript.IndexOf("</p>");
                p.SmallDescript = pageResult.Descript.Remove(s_end);

                //если обзац слишком большой сокращаем еще
                if (p.SmallDescript.Length > 500) p.SmallDescript = p.SmallDescript.Remove(500) + "...</p>";

                //получаем ссылку на титульную картинку
                PrintGameDataEntities entities1 = new PrintGameDataEntities();
                var img = entities1.GetGameBoxImage(p.GameID).First();
                p.BoxImage = img.SmallImagePath;

                p.SEOGameUrl = $"/game/{p.GameID}-{Slug.Create(p.TitleEn)}";

                Page.Add(p);
            }

            int MaxPage = (int)Math.Ceiling((query.Count() / ((float)Constants.GamePerPage)));

            ViewBag.Pangination = PagePangination.GetPangination((int)PageID, MaxPage, @"/strategic/");
            //return  View("Index",Page);
            return View("~/Views/Page/Index.cshtml", Page);
        }
    }
}