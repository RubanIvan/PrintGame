using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintGame.Models
{
    public class PageModel
    {
        /// <summary>идентификатор игры</summary>
        public int GameID;

        /// <summary>Название по русски</summary>
        public string TitleRu;

        /// <summary>Название по английски</summary>
        public string TitleEn;

        /// <summary>Каталог с игрой</summary>
        public string CatalogName;

        /// <summary>Урезаное описание игры</summary>
        public string SmallDescript;

        /// <summary>Путь до титульной картинки</summary>
        public string BoxImage;

        //    NumOfPlayers nvarchar(512),						--Количество игроков
        //    NumOfSuggested nvarchar(512),						--Рекомендуемое количество игроков
        //   SuggestedAges   nvarchar(512),						--Рекомендуемый возраст игроков
        //   Acquaintance    nvarchar(512),						--Время освоения игры
        //   PlayingTime     nvarchar(512),						--Время партии
        //    Components nvarchar(MAX) NOT NULL,             --Состав коробки
        //    CreateTime SmallDateTime NOT NULL				--Время добавления
    }
}