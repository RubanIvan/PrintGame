using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintGame.Models
{
    public class GameModel
    {
        /// <summary>идентификатор игры</summary>
        public int GameID;

        /// <summary>Название по русски</summary>
        public string TitleRu;

        /// <summary>Название по английски</summary>
        public string TitleEn;

        /// <summary>Каталог с игрой</summary>
        public string CatalogName;

        public List<GameImage> GameImages;

        public List<FileShare> FileShares;

        public List<GameTag> Tags;


    }
}