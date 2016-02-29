﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            if (GameId < 0) return HttpNotFound();

            GameModel model = new GameModel();


            PrintGameDataEntities entities = new PrintGameDataEntities();


            //var query = entities.Game.Where( gm => gm.GameID == GameId).Select(g => 
            //            new GameModel()
            //            {
            //                CatalogName = g.CatalogName,
            //                TitleEn = g.TitleEn,
            //                Tags = entities.GameTag.Where(t => g.GameID == t.GameID).ToList(),
            //                FileShares = entities.FileShare.Where(f => g.GameID == f.GameID).ToList()

            //            });

            var query = from g in entities.Game
                        where g.GameID == GameId
                        select new GameModel()
                        {
                            GameID = g.GameID,
                            TitleRu = g.TitleRu,
                            TitleEn = g.TitleEn,
                            CatalogName = g.CatalogName,
                            YearPublishing = g.YearPublishing,
                            Rating = g.Rating,
                            Lang = g.Lang,
                            Descript = g.Descript,
                            NumOfPlayers = g.NumOfPlayers,
                            NumOfSuggested = g.NumOfSuggested,
                            SuggestedAges = g.SuggestedAges,
                            Acquaintance = g.Acquaintance,
                            PlayingTime = g.PlayingTime,
                            Components = g.Components,
                            CreateTime = g.CreateTime,

                            GameImages = entities.GameImage.Where(i => g.GameID == i.GameID).OrderBy(i => i.GameImageID).ToList(),
                            FileShares = entities.FileShare.Where(f => g.GameID == f.GameID).OrderBy(f => f.FileShareID).ToList(),

                            Tags = (from t in entities.Tag
                                    join gt in entities.GameTag on t.TagID equals gt.TagID
                                    where gt.GameID == GameId
                                    orderby gt.GameTagID
                                    select t).ToList()

                            //GameTags = entities.GameTag.Where(ggt=>ggt.GameID==GameId).ToList(),



                        };

            //var q1 = from t in entities.Tag
            //    from gt in entities.GameTag
            //    where t.TagID == gt.GameID
            //    select new TagsModel()
            //    {
            //        TagId = t.TagID,
            //        TagName = t.TagName
            //    };


            model = query.FirstOrDefault();
            
            return View(model);
        }


    }
}