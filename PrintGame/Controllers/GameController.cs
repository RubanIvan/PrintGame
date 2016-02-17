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

            return View(model);
        }
    }
}