using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintGame.Models
{
    public class SubscribeModel
    {
        /// <summary>Список всех тегов</summary>
        public List<Tag> FullTags;

        /// <summary>Список тегов на которые подписан пользователь</summary>
        public string[] SubscribeTag;
    }
}