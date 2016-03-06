using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintGame.Models;
using PrintGame.Proc;

namespace PrintGame.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string text,string tag, int? page)
        {
            ViewBag.Title = "Распечатай и играй. Сканы настольных игр";

            int PageID = 1;

            //если на естановлен устанавливаем
            if (page == null) PageID = 1;
            else PageID = (int)page;

            //если id ошибочный возвращаем 404 
            if (PageID < 1) return HttpNotFound();

            #region разбиваем на теги
            List<string> tagList=new List<string>();
            #endregion


            List<PageModel> Page = new List<PageModel>();

            PrintGameDataEntities entities = new PrintGameDataEntities();

            var query = (from g in entities.Game
                         join gt in entities.GameTag on g.GameID equals gt.GameID
                         join t in entities.Tag on gt.TagID equals t.TagID
                         where g.TitleRu.Contains(text)
                         select  g).Distinct();

            //if (tagList.Count ==1)
            //{
            //    query = (from g in entities.Game
            //             join gt in entities.GameTag on g.GameID equals gt.GameID
            //             join t in entities.Tag on gt.TagID equals t.TagID
            //             where g.TitleRu.Contains(text) && t.TagName == tagList[0]
            //             select g).Distinct();
            //}


            foreach (Game pageResult in query.OrderByDescending(g => g.GameID).Skip((int)(PageID - 1) * Constants.GamePerPageFind).Take(Constants.GamePerPageFind))
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
                p.BoxImageFull = img.FulllImagePath;

                p.SEOGameUrl = $"/game/{p.GameID}-{Slug.Create(p.TitleEn)}";

                Page.Add(p);
            }

            int MaxPage = (int)Math.Ceiling(query.Count() / ((float)Constants.GamePerPageFind));

            ViewBag.Pangination = PagePangination.GetPangination((int)PageID, MaxPage, $@"/Search/?text={text}&page=","1");
            
            //return View("~/Views/Page/Index.cshtml", Page);
            return View(Page);

        }
    }
}