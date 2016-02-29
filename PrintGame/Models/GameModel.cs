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

        /// <summary>Год издания</summary>
        public int YearPublishing;

        /// <summary> Средний рейтинг </summary>
        public float Rating;

        /// <summary>Язык игры</summary>
        public string Lang;

        /// <summary>Описание игры</summary>
        public string Descript;

        /// <summary>Количество игроков</summary>
        public string NumOfPlayers;

        /// <summary>Рекомендуемое количество игроков</summary>
        public string NumOfSuggested;

        /// <summary>Рекомендуемый возраст игроков</summary>
        public string SuggestedAges;

        /// <summary>Время освоения игры</summary>
        public string Acquaintance;

        /// <summary>Время партии </summary>
        public string PlayingTime;

        /// <summary>Состав коробки</summary>
        public string Components;

        /// <summary>Время добавления в базу</summary>
        public DateTime CreateTime;
        
        /// <summary>Список картинок</summary>
        public List<GameImage> GameImages;

        /// <summary>Список внешних ссылок на файлы</summary>
        public List<FileShare> FileShares;

        /// <summary>Список тэгов</summary>
        public List<Tag> Tags;

        //public List<GameTag> GameTags;

    }

    public class TagsModel
    {
        public int TagID;
        public string TagName;
    }

    
}