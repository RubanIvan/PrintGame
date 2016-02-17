using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintGame.Models;

namespace PrintGame.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index(string GameIdAndSlug)
        {
            //предварительная проверка GameId на коректность
            int GameId;
            try
            {
                GameId = int.Parse(GameIdAndSlug.Split('-').First());
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
            if(GameId <0) return HttpNotFound();

            GameModel model =new GameModel();


            PrintGameDataEntities entities = new PrintGameDataEntities();
           
            
            var query = entities.Game.Where( gm => gm.GameID == GameId).Select(g => 
                        new GameModel()
                        {
                            CatalogName = g.CatalogName,
                            TitleEn = g.TitleEn,
                            Tags = entities.GameTag.Where(t => g.GameID == t.GameID).ToList(),
                            FileShares = entities.FileShare.Where(f => g.GameID == f.GameID).ToList()

                        });

            var result = query.FirstOrDefault();

            return View(model);
        }

        //public ActionResult GetGame(int GameId)
        //{
        //    //PrintGameDataEntities entities = new PrintGameDataEntities();

        //    //var query = from g in entities.Game
        //    //    where g.GameID == GameId
        //    //    select new GameModel()
        //    //    {
        //    //        CatalogName = g.CatalogName,
        //    //        TitleEn = g.TitleEn,
        //    //        Tags = entities.GameTag.Where(t => g.GameID == t.GameID).ToList(),
        //    //        FileShares = entities.FileShare.Where(f => g.GameID == f.GameID).ToList()

        //    //    };

        //    //return view()
        //}
    }
}