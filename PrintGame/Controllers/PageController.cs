using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PrintGame.Models;
using PrintGame.Proc;

// ReSharper disable All

namespace PrintGame.Controllers
{
    public class PageController : Controller
    {
        ///<summary>ко-во игр на одной странице</summary>
        //private const int NewsPerPage=3;


        // GET: Page
        public ActionResult Index(int? PageID)
        {
            ViewBag.Title = "Распечатай и играй. Сканы настольных игр";

            //если id ошибочный возвращаем 404 
            if (PageID<1) return HttpNotFound();

            //если на естановлен устанавливаем
            if (PageID == null) PageID = 1;

            
            List<PageModel> Page =new List<PageModel>();

            PrintGameDataEntities entities= new PrintGameDataEntities();

            //var GamePage=entities.GetGamePage(PageID);
            // foreach (GetGamePage_Result pageResult in GamePage)
            var GamePage = entities.Game.OrderByDescending(g => g.GameID).Skip((int)(PageID-1)*Constants.GamePerPage).Take(Constants.GamePerPage);
            
            foreach (Game pageResult in GamePage)
            {
                PageModel p=new PageModel();
                p.GameID = pageResult.GameID;
                p.TitleRu = pageResult.TitleRu;
                p.TitleEn = pageResult.TitleEn;
                p.Rating = pageResult.Rating*10;
                p.YearPublishing = pageResult.YearPublishing;
                //сокращаем до 1 абзаца описание
                int s_begin = pageResult.Descript.IndexOf("<p>")+3;
                if (s_begin != 0) pageResult.Descript = pageResult.Descript.Remove(0, s_begin);
                int s_end= pageResult.Descript.IndexOf("</p>");
                p.SmallDescript = pageResult.Descript.Remove(s_end);

                //если обзац слишком большой сокращаем еще
                if (p.SmallDescript.Length > 500) p.SmallDescript=p.SmallDescript.Remove(500) + "...</p>";

                //получаем ссылку на титульную картинку
                PrintGameDataEntities entities1 = new PrintGameDataEntities();
                var img = entities1.GetGameBoxImage(p.GameID).First();
                p.BoxImage = img.SmallImagePath;

                p.SEOGameUrl = $"/game/{p.GameID}-{Slug.Create(p.TitleEn)}";

                Page.Add(p);
            }

            ViewBag.Pangination=PagePangination.GetPangination((int)PageID, 50);
            return View(Page);
        }
    }
}